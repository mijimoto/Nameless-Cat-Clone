using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkinMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Scroll Settings")]
    private const float offset = 200f; // Distance between skin items
    public float snapForce = 0.5f;
    public float snapSpeed = 10f;
    
    [Header("UI References")]
    public GameObject holder; // SkinMeunContent - the outer container
    public Transform skinsHolder; // The 'skins' container that holds individual skin objects
    public Button useButton;
    public Button buyButton;
    public TextMeshProUGUI buyButtonText;
    public Button buyRealButton;
    public TextMeshProUGUI buyRealButtonText;
    
    [Header("Navigation Buttons (Optional)")]
    public Button nextButton;
    public Button prevButton;
    
    private int currentSkinBox;
    private int maxSkinBox;
    private SkinBlock[] skinBoxs;
    public GameObject loadingObj;
    private bool loading;
    
    // Scrolling variables
    private bool isDragging = false;
    private Vector2 startDragPosition;
    private RectTransform skinsRectTransform;
    private float targetPosition;
    private bool isSnapping = false;

    private void Start()
    {
        loading = false;
        skinsRectTransform = skinsHolder != null ? skinsHolder.GetComponent<RectTransform>() : null;
        
        initSkin();
        setupButtons();
        updateAllSkinUI();
        
        // Set initial position (center the saved index)
        currentSkinBox = PlayerPrefs.GetInt("CurrentSkinIndex", 0);
        currentSkinBox = Mathf.Clamp(currentSkinBox, 0, maxSkinBox - 1);

        // Normalize child positions so currentSkinBox is centered (x = 0)
        if (skinBoxs != null && skinBoxs.Length > 0)
        {
            for (int i = 0; i < skinBoxs.Length; i++)
            {
                RectTransform rt = skinBoxs[i].GetComponent<RectTransform>();
                if (rt == null) continue;
                rt.anchoredPosition = new Vector2((i - currentSkinBox) * offset, rt.anchoredPosition.y);
            }
        }
    }

    private void setupButtons()
    {
        // Clear existing listeners
        if (useButton != null) useButton.onClick.RemoveAllListeners();
        if (buyButton != null) buyButton.onClick.RemoveAllListeners();
        if (buyRealButton != null) buyRealButton.onClick.RemoveAllListeners();
        if (nextButton != null) nextButton.onClick.RemoveAllListeners();
        if (prevButton != null) prevButton.onClick.RemoveAllListeners();

        // Setup button listeners
        if (useButton != null) useButton.onClick.AddListener(UseSkin);
        if (buyButton != null) buyButton.onClick.AddListener(buySkin);
        if (buyRealButton != null) buyRealButton.onClick.AddListener(buySkinWithBox);
        if (nextButton != null) nextButton.onClick.AddListener(NextSkin);
        if (prevButton != null) prevButton.onClick.AddListener(PreviousSkin);
    }

    private void UseSkin()
    {
        if (skinBoxs == null || skinBoxs.Length == 0) return;
        int idx = Mathf.Clamp(currentSkinBox, 0, skinBoxs.Length - 1);
        var sb = skinBoxs[idx];
        if (sb != null)
        {
            PlayerPrefs.SetInt("CurrentSkinIndex", idx);
            PlayerPrefs.Save();
            updateAllSkinUI();
            Debug.Log($"Using skin {idx}");
        }
    }

    private void initSkin()
    {
        if (skinsHolder == null) return;
        skinBoxs = skinsHolder.GetComponentsInChildren<SkinBlock>(true);
        maxSkinBox = skinBoxs != null ? skinBoxs.Length : 0;

        for (int i = 0; i < maxSkinBox; i++)
        {
            SkinBlock sb = skinBoxs[i];
            if (sb == null) continue;

            int skinIndex = 0;
            string goName = sb.gameObject.name;
            Match m = Regex.Match(goName, @"\d+");
            if (m.Success)
            {
                int.TryParse(m.Value, out skinIndex);
            }
            else
            {
                skinIndex = i;
            }

            Skin s = ScriptableObject.CreateInstance<Skin>();
            s.id = skinIndex;
            s.useIAP = (skinIndex % 3 == 0 && skinIndex > 0); // Example: every 3rd skin is IAP
            s.canBuy = true;
            s.canUse = PlayerPrefs.GetInt($"SkinOwned_{s.id}", skinIndex == 0 ? 1 : 0) == 1; // First skin owned by default
            s.price = skinIndex * 5 + 5; // Progressive pricing

            sb.LoadSkin(s);

            // Immediately set child positions relative to index i (we'll re-center later in Start)
            RectTransform rt = sb.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = new Vector2(i * offset, rt.anchoredPosition.y);
            }
        }
    }

    // Drag handling for smooth scrolling — now moves children instead of parent
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        startDragPosition = eventData.position;
        isSnapping = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || skinBoxs == null || skinBoxs.Length == 0) return;
        
        Vector2 dragDelta = eventData.position - startDragPosition;
        // move each child's anchoredPosition by delta.x (damped to maintain feel)
        float damp = 0.6f;
        for (int i = 0; i < skinBoxs.Length; i++)
        {
            var rt = skinBoxs[i].GetComponent<RectTransform>();
            if (rt == null) continue;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + dragDelta.x * damp, rt.anchoredPosition.y);
        }
        
        startDragPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;
        
        // Find closest skin and snap to it (based on child's proximity to center x=0)
        int closestSkin = GetClosestSkinIndex();
        snapToSkin(closestSkin, true);
    }

    private int GetClosestSkinIndex()
    {
        if (skinBoxs == null || skinBoxs.Length == 0) return currentSkinBox;
        
        // find which child RectTransform is closest to x == 0 (center)
        int bestIdx = 0;
        float bestDist = Mathf.Abs(GetRectX(skinBoxs[0]));
        for (int i = 1; i < skinBoxs.Length; i++)
        {
            float d = Mathf.Abs(GetRectX(skinBoxs[i]));
            if (d < bestDist)
            {
                bestDist = d;
                bestIdx = i;
            }
        }

        return Mathf.Clamp(bestIdx, 0, maxSkinBox - 1);
    }

    // Helpers to get/set child RectTransform anchoredPosition.x
    private float GetRectX(SkinBlock sb)
    {
        var rt = sb.GetComponent<RectTransform>();
        return rt != null ? rt.anchoredPosition.x : 0f;
    }

    private void SetRectX(SkinBlock sb, float x)
    {
        var rt = sb.GetComponent<RectTransform>();
        if (rt != null) rt.anchoredPosition = new Vector2(x, rt.anchoredPosition.y);
    }

    public void NextSkin()
    {
        if (maxSkinBox <= 0) return;
        int nextIndex = (currentSkinBox + 1) % maxSkinBox;
        snapToSkin(nextIndex, true);
    }

    public void PreviousSkin()
    {
        if (maxSkinBox <= 0) return;
        int prevIndex = (currentSkinBox - 1 + maxSkinBox) % maxSkinBox;
        snapToSkin(prevIndex, true);
    }

    private void snapToSkin(int skinIndex, bool animated = true)
    {
        if (skinBoxs == null || skinBoxs.Length == 0) return;
        
        skinIndex = Mathf.Clamp(skinIndex, 0, maxSkinBox - 1);
        currentSkinBox = skinIndex;
        targetPosition = -currentSkinBox * offset; // kept for compatibility though parent isn't moved now
        
        // compute desired target x for each child: (i - skinIndex) * offset
        Vector2[] targets = new Vector2[skinBoxs.Length];
        for (int i = 0; i < skinBoxs.Length; i++)
        {
            targets[i] = new Vector2((i - skinIndex) * offset, skinBoxs[i].GetComponent<RectTransform>().anchoredPosition.y);
        }

        if (animated)
        {
            StartCoroutine(SmoothSnapToPosition(targets));
        }
        else
        {
            for (int i = 0; i < skinBoxs.Length; i++)
            {
                SetRectX(skinBoxs[i], targets[i].x);
            }
            // normalize positions to multiples exactly to avoid halfed visuals
            NormalizePositions(skinIndex);
            updateAllSkinUI();
        }
    }

    // Smoothly animate each child to its desired anchoredPosition
    private IEnumerator SmoothSnapToPosition(Vector2[] targets)
    {
        isSnapping = true;
        float elapsed = 0f;
        float duration = 0.28f;

        Vector2[] starts = new Vector2[skinBoxs.Length];
        for (int i = 0; i < skinBoxs.Length; i++)
        {
            var rt = skinBoxs[i].GetComponent<RectTransform>();
            starts[i] = rt != null ? rt.anchoredPosition : Vector2.zero;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
            for (int i = 0; i < skinBoxs.Length; i++)
            {
                var rt = skinBoxs[i].GetComponent<RectTransform>();
                if (rt == null) continue;
                rt.anchoredPosition = Vector2.Lerp(starts[i], targets[i], t);
            }
            yield return null;
        }

        // final snap to exact targets
        for (int i = 0; i < skinBoxs.Length; i++)
        {
            SetRectX(skinBoxs[i], targets[i].x);
        }

        // Normalize so children positions are exact multiples and keep indexing consistent
        NormalizePositions(currentSkinBox);

        isSnapping = false;
        updateAllSkinUI();
    }

    // Ensure children positions are exact multiples of offset and optionally rotate the array for looping feel.
    private void NormalizePositions(int centerIndex)
    {
        // After positioning, ensure each child is exactly (i - centerIndex)*offset.
        for (int i = 0; i < skinBoxs.Length; i++)
        {
            SetRectX(skinBoxs[i], (i - centerIndex) * offset);
        }

        // To avoid drift from repeated moves, make sure anchoredPositions are tidy and the numeric center index is correct.
        currentSkinBox = Mathf.Clamp(centerIndex, 0, maxSkinBox - 1);
    }

    public void updateAllSkinUI()
    {
        if (skinBoxs == null) return;
        for (int i = 0; i < skinBoxs.Length; i++)
        {
            // compute shiftIndex based on actual anchoredPosition.x rounded to nearest slot
            int shiftIndex = 0;
            var rt = skinBoxs[i].GetComponent<RectTransform>();
            if (rt != null)
            {
                shiftIndex = Mathf.RoundToInt(rt.anchoredPosition.x / offset);
            }
            // shiftIndex: 0 means centered, negative -> left of center, positive -> right of center
            updateSkinUI(i, -shiftIndex); // invert so shiftIndex==0 => center
        }
    }

    private void updateSkinUI(int skinBoxIndex, int shiftIndex)
    {
        if (skinBoxs == null || skinBoxIndex < 0 || skinBoxIndex >= skinBoxs.Length) return;
        var block = skinBoxs[skinBoxIndex];

        // Visual emphasis: scale center element
        float scale = (shiftIndex == 0) ? 1.2f : 0.8f;
        float alpha = (shiftIndex == 0) ? 1f : 0.6f;
        
        block.transform.localScale = Vector3.one * scale;
        
        // Update alpha if possible
        CanvasGroup cg = block.GetComponent<CanvasGroup>();
        if (cg == null) cg = block.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = alpha;

        // Update global buttons only for selected (center) element
        if (shiftIndex == 0)
        {
            Skin s = GetSkinByBlock(block);
            bool owned = s != null && s.canUse;
            bool purchasable = s != null && s.canBuy;

            if (useButton != null) useButton.gameObject.SetActive(owned);
            if (buyButton != null) buyButton.gameObject.SetActive(!owned && purchasable && !s.useIAP);
            if (buyRealButton != null) buyRealButton.gameObject.SetActive(!owned && purchasable && s.useIAP);

            if (buyButtonText != null) buyButtonText.text = s != null ? $"Buy ({s.price})" : "Buy";
            if (buyRealButtonText != null) buyRealButtonText.text = s != null ? (s.useIAP ? "Buy (IAP)" : $"Buy ({s.price})") : "Buy";
        }
    }

    public void buySkinWithBox()
    {
        if (skinBoxs == null) return;
        int idx = Mathf.Clamp(currentSkinBox, 0, skinBoxs.Length - 1);
        var block = skinBoxs[idx];
        Skin s = GetSkinByBlock(block);
        if (s == null) return;

        if (s.useIAP)
        {
            LoadAndLock(true);
            // Simulate IAP process
            Invoke(nameof(SimulateIAPSuccess), 1f);
        }
        else
        {
            buySkin();
        }
    }

    private void SimulateIAPSuccess()
    {
        finishBuy(true);
    }

    public void buySkin()
    {
        if (skinBoxs == null) return;
        int idx = Mathf.Clamp(currentSkinBox, 0, skinBoxs.Length - 1);
        var block = skinBoxs[idx];
        Skin s = GetSkinByBlock(block);
        if (s == null) return;

        if (s.useIAP)
        {
            buySkinWithBox();
            return;
        }

        if (!s.canBuy)
        {
            Debug.Log("This skin is not purchasable.");
            return;
        }

        int playerFood = PlayerPrefs.GetInt("CatFood", 100); // Start with some money for testing
        if (playerFood >= s.price)
        {
            playerFood -= s.price;
            PlayerPrefs.SetInt("CatFood", playerFood);
            PlayerPrefs.Save();

            s.setCanUse(true);
            PlayerPrefs.SetInt($"SkinOwned_{s.id}", 1);
            PlayerPrefs.Save();

            updateAllSkinUI();
            Debug.Log($"Bought skin {s.id} for {s.price} cat food. Remaining: {playerFood}");
        }
        else
        {
            Debug.Log($"Not enough cat food. Need {s.price}, have {playerFood}");
        }
    }

    private void LoadAndLock(bool startLoad)
    {
        loading = startLoad;
        if (loadingObj != null) loadingObj.SetActive(startLoad);
    }

    public void finishBuy(bool success = false)
    {
        LoadAndLock(false);
        if (!success)
        {
            Debug.Log("Purchase failed / canceled.");
            return;
        }

        if (skinBoxs == null) return;
        int idx = Mathf.Clamp(currentSkinBox, 0, skinBoxs.Length - 1);
        var block = skinBoxs[idx];
        Skin s = GetSkinByBlock(block);
        if (s == null) return;

        s.setCanUse(true);
        PlayerPrefs.SetInt($"SkinOwned_{s.id}", 1);
        PlayerPrefs.Save();

        updateAllSkinUI();
        Debug.Log($"Successfully purchased skin {s.id}!");
    }

    private Skin GetSkinByBlock(SkinBlock block)
    {
        int skinIndex = 0;
        string goName = block.gameObject.name;
        var m = Regex.Match(goName, @"\d+");
        if (m.Success) int.TryParse(m.Value, out skinIndex);
        
        Skin s = ScriptableObject.CreateInstance<Skin>();
        s.id = skinIndex;
        s.canUse = PlayerPrefs.GetInt($"SkinOwned_{s.id}", skinIndex == 0 ? 1 : 0) == 1;
        s.canBuy = true;
        s.useIAP = (skinIndex % 3 == 0 && skinIndex > 0);
        s.price = skinIndex * 5 + 5;
        return s;
    }
}
