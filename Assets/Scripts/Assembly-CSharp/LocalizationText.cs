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
    
    private string localizedText;

    private void Awake()
    {
        if (textComponent == null)
            textComponent = GetComponent<TextMeshProUGUI>();
            
        RefreshLocalization();
    }

    public void SetKey(string key)
    {
        textKey = key;
        RefreshLocalization();
    }

    public void UpdateValue(string param)
    {
        if (!string.IsNullOrEmpty(localizedText))
        {
            string formattedText = string.Format(localizedText, param);
            if (textComponent != null)
                textComponent.text = formattedText;
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

        localizedText = TextLoader.getText(textKey);
        textComponent.text = localizedText;

        TextBoxSetting currentSetting = DataLoader.getCurrentTextBoxSetting();
        if (currentSetting != null && !overrideEngFont)
        {
            currentSetting.InitTextMeshProUGUI(textComponent, updateFontSize);
        }
    }
}