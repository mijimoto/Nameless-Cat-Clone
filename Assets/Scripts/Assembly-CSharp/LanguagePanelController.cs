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
        if (DataLoader.TextBoxSettings != null && buttonPrefab != null && buttonContainer != null)
        {
            foreach (var textBoxSetting in DataLoader.TextBoxSettings)
            {
                if (textBoxSetting != null)
                {
                    LanguageButtonController buttonInstance = Instantiate(buttonPrefab, buttonContainer);
                    buttonInstance.Init(textBoxSetting, OnChangeLanauge);
                }
            }
        }
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public void OnChangeLanauge(string targetLan)
    {
        OnButtonClick?.Invoke(targetLan);
    }
}