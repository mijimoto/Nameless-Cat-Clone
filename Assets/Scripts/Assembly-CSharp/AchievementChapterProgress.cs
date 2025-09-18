using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class AchievementChapterProgress : AchievementProgress
{
    public override void LoadProgress()
    {
        base.LoadProgress();
        
        // Additional chapter-specific loading logic
        // For example, check if specific chapters are completed
        string chapterKey = $"Chapter_{index}_Completed";
        if (PlayerPrefs.GetInt(chapterKey, 0) == 1)
        {
            SetProgress(TargetProgress);
        }
    }

    public override void SaveProgress()
    {
        base.SaveProgress();
        
        // Save chapter completion status
        if (completed)
        {
            string chapterKey = $"Chapter_{index}_Completed";
            PlayerPrefs.SetInt(chapterKey, 1);
        }
    }

    protected override void UpdateCompleted()
    {
        base.UpdateCompleted();
        
        // Additional chapter completion logic
        if (completed)
        {
            Debug.Log($"Chapter {index} completed!");
        }
    }
}