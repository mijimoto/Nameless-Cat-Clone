using TMPro;
using UnityEngine;

public class LocalizationText : MonoBehaviour
{
    [SerializeField]
    private string textKey;

    [SerializeField]
    private TextMeshProUGUI textComponent;

    [SerializeField]
    private bool updateFontSize = true;

    [SerializeField]
    private bool overrideEngFont = false;

    private string localizedText;
    private string originalKey;

    private void Awake()
    {
        // Ensure text component is assigned
        if (textComponent == null)
            textComponent = GetComponent<TextMeshProUGUI>();

        originalKey = textKey;
        
        // Subscribe to language change events if needed
        // UpdateValue will be called by DataLoader when language changes
    }

    private void Start()
    {
        // Initial localization
        if (!string.IsNullOrEmpty(textKey))
        {
            UpdateValue("");
        }
    }

    public void SetKey(string key)
    {
        textKey = key;
        originalKey = key;
        UpdateValue("");
    }

    public void UpdateValue(string param)
    {
        if (textComponent == null)
            return;

        // Get localized text from TextLoader
        string localizedText = TextLoader.getText(textKey);
        
        // Replace parameter if provided
        if (!string.IsNullOrEmpty(param))
        {
            localizedText = localizedText.Replace("{0}", param);
        }
        
        textComponent.text = localizedText;

        // Apply current language settings
        var currentTextBoxSetting = DataLoader.getCurrentTextBoxSetting();
        if (currentTextBoxSetting != null)
        {
            // Update font and styling based on current language
            ApplyTextBoxSettings(currentTextBoxSetting);
        }
    }

    private void ApplyTextBoxSettings(TextBoxSetting setting)
    {
        if (textComponent == null || setting == null)
            return;

        // Apply font settings using the TextBoxSetting method
        setting.InitTextMeshProUGUI(textComponent, updateFontSize);

        // Handle English font override if needed
        if (DataLoader.usingFileLan == DataLoader.DEFAULT_LANGUAGE_KEY && overrideEngFont)
        {
            // You might need to implement additional logic here for English font override
            // This would depend on your specific requirements
        }
    }

    public void CleanText()
    {
        if (textComponent != null)
        {
            textComponent.text = "";
        }
        localizedText = "";
    }

    // Helper method to refresh localization (can be called by other scripts)
    public void RefreshLocalization()
    {
        UpdateValue("");
    }
}