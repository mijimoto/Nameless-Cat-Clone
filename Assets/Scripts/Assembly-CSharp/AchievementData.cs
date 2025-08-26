using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement Data", menuName = "Achievements/Achievement Data")]
public class AchievementData : ScriptableObject
{
    [SerializeField]
    protected string id;

    [SerializeField]
    protected string titleKey;

    [SerializeField]
    protected string descriptionKey;

    [SerializeField]
    private AchievementProgressReference outerReference;

    [SerializeField]
    protected AchievementProgress[] progresses;

    public AchievementProgress[] Progresses => progresses;
    public string Id => id;

    public void Init()
    {
        if (progresses != null)
        {
            for (int i = 0; i < progresses.Length; i++)
            {
                if (progresses[i] != null)
                {
                    progresses[i].Init(i, $"{id}_{i}", titleKey, descriptionKey, outerReference);
                }
            }
        }

        // Initialize outer reference if it's an OuterKey type
        if (outerReference is AchievementProgressReferenceOuterKey outerKeyRef)
        {
            outerKeyRef.Init(0); // Initialize with index 0, can be modified as needed
        }
    }

    public void UpdateLocaliztion()
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
                    progress.LoadProgress();
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
                    progress.SaveProgress();
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
                    progress.AddProgress(amount);
                }
            }
        }
    }

    // Get the first uncompleted progress, or the last one if all completed
    public AchievementProgress GetCurrentProgress()
    {
        if (progresses == null || progresses.Length == 0)
            return null;

        // Find first uncompleted progress
        foreach (var progress in progresses)
        {
            if (progress != null && !progress.Completed)
            {
                return progress;
            }
        }

        // All completed, return the last one
        return progresses[progresses.Length - 1];
    }

    // Check if all progresses are completed
    public bool IsFullyCompleted()
    {
        if (progresses == null || progresses.Length == 0)
            return false;

        foreach (var progress in progresses)
        {
            if (progress != null && !progress.Completed)
            {
                return false;
            }
        }

        return true;
    }

    // Get completion percentage for the entire achievement
    public float GetOverallCompletionPercentage()
    {
        if (progresses == null || progresses.Length == 0)
            return 0f;

        float totalProgress = 0f;
        float totalTarget = 0f;

        foreach (var progress in progresses)
        {
            if (progress != null)
            {
                totalProgress += progress.Progress;
                totalTarget += progress.TargetProgress;
            }
        }

        return totalTarget > 0f ? totalProgress / totalTarget : 0f;
    }
}