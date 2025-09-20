using TMPro;
using UnityEngine;

public class LocalizationText : MonoBehaviour
{
    [SerializeField]
    private string textKey;
    
    [SerializeField]
    private TextMeshProUGUI textComponent;
    
    [SerializeField]
    private bool updateFontSize;
    
    [SerializeField]
    private bool overrideEngFont;
    
    // Store if this is a stat text that needs formatting
    [SerializeField]
    private bool isStatText;
    
    private string localizedText;
    private string formattedValue;

    private void Awake()
    {
        if (textComponent == null)
            textComponent = GetComponent<TextMeshProUGUI>();
            
        RefreshLocalization();
    }

    private void Start()
    {
        // Additional refresh in Start to ensure everything is loaded
        RefreshLocalization();
    }

    public void SetKey(string key)
    {
        textKey = key;
        RefreshLocalization();
    }

    public void UpdateValue(string param)
    {
        formattedValue = param;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (textComponent == null) return;

        if (!string.IsNullOrEmpty(localizedText))
        {
            // For stat texts, combine title and value
            if (isStatText && !string.IsNullOrEmpty(formattedValue))
            {
                textComponent.text = $"{localizedText}: {formattedValue}";
            }
            else if (!string.IsNullOrEmpty(formattedValue))
            {
                // For other texts that use formatting
                textComponent.text = string.Format(localizedText, formattedValue);
            }
            else
            {
                // Just display the localized text
                textComponent.text = localizedText;
            }
        }
    }

    public void CleanText()
    {
        if (textComponent != null)
            textComponent.text = "";
    }

    public void RefreshLocalization()
    {
        if (string.IsNullOrEmpty(textKey) || textComponent == null)
            return;

        // Get the localized text from the resource file
        localizedText = TextLoader.getText(textKey);
        
        if (string.IsNullOrEmpty(localizedText))
        {
            // If text not found, use the key as fallback
            localizedText = textKey;
        }

        UpdateDisplay();

        // Apply font settings
        TextBoxSetting currentSetting = DataLoader.getCurrentTextBoxSetting();
        if (currentSetting != null && !overrideEngFont)
        {
            currentSetting.InitTextMeshProUGUI(textComponent, updateFontSize);
        }
    }

    public void SetAsStatText(bool isStat)
    {
        isStatText = isStat;
    }
}