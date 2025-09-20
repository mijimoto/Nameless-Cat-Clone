using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public const string DEFAULT_LANGUAGE_KEY = "en";

    public static string usingFileLan;

    public TextBoxSetting[] textBoxObjSettings;
    public AchievementData[] achievementDatas;
    public LanguagePanelController languagePanel;

    private static Dictionary<string, TextBoxSetting> textBoxSettings;
    private static Dictionary<string, AchievementData> achievementDataDictionary;

    public static IReadOnlyList<TextBoxSetting> TextBoxSettings => instance?.textBoxObjSettings;
    public static Dictionary<string, AchievementData> AchievementDataDictionary => achievementDataDictionary;
    public static IReadOnlyList<AchievementData> AchievementDatas => instance?.achievementDatas;

    private static DataLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize language from PlayerPrefs or use default
            usingFileLan = PlayerPrefs.GetString("SelectedLanguage", DEFAULT_LANGUAGE_KEY);

            // Setup data structures
            textBoxSetup();
            achievementDataSetup();

            // Setup language panel callback
            if (languagePanel != null)
            {
                languagePanel.OnButtonClick = ChangeLangauge;
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public string LanFile()
    {
        return $"Localization/{usingFileLan}";
    }

    private void ChangeLangauge(string targetLan)
    {
        if (targetLan != usingFileLan)
        {
            changeLan(targetLan, true);
        }
    }

    public static void changeLan(string lan)
    {
        changeLan(lan, false);
    }

    public static void changeLan(string lan, bool updateText)
    {
        usingFileLan = lan;
        PlayerPrefs.SetString("SelectedLanguage", lan);
        PlayerPrefs.Save();

        if (updateText)
        {
            // Reload text data for new language
            TextLoader.ReloadText();

            // Update all localized text components
            LocalizationText[] allTexts = FindObjectsOfType<LocalizationText>();
            foreach (var text in allTexts)
            {
                text.RefreshLocalization();
            }

            // Update all text boxes
            TextBox[] allTextBoxes = FindObjectsOfType<TextBox>();
            TextBoxSetting currentSetting = getCurrentTextBoxSetting();

            foreach (var textBox in allTextBoxes)
            {
                if (textBox == null) continue;

                // Try to find the TextMeshProUGUI component beneath the TextBox GameObject
                var tmp = textBox.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
                if (tmp != null)
                {
                    // Apply font/spacing/etc for the current language
                    currentSetting?.InitTextMeshProUGUI(tmp, true);
                }
                else
                {
                    // Fallback: call the public Init(TextBoxSetting) on TextBox if it exists and you want
                    // the TextBox to handle other initialization details (this does not require new functions).
                    try
                    {
                        textBox.Init(currentSetting);
                    }
                    catch
                    {
                        // If Init is not appropriate, just skip silently to avoid crashes.
                        Debug.LogWarning("[DataLoader] TextBox has no TMP child and Init failed. Skipping.");
                    }
                }
            }

            // Update achievement localizations
            updateAchievementLocalization();
        }
    }

    private void textBoxSetup()
    {
        textBoxSettings = new Dictionary<string, TextBoxSetting>();

        if (textBoxObjSettings != null)
        {
            foreach (var setting in textBoxObjSettings)
            {
                if (setting != null && !string.IsNullOrEmpty(setting.LanKey))
                {
                    textBoxSettings[setting.LanKey] = setting;
                }
            }
        }
    }

    public static TextBoxSetting getCurrentTextBoxSetting()
    {
        if (textBoxSettings != null && textBoxSettings.ContainsKey(usingFileLan))
        {
            return textBoxSettings[usingFileLan];
        }

        // Fallback to default language
        if (textBoxSettings != null && textBoxSettings.ContainsKey(DEFAULT_LANGUAGE_KEY))
        {
            return textBoxSettings[DEFAULT_LANGUAGE_KEY];
        }

        return null;
    }

    private void achievementDataSetup()
    {
        achievementDataDictionary = new Dictionary<string, AchievementData>();

        if (achievementDatas != null)
        {
            foreach (var achievement in achievementDatas)
            {
                if (achievement != null && !string.IsNullOrEmpty(achievement.Id))
                {
                    achievementDataDictionary[achievement.Id] = achievement;
                    achievement.Init();
                }
            }
        }
    }

    public static AchievementData getAchievementData(string id)
    {
        if (achievementDataDictionary != null && achievementDataDictionary.ContainsKey(id))
        {
            return achievementDataDictionary[id];
        }
        return null;
    }

    public static void updateAchievementLocalization()
    {
        if (achievementDataDictionary != null)
        {
            foreach (var achievement in achievementDataDictionary.Values)
            {
                achievement.UpdateLocalization();
            }
        }

        // Update all achievement item controllers in scene
        AchievementItemController[] achievementItems = FindObjectsOfType<AchievementItemController>();
        foreach (var item in achievementItems)
        {
            item.UpdateTextSetting();
        }
    }
}