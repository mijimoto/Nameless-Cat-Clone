using TMPro;
using UnityEngine;

[CreateAssetMenu]
[SerializeField]
public class TextBoxSetting : ScriptableObject
{
    [SerializeField]
    private string lanKey;

    [SerializeField]
    private string lanName;

    [SerializeField]
    private SystemLanguage[] applySystemLanguages;

    [SerializeField]
    private TMP_FontAsset font;

    [SerializeField]
    private float maxFontSize;

    [SerializeField]
    private float minFontSize;

    [SerializeField]
    private float lineSpacing;

    [SerializeField]
    private float wordSpacing;

    [SerializeField]
    private float characterSpacing;

    public string LanKey => lanKey;

    public string LanName => lanName;

    public SystemLanguage[] ApplySystemLanguages => applySystemLanguages;

    public TMP_FontAsset Font => font;

    public float MaxFontSize => maxFontSize;

    public float MinFontSize => minFontSize;

    public float LineSpacing => lineSpacing;

    public float WordSpacing => wordSpacing;

    public float CharacterSpacing => characterSpacing;

    public void InitTextMeshProUGUI(TextMeshProUGUI textComponent, bool updateFontSize = true)
    {
        if (textComponent == null || font == null)
            return;

        textComponent.font = font;
        textComponent.lineSpacing = lineSpacing;
        textComponent.wordSpacing = wordSpacing;
        textComponent.characterSpacing = characterSpacing;

        if (updateFontSize)
        {
            textComponent.fontSizeMax = maxFontSize;
            textComponent.fontSizeMin = minFontSize;
            textComponent.enableAutoSizing = true;
        }
    }
}