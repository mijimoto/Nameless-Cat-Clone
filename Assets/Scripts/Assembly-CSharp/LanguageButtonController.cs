using System;
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
	}

	private void ChangeLanguage()
	{
	}

	private void Awake()
	{
	}

	private void OnDestory()
	{
	}
}
