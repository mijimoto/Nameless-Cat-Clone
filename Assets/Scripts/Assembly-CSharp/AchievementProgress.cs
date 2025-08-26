using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement Progress", menuName = "Achievements/Achievement Progress")]
public class AchievementProgress : ScriptableObject
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float targetProgress;

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
        // Get localized text using TextLoader
        title = TextLoader.getText(titleKey);
        description = TextLoader.getText(descriptionKey);
    }

    public virtual void LoadProgress()
    {
        // Load progress from PlayerPrefs
        string progressKey = $"Achievement_{id}_Progress";
        string completedKey = $"Achievement_{id}_Completed";
        string dateKey = $"Achievement_{id}_CompleteDate";

        progress = PlayerPrefs.GetFloat(progressKey, 0f);
        completed = PlayerPrefs.GetInt(completedKey, 0) == 1;

        if (completed)
        {
            string dateString = PlayerPrefs.GetString(dateKey, "");
            if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out DateTime savedDate))
            {
                completeDate = savedDate;
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
        // Save progress to PlayerPrefs
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
        string dateKey = $"Achievement_{id}_CompleteDate";
        PlayerPrefs.SetString(dateKey, completeDate.ToString());
    }

    public virtual void SetProgress(float amount)
    {
        if (completed)
            return;

        progress = Mathf.Clamp(amount, 0f, targetProgress);
        UpdateCompleted();
        SaveProgress();
    }

    public virtual void AddProgress(float amount)
    {
        if (completed)
            return;

        SetProgress(progress + amount);
    }

    protected virtual void UpdateCompleted()
    {
        if (!completed && progress >= targetProgress)
        {
            completed = true;
            completeDate = DateTime.Now;
            
            // Notify achievement system
            Debug.Log($"Achievement Completed: {title}");
            
            // You can add achievement notification/popup logic here
        }
    }
}