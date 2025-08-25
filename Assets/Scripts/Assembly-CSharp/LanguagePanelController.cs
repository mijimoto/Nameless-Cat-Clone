using System;
using UnityEngine;

public class LanguagePanelController : MonoBehaviour
{
	[SerializeField]
	private LanguageButtonController buttonPrefab;

	[SerializeField]
	private Transform buttonContainer;

	public Action<string> OnButtonClick;

	private void Start()
	{
	}

	public void HidePanel()
	{
	}

	public void OnChangeLanauge(string targetLan)
	{
	}
}
