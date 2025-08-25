using UnityEngine;
using UnityEngine.UI;

public class LanguageButtonController : MonoBehaviour
{
    [SerializeField]
    private Button languageButton;
    
    [SerializeField]
    private Text languageText;
    
    private string targetLanKey;
    public Action<string> OnChangeLanauge;

    public void Init(TextBoxSetting textBoxSetting, Action<string> changeLanguageAction)
    {
        if (textBoxSetting == null)
        {
            Debug.LogError("TextBoxSetting is null in LanguageButtonController.Init");
            return;
        }

        targetLanKey = textBoxSetting.languageKey;
        OnChangeLanauge = changeLanguageAction;

        // Set button text to language name
        if (languageText != null)
        {
            languageText.text = textBoxSetting.languageName;
        }

        // Setup button click handler
        if (languageButton != null)
        {
            languageButton.onClick.AddListener(ChangeLanguage);
        }

        // Highlight if this is the current language
        UpdateButtonState();
    }

    private void ChangeLanguage()
    {
        OnChangeLanauge?.Invoke(targetLanKey);
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (languageButton != null)
        {
            bool isCurrentLanguage = (targetLanKey == DataLoader.usingFileLan);
            
            // You can customize the visual feedback here
            // For example, change button color or add a checkmark
            var colors = languageButton.colors;
            colors.normalColor = isCurrentLanguage ? Color.green : Color.white;
            languageButton.colors = colors;
        }
    }

    private void Awake()
    {
        // Ensure components are assigned
        if (languageButton == null)
            languageButton = GetComponent<Button>();
        
        if (languageText == null)
            languageText = GetComponentInChildren<Text>();
    }

    private void OnDestroy()
    {
        if (languageButton != null)
        {
            languageButton.onClick.RemoveListener(ChangeLanguage);
        }
    }
}