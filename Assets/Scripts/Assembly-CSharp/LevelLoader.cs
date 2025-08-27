using System.Collections.Generic;
using UnityEngine;

public class LevelLoader
{
    private string[] story;

    public List<int[]> getLevelCondition(int chaper)
    {
        List<int[]> conditions = new List<int[]>();
        
        TextAsset conditionFile = Resources.Load<TextAsset>($"Levels/chapter{chaper}_conditions");
        if (conditionFile != null)
        {
            string[] lines = conditionFile.text.Split('\n');
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    string[] parts = trimmedLine.Split(',');
                    int[] condition = new int[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (int.TryParse(parts[i].Trim(), out int value))
                        {
                            condition[i] = value;
                        }
                    }
                    conditions.Add(condition);
                }
            }
        }
        
        return conditions;
    }

    public List<string> getLevelStory(string levelString)
    {
        List<string> storyLines = new List<string>();
        
        TextAsset storyFile = Resources.Load<TextAsset>($"story_{DataLoader.usingFileLan}");
        if (storyFile == null)
        {
            storyFile = Resources.Load<TextAsset>($"story_{DataLoader.DEFAULT_LANGUAGE_KEY}");
        }
        
        if (storyFile != null)
        {
            string[] lines = storyFile.text.Split('\n');
            bool foundLevel = false;
            
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                
                if (line.EndsWith(":") && line.StartsWith(levelString))
                {
                    foundLevel = true;
                    continue;
                }
                
                if (foundLevel)
                {
                    if (line == ";" || (line.EndsWith(":") && line != levelString + ":"))
                    {
                        break;
                    }
                    
                    if (!string.IsNullOrEmpty(line))
                    {
                        storyLines.Add(line);
                    }
                }
            }
        }
        
        return storyLines;
    }
}