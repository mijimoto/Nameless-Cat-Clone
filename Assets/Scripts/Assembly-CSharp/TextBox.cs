using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI msgText;

    [SerializeField]
    private Animator animator;

    public Color TextColor
    {
        get
        {
            return msgText != null ? msgText.color : Color.white;
        }
        set
        {
            if (msgText != null)
                msgText.color = value;
        }
    }

    private void Awake()
    {
        // If msgText is not assigned, try to find it
        if (msgText == null)
        {
            // First, try to find it as a direct child
            msgText = GetComponentInChildren<TextMeshProUGUI>();
            
            // If still not found, search more specifically
            if (msgText == null)
            {
                Transform textBoxBody = transform.Find("TextBox2Body");
                if (textBoxBody != null)
                {
                    msgText = textBoxBody.GetComponentInChildren<TextMeshProUGUI>();
                }
            }
            
            Debug.Log($"TextBox msgText found: {msgText != null}");
        }

        // If animator is not assigned, try to find it
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            Debug.Log($"TextBox animator found: {animator != null}");
        }
    }

    public void Init(TextBoxSetting textBoxSetting)
    {
        Debug.Log($"TextBox Init called with setting: {textBoxSetting != null}");
        
        if (textBoxSetting != null && msgText != null)
        {
            textBoxSetting.InitTextMeshProUGUI(msgText);
            Debug.Log("TextBox setting applied successfully");
        }
        else
        {
            Debug.LogWarning($"TextBox Init failed - Setting: {textBoxSetting != null}, MsgText: {msgText != null}");
            
            // Fallback: set basic text properties
            if (msgText != null)
            {
                msgText.fontSize = 18f;
                msgText.color = Color.white;
                msgText.alignment = TextAlignmentOptions.TopLeft;
                Debug.Log("Applied fallback text settings");
            }
        }
    }

    public void UpdateText(string msg)
    {
        Debug.Log($"UpdateText called with: {msg}");
        
        if (msgText != null)
        {
            msgText.text = msg;
            Debug.Log($"Text updated successfully. Current text: {msgText.text}");
        }
        else
        {
            Debug.LogError("msgText is null! Cannot update text.");
            
            // Try to find msgText again
            msgText = GetComponentInChildren<TextMeshProUGUI>();
            if (msgText != null)
            {
                msgText.text = msg;
                Debug.Log("Found msgText and updated text successfully");
            }
        }
    }

    public void playAnimation(bool inverse)
    {
        if (animator != null)
        {
            if (inverse)
            {
                animator.SetTrigger("Hide");
                Debug.Log("Playing Hide animation");
            }
            else
            {
                animator.SetTrigger("Show");
                Debug.Log("Playing Show animation");
            }
        }
        else
        {
            Debug.LogWarning("Animator is null, cannot play animation");
        }
    }

    public void RefreshSettings()
    {
        TextBoxSetting currentSetting = DataLoader.getCurrentTextBoxSetting();
        if (currentSetting != null)
        {
            Init(currentSetting);
        }
    }

    // Debug method to check TextBox state
    public void DebugTextBoxState()
    {
        Debug.Log("=== TextBox Debug Info ===");
        Debug.Log($"GameObject name: {gameObject.name}");
        Debug.Log($"GameObject active: {gameObject.activeInHierarchy}");
        Debug.Log($"msgText assigned: {msgText != null}");
        Debug.Log($"animator assigned: {animator != null}");
        
        if (msgText != null)
        {
            Debug.Log($"msgText text: '{msgText.text}'");
            Debug.Log($"msgText enabled: {msgText.enabled}");
            Debug.Log($"msgText gameObject active: {msgText.gameObject.activeInHierarchy}");
            Debug.Log($"msgText fontSize: {msgText.fontSize}");
            Debug.Log($"msgText color: {msgText.color}");
        }
        
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
        {
            Debug.Log($"RectTransform position: {rect.anchoredPosition}");
            Debug.Log($"RectTransform size: {rect.sizeDelta}");
            Debug.Log($"RectTransform anchors: min{rect.anchorMin} max{rect.anchorMax}");
        }
        
        Debug.Log("=== End TextBox Debug ===");
    }
}