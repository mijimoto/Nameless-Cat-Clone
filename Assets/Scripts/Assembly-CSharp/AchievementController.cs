using UnityEngine;
using System.Collections.Generic;

public class AchievementController : MonoBehaviour
{
    [SerializeField]
    private Transform achievementHolder;

    [SerializeField]
    private AchievementItemController achievementItemPrefab;

    [SerializeField]
    private AchievementData[] achievementDataArray;

    private List<AchievementItemController> achievementItems = new List<AchievementItemController>();
    private Dictionary<string, AchievementData> achievementDataDictionary;

    private void Start()
    {
        InitializeAchievements();
        CreateProgressItem();
        UpdateProgress();
    }

    private void InitializeAchievements()
    {
        achievementDataDictionary = new Dictionary<string, AchievementData>();
        
        // Initialize achievement data from DataLoader if available
        var dataLoaderAchievements = DataLoader.AchievementDataDictionary;
        if (dataLoaderAchievements != null)
        {
            foreach (var kvp in dataLoaderAchievements)
            {
                achievementDataDictionary[kvp.Key] = kvp.Value;
                kvp.Value.Init();
            }
        }

        // Initialize local achievement data array
        if (achievementDataArray != null)
        {
            foreach (var achievementData in achievementDataArray)
            {
                if (achievementData != null)
                {
                    achievementData.Init();
                    if (!achievementDataDictionary.ContainsKey(achievementData.Id))
                    {
                        achievementDataDictionary[achievementData.Id] = achievementData;
                    }
                }
            }
        }
    }

    public void UpdateProgress()
    {
        // Update all achievement items
        foreach (var item in achievementItems)
        {
            if (item != null)
            {
                item.UpdateProgress();
            }
        }

        // Update text settings for language changes
        foreach (var item in achievementItems)
        {
            if (item != null)
            {
                item.UpdateTextSetting();
            }
        }

        // Update localization for all achievement data
        foreach (var achievementData in achievementDataDictionary.Values)
        {
            if (achievementData != null)
            {
                achievementData.UpdateLocaliztion();
            }
        }
    }

    private void CreateProgressItem()
    {
        if (achievementHolder == null || achievementItemPrefab == null)
        {
            Debug.LogError("Achievement holder or prefab is not assigned!");
            return;
        }

        // Clear existing items
        ClearExistingItems();

        // Create items for each achievement data
        foreach (var achievementData in achievementDataDictionary.Values)
        {
            if (achievementData != null && achievementData.Progresses != null)
            {
                foreach (var progress in achievementData.Progresses)
                {
                    if (progress != null)
                    {
                        CreateAchievementItem(progress);
                    }
                }
            }
        }
    }

    private void ClearExistingItems()
    {
        foreach (var item in achievementItems)
        {
            if (item != null)
            {
                if (Application.isPlaying)
                    Destroy(item.gameObject);
                else
                    DestroyImmediate(item.gameObject);
            }
        }
        achievementItems.Clear();
    }

    private void CreateAchievementItem(AchievementProgress progress)
    {
        var itemInstance = Instantiate(achievementItemPrefab, achievementHolder);
        itemInstance.Init(progress);
        achievementItems.Add(itemInstance);
    }

    // Public methods for external systems to interact with achievements

    public void AddProgressToAchievement(string achievementId, float amount)
    {
        if (achievementDataDictionary.ContainsKey(achievementId))
        {
            achievementDataDictionary[achievementId].AddProgress(amount);
            UpdateProgress();
        }
    }

    public bool IsAchievementCompleted(string achievementId)
    {
        if (achievementDataDictionary.ContainsKey(achievementId))
        {
            return achievementDataDictionary[achievementId].IsFullyCompleted();
        }
        return false;
    }

    public float GetAchievementProgress(string achievementId)
    {
        if (achievementDataDictionary.ContainsKey(achievementId))
        {
            return achievementDataDictionary[achievementId].GetOverallCompletionPercentage();
        }
        return 0f;
    }

    // Method to save all achievement progress
    public void SaveAllProgress()
    {
        foreach (var achievementData in achievementDataDictionary.Values)
        {
            if (achievementData != null)
            {
                achievementData.SaveProgress();
            }
        }
    }

    // Method to load all achievement progress
    public void LoadAllProgress()
    {
        foreach (var achievementData in achievementDataDictionary.Values)
        {
            if (achievementData != null)
            {
                achievementData.LoadProgress();
            }
        }
        UpdateProgress();
    }

    // Method called when language changes
    public void OnLanguageChanged()
    {
        UpdateProgress();
    }

    private void OnDestroy()
    {
        // Save progress when controller is destroyed
        SaveAllProgress();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAllProgress();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveAllProgress();
        }
    }
}