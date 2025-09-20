using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class TextLoader
{
    private static Dictionary<string, string> localizedText = new Dictionary<string, string>();
    private static bool isLoaded = false;

    private static string SavePath => Path.Combine(Application.persistentDataPath, $"Localization_{DataLoader.usingFileLan}.txt");

    static TextLoader()
    {
        LoadLocalizedText();
    }

    public static void LoadLocalizedText()
    {
        localizedText.Clear();

        // 1. Try to load from persistent data (saved file)
        if (File.Exists(SavePath))
        {
            string dataAsString = File.ReadAllText(SavePath);
            LoadFromString(dataAsString);
            isLoaded = true;
            Debug.Log($"Loaded {localizedText.Count} strings from saved file: {SavePath}");
            return;
        }

        // 2. Fallback: load from Resources
        string filePath = $"Localization/{DataLoader.usingFileLan}";
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);

        if (textAsset == null)
        {
            // Try default language
            filePath = $"Localization/{DataLoader.DEFAULT_LANGUAGE_KEY}";
            textAsset = Resources.Load<TextAsset>(filePath);

            if (textAsset == null)
            {
                Debug.LogError($"Could not load localization file at: {filePath}");
                return;
            }
        }

        string dataFromResources = textAsset.text;
        LoadFromString(dataFromResources);
        isLoaded = true;

        Debug.Log($"Loaded {localizedText.Count} localized strings from Resources/{filePath}");
    }

    private static void LoadFromString(string dataAsString)
    {
        string[] lines = dataAsString.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                continue;

            int colonIndex = line.IndexOf(':');
            if (colonIndex > 0)
            {
                string key = line.Substring(0, colonIndex).Trim();
                string value = line.Substring(colonIndex + 1).Trim();

                // Handle escape sequences
                value = value.Replace("\\n", "\n");

                localizedText[key] = value;
            }
        }
    }

    public static string getText(string key)
    {
        if (!isLoaded)
        {
            LoadLocalizedText();
        }

        if (string.IsNullOrEmpty(key))
            return "";

        if (localizedText.ContainsKey(key))
        {
            return localizedText[key];
        }

        Debug.LogWarning($"Localization key not found: {key}");
        return key; // fallback
    }

    public static void ReloadText()
    {
        isLoaded = false;
        LoadLocalizedText();
    }

    // 🔹 Save current dictionary to a file
    public static void SaveText()
    {
        List<string> lines = new List<string>();

        foreach (var kvp in localizedText)
        {
            string value = kvp.Value.Replace("\n", "\\n"); // escape newlines
            lines.Add($"{kvp.Key}:{value}");
        }

        File.WriteAllLines(SavePath, lines.ToArray());

        Debug.Log($"Localization saved to {SavePath}");
    }

    // 🔹 Update or add a key:value and auto-save
    public static void SetText(string key, string value, bool saveNow = true)
    {
        if (string.IsNullOrEmpty(key)) return;

        localizedText[key] = value;

        if (saveNow)
        {
            SaveText();
        }
    }
}
