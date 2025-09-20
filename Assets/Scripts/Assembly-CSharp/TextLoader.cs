// ===== TextLoader.cs =====
using System.Collections.Generic;
using UnityEngine;

public static class TextLoader
{
    private static Dictionary<string, string> localizedText = new Dictionary<string, string>();
    private static bool isLoaded = false;

    static TextLoader()
    {
        LoadLocalizedText();
    }

    public static void LoadLocalizedText()
    {
        localizedText.Clear();
        
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

        string dataAsString = textAsset.text;
        LoadFromString(dataAsString);
        isLoaded = true;
        
        Debug.Log($"Loaded {localizedText.Count} localized strings from {filePath}");
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
        return key; // Return the key itself as fallback
    }

    public static void ReloadText()
    {
        isLoaded = false;
        LoadLocalizedText();
    }
}