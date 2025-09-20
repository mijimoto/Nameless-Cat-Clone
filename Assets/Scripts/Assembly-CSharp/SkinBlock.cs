using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkinBlock : MonoBehaviour
{
    [SerializeField] private Image catImage;
    [SerializeField] private Image bgImage;
    [SerializeField] private GameObject priceCanObj;
    [SerializeField] private GameObject priceIAPObj;
    [SerializeField] private TextMeshProUGUI priceCanText;
    [SerializeField] private TextMeshProUGUI priceIAPText;
    [SerializeField] private TextMeshProUGUI priceText; // The main PriceText object

    // cached skin object assigned via LoadSkin
    private Skin skinData;

    public void LoadSkin(Skin skinData)
    {
        this.skinData = skinData;

        if (skinData == null)
        {
            // nothing to load
            return;
        }

        // Report achievement for new look (keep your existing achievement unlock)
        Social.ReportProgress(GPGSIds.achievement_i_love_new_look, 100.0f, success => {
            Debug.Log("Achievement unlocked: " + success);
        });

        // UI visibility
        if (priceCanObj != null) priceCanObj.SetActive(!skinData.useIAP && !skinData.canUse);
        if (priceIAPObj != null) priceIAPObj.SetActive(skinData.useIAP && !skinData.canUse);

        // Set price texts
        if (priceCanText != null) 
        {
            priceCanText.text = skinData.price.ToString();
        }
        
        if (priceIAPText != null) 
        {
            if (skinData.useIAP)
            {
                // Try to get real price from Google Play
                string realPrice = "$0.99"; // fallback
                if (Purchaser._instance != null && Purchaser._instance.IsInitialized())
                {
                    realPrice = Purchaser._instance.getProductPrice(skinData.id);
                }
                priceIAPText.text = realPrice;
            }
            else
            {
                priceIAPText.text = "IAP";
            }
        }

        // Update the main PriceText to show current player money vs required
        UpdatePriceText();

        // Try to map the current catImage sprite (base) to the skin variant using your naming rules.
        // base example: "player_jump(white)_0" -> skin example: "player_jump_skin_3_0"
        if (catImage != null)
        {
            Sprite baseSprite = catImage.sprite;
            Sprite newSprite = null;

            if (baseSprite != null)
            {
                string baseName = baseSprite.name; // e.g., player_run(white)_1
                // parse frame suffix (underscore + frame)
                int lastUnderscore = baseName.LastIndexOf('_');
                string frame = "0";
                string prefix = baseName;
                if (lastUnderscore >= 0 && lastUnderscore < baseName.Length - 1)
                {
                    frame = baseName.Substring(lastUnderscore + 1);
                    prefix = baseName.Substring(0, lastUnderscore);
                }

                // remove "(white)" or other parentheses if present
                prefix = Regex.Replace(prefix, @"\([^\)]*\)", "");

                // build candidate name
                string candidateName = $"{prefix}_skin_{skinData.id}_{frame}";

                // Try runtime Resources first
#if UNITY_EDITOR
                // In editor, try AssetDatabase lookup in Assets/Sprite for convenience
                string[] guids = AssetDatabase.FindAssets(candidateName + " t:Sprite", new[] { "Assets/Sprite" });
                if (guids != null && guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    newSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                }
#endif
                if (newSprite == null)
                {
                    // Try Resources
                    Sprite[] loaded = Resources.LoadAll<Sprite>("Sprite");
                    if (loaded != null)
                    {
                        for (int i = 0; i < loaded.Length; i++)
                        {
                            if (loaded[i] != null && loaded[i].name == candidateName)
                            {
                                newSprite = loaded[i];
                                break;
                            }
                        }
                    }
                }
            }

            // If found, assign; otherwise fall back to a sprite stored in skinData.sprite or keep base
            if (newSprite != null)
            {
                catImage.sprite = newSprite;
            }
            else if (skinData.sprite != null)
            {
                catImage.sprite = skinData.sprite;
            }
            // else keep whatever is set in the scene (base)
        }

        // Update visual state based on ownership
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (skinData == null) return;

        // Dim the image if not owned
        if (catImage != null)
        {
            Color imageColor = catImage.color;
            imageColor.a = skinData.canUse ? 1f : 0.5f;
            catImage.color = imageColor;
        }

        // Update background color or add owned indicator
        if (bgImage != null)
        {
            Color bgColor = bgImage.color;
            if (skinData.canUse)
            {
                bgColor = Color.green; // Owned indicator
            }
            else if (skinData.useIAP)
            {
                bgColor = Color.yellow; // IAP indicator
            }
            else
            {
                bgColor = Color.white; // Default
            }
            bgColor.a = 0.3f; // Keep it subtle
            bgImage.color = bgColor;
        }
    }

    // Call this when ownership status changes (from SkinMenu)
    public void RefreshSkinDisplay()
    {
        if (skinData != null)
        {
            // Refresh ownership status
            if (skinData.useIAP && Purchaser._instance != null)
            {
                skinData.canUse = Purchaser._instance.HasSkin(skinData.id);
            }
            else
            {
                skinData.canUse = PlayerPrefs.GetInt($"SkinOwned_{skinData.id}", skinData.id == 0 ? 1 : 0) == 1;
            }

            UpdateVisualState();
            UpdatePriceText();

            // Update price display visibility
            if (priceCanObj != null) priceCanObj.SetActive(!skinData.useIAP && !skinData.canUse);
            if (priceIAPObj != null) priceIAPObj.SetActive(skinData.useIAP && !skinData.canUse);
        }
    }

    private void UpdatePriceText()
    {
        if (priceText == null || skinData == null) return;

        if (skinData.canUse)
        {
            // Already owned
            priceText.text = "OWNED";
            priceText.color = Color.green;
        }
        else if (skinData.useIAP)
        {
            // IAP skin - show Google Play price
            string realPrice = "$0.99"; // fallback
            if (Purchaser._instance != null && Purchaser._instance.IsInitialized())
            {
                realPrice = Purchaser._instance.getProductPrice(skinData.id);
            }
            priceText.text = realPrice;
            priceText.color = Color.yellow;
        }
        else
        {
            // Regular skin - show player currency vs required
            int playerMoney = PlayerPrefs.GetInt("CatFood", 0);
            int required = skinData.price;
            
            if (playerMoney >= required)
            {
                // Can afford it
                priceText.text = $"{playerMoney}/{required} 🐟";
                priceText.color = Color.green;
            }
            else
            {
                // Can't afford it
                int needed = required - playerMoney;
                priceText.text = $"Need {needed} more 🐟";
                priceText.color = Color.red;
            }
        }
    }

    // Auto-find PriceText if not assigned in inspector
    private void Start()
    {
        if (priceText == null)
        {
            // Try to find PriceText in children
            Transform priceTextTransform = transform.Find("PriceHolder/PriceText");
            if (priceTextTransform == null)
            {
                // Try recursive search
                priceTextTransform = FindChildByName(transform, "PriceText");
            }
            
            if (priceTextTransform != null)
            {
                priceText = priceTextTransform.GetComponent<TextMeshProUGUI>();
            }
        }
    }

    private Transform FindChildByName(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name == name)
                return child;
            
            Transform found = FindChildByName(child, name);
            if (found != null)
                return found;
        }
        return null;
    }
}