using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New TextBox Setting", menuName = "Localization/TextBox Setting")]
public class TextBoxSetting : ScriptableObject
{
    [Header("Language Information")]
    [SerializeField]
    private string lanKey;
    
    [SerializeField]
    private string lanName;
    
    [SerializeField]
    private SystemLanguage[] applySystemLanguages;

    [Header("Font Settings")]
    [SerializeField]
    private TMP_FontAsset font;
    
    [SerializeField]
    private float maxFontSize = 36f;
    
    [SerializeField]
    private float minFontSize = 8f;

    [Header("Text Spacing")]
    [SerializeField]
    private float lineSpacing = 0f;
    
    [SerializeField]
    private float wordSpacing = 0f;
    
    [SerializeField]
    private float characterSpacing = 0f;

    // Properties with proper getters
    public string LanKey => lanKey;
    public string LanName => lanName;
    public SystemLanguage[] ApplySystemLanguages => applySystemLanguages;
    public TMP_FontAsset Font => font;
    public float MaxFontSize => maxFontSize;
    public float MinFontSize => minFontSize;
    public float LineSpacing => lineSpacing;
    public float WordSpacing => wordSpacing;
    public float CharacterSpacing => characterSpacing;

    // For backward compatibility (used by other scripts)
    public string languageKey => lanKey;
    public string languageName => lanName;

    public void InitTextMeshProUGUI(TextMeshProUGUI textComponent, bool updateFontSize = true)
    {
        if (textComponent == null)
            return;

        // Apply font if available
        if (font != null)
        {
            textComponent.font = font;
        }

        // Apply font size constraints if requested
        if (updateFontSize)
        {
            if (textComponent.fontSize > maxFontSize)
            {
                textComponent.fontSize = maxFontSize;
            }
            else if (textComponent.fontSize < minFontSize)
            {
                textComponent.fontSize = minFontSize;
            }
        }

        // Apply spacing settings
        textComponent.lineSpacing = lineSpacing;
        textComponent.wordSpacing = wordSpacing;
        textComponent.characterSpacing = characterSpacing;

        // Enable auto-sizing if min and max font sizes are different
        if (maxFontSize > minFontSize)
        {
            textComponent.enableAutoSizing = true;
            textComponent.fontSizeMin = minFontSize;
            textComponent.fontSizeMax = maxFontSize;
        }
        else
        {
            textComponent.enableAutoSizing = false;
        }
    }

    // Helper method to check if this setting applies to the current system language
    public bool AppliesTo(SystemLanguage systemLanguage)
    {
        if (applySystemLanguages == null || applySystemLanguages.Length == 0)
            return false;

        foreach (var lang in applySystemLanguages)
        {
            if (lang == systemLanguage)
                return true;
        }
        
        return false;
    }

    // Method to get appropriate font size within constraints
    public float GetClampedFontSize(float desiredSize)
    {
        return Mathf.Clamp(desiredSize, minFontSize, maxFontSize);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Ensure max font size is always greater than min font size
        if (maxFontSize < minFontSize)
        {
            maxFontSize = minFontSize;
        }
    }
#endif
}