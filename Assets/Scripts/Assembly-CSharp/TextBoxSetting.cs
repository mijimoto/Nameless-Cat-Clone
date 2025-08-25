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

	public string LanKey => null;

	public string LanName => null;

	public SystemLanguage[] ApplySystemLanguages => null;

	public TMP_FontAsset Font => null;

	public float MaxFontSize => 0f;

	public float MinFontSize => 0f;

	public float LineSpacing => 0f;

	public float WordSpacing => 0f;

	public float CharacterSpacing => 0f;

	public void InitTextMeshProUGUI(TextMeshProUGUI textComponent, bool updateFontSize = true)
	{
	}
}
