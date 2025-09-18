using TMPro;
using UnityEngine;

public class StaticsItemController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private bool updateFontSize = true;
    
    private float value;

    private void Start()
    {
        // Initialize with current text box settings
        TextBoxSetting setting = DataLoader.getCurrentTextBoxSetting();
        if (setting != null && textComponent != null)
        {
            setting.InitTextMeshProUGUI(textComponent, updateFontSize);
        }
    }

    public void Init(TextBoxSetting textBoxSetting, string text)
    {
        if (textComponent == null) return;

        // Apply text box settings (font, size, spacing, etc.)
        if (textBoxSetting != null)
        {
            textBoxSetting.InitTextMeshProUGUI(textComponent, updateFontSize);
        }

        // Set the text
        UpdateText(text);
    }

    public void UpdateText(string text)
    {
        if (textComponent != null)
        {
            textComponent.text = text;
        }

        // Try to parse numeric value for potential future use
        string numericText = text.Replace("s", "").Replace("m", "").Replace(".", "");
        if (float.TryParse(numericText, out float parsedValue))
        {
            value = parsedValue;
        }
    }

    public float GetValue()
    {
        return value;
    }

    public void RefreshTextSettings()
    {
        TextBoxSetting setting = DataLoader.getCurrentTextBoxSetting();
        if (setting != null && textComponent != null)
        {
            setting.InitTextMeshProUGUI(textComponent, updateFontSize);
        }
    }
}