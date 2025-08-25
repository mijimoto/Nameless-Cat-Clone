using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public const string DEFAULT_LANGUAGE_KEY = "en";

    public static string usingFileLan;

    public TextBoxSetting[] textBoxObjSettings;       // Localized text box styles
    public AchievementData[] achievementDatas;        // Achievements metadata
    public LanguagePanelController languagePanel;

    private static Dictionary<string, TextBoxSetting> textBoxSettings;
    private static Dictionary<string, AchievementData> achievementDataDictionary;

    public static IReadOnlyList<TextBoxSetting> TextBoxSettings => new List<TextBoxSetting>(textBoxSettings.Values);
    public static Dictionary<string, AchievementData> AchievementDataDictionary => achievementDataDictionary;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (string.IsNullOrEmpty(usingFileLan))
            usingFileLan = PlayerPrefs.GetString("Language", DEFAULT_LANGUAGE_KEY);

        textBoxSetup();
        achievementDataSetup();

        ChangeLangauge(usingFileLan);
    }

    private void OnDestroy()
    {
        // cleanup if needed
        textBoxSettings?.Clear();
        achievementDataDictionary?.Clear();
    }

    public string LanFile()
    {
        return usingFileLan;
    }

    private void ChangeLangauge(string targetLan)
    {
        usingFileLan = targetLan;
        PlayerPrefs.SetString("Language", targetLan);
        PlayerPrefs.Save();

        if (languagePanel != null)
            languagePanel.UpdateTexts(targetLan); // refresh UI text
    }

    public static void changeLan(string lan)
    {
        changeLan(lan, true);
    }

    public static void changeLan(string lan, bool updateText)
    {
        usingFileLan = lan;
        PlayerPrefs.SetString("Language", lan);
        PlayerPrefs.Save();

        if (updateText && _instance()?.languagePanel != null)
            _instance().languagePanel.UpdateTexts(lan);
    }

    private void textBoxSetup()
    {
        textBoxSettings = new Dictionary<string, TextBoxSetting>();
        foreach (var s in textBoxObjSettings)
        {
            if (!textBoxSettings.ContainsKey(s.id))
                textBoxSettings.Add(s.id, s);
        }
    }

    public static TextBoxSetting getCurrentTextBoxSetting()
    {
        if (textBoxSettings == null || string.IsNullOrEmpty(usingFileLan))
            return null;

        return textBoxSettings.ContainsKey(usingFileLan) ? textBoxSettings[usingFileLan] : null;
    }

    private void achievementDataSetup()
    {
        achievementDataDictionary = new Dictionary<string, AchievementData>();
        foreach (var a in achievementDatas)
        {
            if (!achievementDataDictionary.ContainsKey(a.id))
                achievementDataDictionary.Add(a.id, a);
        }
    }

    public static AchievementData getAchievementData(string id)
    {
        if (achievementDataDictionary == null) return null;
        return achievementDataDictionary.ContainsKey(id) ? achievementDataDictionary[id] : null;
    }

    public static void updateAchievemenLocaliztion()
    {
        // refresh achievement descriptions after language change
        foreach (var kv in achievementDataDictionary)
        {
            kv.Value.UpdateLocalization(usingFileLan);
        }
    }

    // Helper to access singleton instance (not originally shown but useful)
    private static DataLoader _instance()
    {
        return FindObjectOfType<DataLoader>();
    }
}
