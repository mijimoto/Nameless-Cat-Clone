using UnityEngine;

[CreateAssetMenu(fileName = "New Chapter Progress", menuName = "Achievements/Chapter Progress")]
public class AchievementChapterProgress : AchievementProgress
{
    [SerializeField]
    private int chapterNumber;

    [SerializeField]
    private string chapterProgressKey;

    public override void LoadProgress()
    {
        // Load chapter-specific progress
        if (!string.IsNullOrEmpty(chapterProgressKey))
        {
            float chapterProgress = PlayerPrefs.GetFloat(chapterProgressKey, 0f);
            progress = chapterProgress;
        }

        // Call base implementation for general progress loading
        base.LoadProgress();
    }

    public override void SaveProgress()
    {
        // Save chapter-specific progress
        if (!string.IsNullOrEmpty(chapterProgressKey))
        {
            PlayerPrefs.SetFloat(chapterProgressKey, progress);
        }

        // Call base implementation for general progress saving
        base.SaveProgress();
    }

    protected override void UpdateCompleted()
    {
        // Custom completion logic for chapters
        if (!completed && progress >= TargetProgress)
        {
            completed = true;
            completeDate = System.DateTime.Now;
            
            Debug.Log($"Chapter {chapterNumber} Achievement Completed: {title}");
            
            // Unlock next chapter or provide chapter-specific rewards
            UnlockChapterRewards();
        }
    }

    private void UnlockChapterRewards()
    {
        // Chapter-specific reward logic
        string rewardKey = $"Chapter_{chapterNumber}_Reward_Unlocked";
        PlayerPrefs.SetInt(rewardKey, 1);
        
        // You can add more chapter-specific unlock logic here
        // For example: unlock skins, levels, or story content
    }

    public bool IsChapterUnlocked()
    {
        string unlockKey = $"Chapter_{chapterNumber}_Unlocked";
        return PlayerPrefs.GetInt(unlockKey, chapterNumber == 1 ? 1 : 0) == 1;
    }

    public void UnlockChapter()
    {
        string unlockKey = $"Chapter_{chapterNumber}_Unlocked";
        PlayerPrefs.SetInt(unlockKey, 1);
        PlayerPrefs.Save();
    }
}