using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement Data", menuName = "Achievements/Achievement Data")]
[System.Serializable]
public class AchievementData : ScriptableObject
{
    [SerializeField] protected string id;
    [SerializeField] protected string titleKey;
    [SerializeField] protected string descriptionKey;
    [SerializeField] private AchievementProgressReference outerReference;
    [SerializeField] protected AchievementProgress[] progresses;
    
    [Header("Google Play Games Settings")]
    [SerializeField] private bool syncWithGooglePlay = true;

    public AchievementProgress[] Progresses => progresses;
    public string Id => id;
    public bool SyncWithGooglePlay => syncWithGooglePlay;

    private void OnValidate()
    {
        // Ensure ID is not empty
        if (string.IsNullOrEmpty(id))
        {
            id = name.Replace(" ", "_").ToLower();
        }
    }

    public void Init()
    {
        if (progresses != null)
        {
            for (int i = 0; i < progresses.Length; i++)
            {
                if (progresses[i] != null)
                {
                    // Initialize outer reference if it's an indexed type
                    if (outerReference is AchievementProgressReferenceOuterKey outerKeyRef)
                    {
                        outerKeyRef.Init(i);
                    }
                    
                    progresses[i].Init(i, $"{id}_{i}", titleKey, descriptionKey, outerReference);
                }
                else
                {
                    Debug.LogWarning($"Achievement {id} has null progress at index {i}");
                }
            }
        }
        else
        {
            Debug.LogWarning($"Achievement {id} has no progress items defined");
        }
    }

    public void UpdateLocalization()
    {
        if (progresses != null)
        {
            foreach (var progress in progresses)
            {
                if (progress != null)
                {
                    progress.UpdateLocalization();
                }
            }
        }
    }

    public void LoadProgress()
    {
        if (progresses != null)
        {
            foreach (var progress in progresses)
            {
                if (progress != null)
                {
                    try
                    {
                        progress.LoadProgress();
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Failed to load progress for {progress.ProgressId}: {e.Message}");
                    }
                }
            }
        }
    }

    public void SaveProgress()
    {
        if (progresses != null)
        {
            foreach (var progress in progresses)
            {
                if (progress != null)
                {
                    try
                    {
                        progress.SaveProgress();
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Failed to save progress for {progress.ProgressId}: {e.Message}");
                    }
                }
            }
        }
    }

    public void AddProgress(float amount)
    {
        if (progresses != null)
        {
            foreach (var progress in progresses)
            {
                if (progress != null && !progress.Completed)
                {
                    try
                    {
                        progress.AddProgress(amount);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Failed to add progress to {progress.ProgressId}: {e.Message}");
                    }
                }
            }
        }
    }

    // Get the first incomplete progress item
    public AchievementProgress GetCurrentProgress()
    {
        if (progresses != null)
        {
            foreach (var progress in progresses)
            {
                if (progress != null && !progress.Completed)
                {
                    return progress;
                }
            }
        }
        return null;
    }

    // Check if all progress items are completed
    public bool IsFullyCompleted()
    {
        if (progresses == null || progresses.Length == 0)
            return false;

        foreach (var progress in progresses)
        {
            if (progress == null || !progress.Completed)
                return false;
        }
        return true;
    }

    // Get completion percentage (0-100)
    public float GetCompletionPercentage()
    {
        if (progresses == null || progresses.Length == 0)
            return 0f;

        int completedCount = 0;
        foreach (var progress in progresses)
        {
            if (progress != null && progress.Completed)
                completedCount++;
        }

        return (float)completedCount / progresses.Length * 100f;
    }

    // Reset all progress (useful for testing)
    [ContextMenu("Reset All Progress")]
    public void ResetAllProgress()
    {
        if (progresses != null)
        {
            foreach (var progress in progresses)
            {
                if (progress != null)
                {
                    progress.ResetAchievement();
                }
            }
        }
    }
}