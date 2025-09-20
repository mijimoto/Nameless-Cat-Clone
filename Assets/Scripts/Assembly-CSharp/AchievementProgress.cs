using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement Progress", menuName = "Achievements/Achievement Progress")]
[System.Serializable]
public class AchievementProgress : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private float targetProgress;
    [SerializeField] private string googlePlayAchievementId; // New field for Google Play ID
    [SerializeField] private bool isIncremental = false; // New field to indicate if it's incremental
    
    protected float progress;
    protected DateTime completeDate;
    protected bool completed;
    protected string title;
    protected string description;
    protected int index;
    protected string id;
    protected string titleKey;
    protected string descriptionKey;
    protected AchievementProgressReference outerReference;

    public string ProgressId => id;
    public Sprite Icon => icon;
    public string Title => title;
    public string Description => description;
    public float Progress => progress;
    public float TargetProgress => targetProgress;
    public DateTime CompleteDate => completeDate;
    public bool Completed => completed;
    public string GooglePlayId => googlePlayAchievementId;
    public bool IsIncremental => isIncremental;

    public virtual void Init(int index, string id, string titleKey, string descriptionKey, AchievementProgressReference outerReference)
    {
        this.index = index;
        this.id = id;
        this.titleKey = titleKey;
        this.descriptionKey = descriptionKey;
        this.outerReference = outerReference;
        
        UpdateLocalization();
        LoadProgress();
    }

    public void UpdateLocalization()
    {
        if (!string.IsNullOrEmpty(titleKey))
        {
            // Use your TextLoader or fallback to key if TextLoader is not available
            title = TextLoader.getText(titleKey) ?? titleKey;
        }
        
        if (!string.IsNullOrEmpty(descriptionKey))
        {
            description = TextLoader.getText(descriptionKey) ?? descriptionKey;
        }
        
        // If title/description are still empty, use default values
        if (string.IsNullOrEmpty(title))
        {
            title = $"Achievement {index + 1}";
        }
        
        if (string.IsNullOrEmpty(description))
        {
            description = $"Complete this achievement by reaching {targetProgress} progress.";
        }
    }

    public virtual void LoadProgress()
    {
        string progressKey = $"Achievement_{id}_Progress";
        string completedKey = $"Achievement_{id}_Completed";
        string dateKey = $"Achievement_{id}_Date";

        progress = PlayerPrefs.GetFloat(progressKey, 0f);
        completed = PlayerPrefs.GetInt(completedKey, 0) == 1;

        if (completed)
        {
            string dateString = PlayerPrefs.GetString(dateKey, "");
            if (!string.IsNullOrEmpty(dateString))
            {
                if (DateTime.TryParse(dateString, out DateTime parsedDate))
                {
                    completeDate = parsedDate;
                }
                else
                {
                    completeDate = DateTime.Now;
                }
            }
            else
            {
                completeDate = DateTime.Now;
            }
        }

        // Load from outer reference if available
        if (outerReference != null)
        {
            float outerProgress = outerReference.LoadProgress();
            if (outerProgress > progress)
            {
                SetProgress(outerProgress);
            }
        }
    }

    public virtual void SaveProgress()
    {
        string progressKey = $"Achievement_{id}_Progress";
        string completedKey = $"Achievement_{id}_Completed";

        PlayerPrefs.SetFloat(progressKey, progress);
        PlayerPrefs.SetInt(completedKey, completed ? 1 : 0);

        if (completed)
        {
            saveDate();
        }

        PlayerPrefs.Save();
    }

    protected void saveDate()
    {
        string dateKey = $"Achievement_{id}_Date";
        PlayerPrefs.SetString(dateKey, completeDate.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    public virtual void SetProgress(float amount)
    {
        float oldProgress = progress;
        progress = Mathf.Clamp(amount, 0f, targetProgress);
        
        bool wasCompleted = completed;
        UpdateCompleted();
        
        // Report to Google Play Games if progress changed
        if (Math.Abs(oldProgress - progress) > 0.001f)
        {
            ReportToGooglePlay();
        }
        
        // If just completed, trigger completion effects
        if (!wasCompleted && completed)
        {
            OnAchievementCompleted();
        }
        
        SaveProgress();
    }

    public virtual void AddProgress(float amount)
    {
        SetProgress(progress + amount);
    }

    protected virtual void UpdateCompleted()
    {
        if (!completed && progress >= targetProgress)
        {
            completed = true;
            completeDate = DateTime.Now;
        }
    }

    protected virtual void OnAchievementCompleted()
    {
        Debug.Log($"Achievement completed: {title}");
        
        // You can add additional completion effects here:
        // - Show notification UI
        // - Play sound effect
        // - Award rewards
        // - etc.
    }

    private void ReportToGooglePlay()
    {
        if (string.IsNullOrEmpty(googlePlayAchievementId))
            return;

        if (AchievementController.Instance == null)
            return;

        if (!Social.localUser.authenticated)
            return;

        if (completed)
        {
            // Unlock the achievement (report 100% progress)
            AchievementController.Instance.UnlockGooglePlayAchievement(googlePlayAchievementId);
        }
        else if (isIncremental && progress > 0)
        {
            // Report incremental progress as percentage (0-100)
            double progressPercentage = (progress / targetProgress) * 100.0;
            AchievementController.Instance.ReportProgressToGooglePlay(googlePlayAchievementId, progressPercentage);
        }
    }

    // Helper method to get progress percentage
    public float GetProgressPercentage()
    {
        if (targetProgress <= 0) return 0f;
        return Mathf.Clamp01(progress / targetProgress) * 100f;
    }

    // Helper method to reset achievement (useful for testing)
    [ContextMenu("Reset Achievement")]
    public void ResetAchievement()
    {
        progress = 0f;
        completed = false;
        completeDate = DateTime.MinValue;
        SaveProgress();
    }

    // Method to manually complete achievement (useful for testing)
    [ContextMenu("Complete Achievement")]
    public void CompleteAchievement()
    {
        SetProgress(targetProgress);
    }
}