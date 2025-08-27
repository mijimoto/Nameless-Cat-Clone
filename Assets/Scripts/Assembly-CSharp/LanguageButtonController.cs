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
        if (textBoxSetting == null) return;
        
        targetLanKey = textBoxSetting.LanKey;
        OnChangeLanauge = changeLanguageAction;
        
        if (languageText != null)
        {
            languageText.text = textBoxSetting.LanName;
        }
        
        if (languageButton != null)
        {
            languageButton.onClick.RemoveListener(ChangeLanguage);
            languageButton.onClick.AddListener(ChangeLanguage);
        }
    }

    private void ChangeLanguage()
    {
        OnChangeLanauge?.Invoke(targetLanKey);
    }

    private void Awake()
    {
        if (languageButton != null)
        {
            languageButton.onClick.AddListener(ChangeLanguage);
        }
    }

    private void OnDestory()
    {
        if (languageButton != null)
        {
            languageButton.onClick.RemoveListener(ChangeLanguage);
        }
    }
}