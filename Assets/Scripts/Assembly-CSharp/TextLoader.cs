using System.Collections.Generic;
using UnityEngine;

public class TextLoader
{
    public static Dictionary<string, string> texts;

    public static string getText(string key)
    {
        if (texts == null)
        {
            loadText();
        }

        if (texts != null && texts.ContainsKey(key))
        {
            return texts[key];
        }

        // Return the key itself as fallback if no translation found
        return key;
    }

    public static bool TryGetText(string key, ref string text)
    {
        if (texts == null)
        {
            loadText();
        }

        if (texts != null && texts.ContainsKey(key))
        {
            text = texts[key];
            return true;
        }

        return false;
    }

    public static void loadText()
    {
        texts = new Dictionary<string, string>();

        // Load text file based on current language
        string languageKey = DataLoader.usingFileLan ?? DataLoader.DEFAULT_LANGUAGE_KEY;
        TextAsset textFile = LoadFile(languageKey);

        if (textFile != null)
        {
            ParseTextFile(textFile.text);
        }
        else
        {
            // Fallback to default language if current language file not found
            if (languageKey != DataLoader.DEFAULT_LANGUAGE_KEY)
            {
                TextAsset defaultFile = LoadFile(DataLoader.DEFAULT_LANGUAGE_KEY);
                if (defaultFile != null)
                {
                    ParseTextFile(defaultFile.text);
                }
            }
        }
    }

    public static TextAsset LoadFile(string prefix)
    {
        // Try to load text file from Resources folder
        string filePath = $"gameText_{prefix}";
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);

        if (textAsset == null)
        {
            Debug.LogWarning($"Could not load localization file at path: {filePath}");
        }
        return textAsset;
    }


    private static void ParseTextFile(string fileContent)
    {
        if (string.IsNullOrEmpty(fileContent))
            return;

        string[] lines = fileContent.Split('\n');

        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();

            // Skip empty lines and comments
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#") || trimmedLine.StartsWith("//"))
                continue;

            // Parse key=value format
            int separatorIndex = trimmedLine.IndexOf('=');
            if (separatorIndex > 0 && separatorIndex < trimmedLine.Length - 1)
            {
                string key = trimmedLine.Substring(0, separatorIndex).Trim();
                string value = trimmedLine.Substring(separatorIndex + 1).Trim();

                // Remove quotes if present
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                // Handle escape sequences
                value = value.Replace("\\n", "\n")
                            .Replace("\\t", "\t")
                            .Replace("\\\"", "\"")
                            .Replace("\\\\", "\\");

                if (!texts.ContainsKey(key))
                {
                    texts[key] = value;
                }
            }
        }
    }

    // Method to reload text when language changes
    public static void ReloadText()
    {
        texts = null;
        loadText();
    }
}