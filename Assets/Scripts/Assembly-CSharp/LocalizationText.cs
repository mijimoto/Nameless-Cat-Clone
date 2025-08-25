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
	}

	public void SetKey(string key)
	{
	}

	public void UpdateValue(string param)
	{
	}

	public void CleanText()
	{
	}
}
