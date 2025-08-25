using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI msgText;
    
    [SerializeField]
    private Animator animator;

    private TextBoxSetting currentSetting;

    public Color TextColor
    {
        get
        {
            if (msgText != null)
                return msgText.color;
            return Color.white;
        }
        set
        {
            if (msgText != null)
                msgText.color = value;
        }
    }

    private void Awake()
    {
        // Ensure msgText is assigned
        if (msgText == null)
            msgText = GetComponent<TextMeshProUGUI>();
            
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void Init(TextBoxSetting textBoxSetting)
    {
        currentSetting = textBoxSetting;
        
        if (textBoxSetting != null && msgText != null)
        {
            // Apply the text box settings to the TextMeshPro component
            textBoxSetting.InitTextMeshProUGUI(msgText, true);
        }
    }

    public void UpdateText(string msg)
    {
        if (msgText != null)
        {
            msgText.text = msg;
        }
    }

    public void playAnimation(bool inverse)
    {
        if (animator != null)
        {
            if (inverse)
            {
                animator.SetTrigger("Hide");
            }
            else
            {
                animator.SetTrigger("Show");
            }
        }
    }

    // Method to update text with localization key
    public void UpdateLocalizedText(string key)
    {
        string localizedText = TextLoader.getText(key);
        UpdateText(localizedText);
    }

    // Method to refresh settings when language changes
    public void RefreshSettings()
    {
        if (currentSetting != null)
        {
            Init(currentSetting);
        }
    }
}