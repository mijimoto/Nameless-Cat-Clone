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
        SetupLanguageButtons();
    }

    private void SetupLanguageButtons()
    {
        if (buttonPrefab == null || buttonContainer == null)
        {
            Debug.LogError("Button prefab or container is not assigned!");
            return;
        }

        // Clear existing buttons
        foreach (Transform child in buttonContainer)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        // Get available language settings
        var textBoxSettings = DataLoader.TextBoxSettings;
        if (textBoxSettings != null)
        {
            foreach (var setting in textBoxSettings)
            {
                if (setting != null)
                {
                    CreateLanguageButton(setting);
                }
            }
        }
    }

    private void CreateLanguageButton(TextBoxSetting textBoxSetting)
    {
        var buttonInstance = Instantiate(buttonPrefab, buttonContainer);
        buttonInstance.Init(textBoxSetting, OnChangeLanauge);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void OnChangeLanauge(string targetLan)
    {
        OnButtonClick?.Invoke(targetLan);
        HidePanel();
    }
}