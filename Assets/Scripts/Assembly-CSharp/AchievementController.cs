using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections.Generic;

public class AchievementController : MonoBehaviour
{
    [SerializeField] private Transform achievementHolder;
    [SerializeField] private AchievementItemController achievementItemPrefab;
    
    public static AchievementController Instance { get; private set; }

  private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        if (transform.root.gameObject.name != "CollectionMenu") // Only persist if not in collection menu
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    else if (Instance != this)
    {
        Destroy(gameObject);
    }
}

private void Start()
{
    // Only initialize Google Play if not in editor or if explicitly enabled
    #if !UNITY_EDITOR || FORCE_GOOGLE_PLAY_IN_EDITOR
    InitializeGooglePlayGames();
    #endif
    
    // Always create progress items
    StartCoroutine(WaitAndCreateItems());
}

private System.Collections.IEnumerator WaitAndCreateItems()
{
    // Wait for DataLoader to be ready
    while (DataLoader.AchievementDatas == null || DataLoader.AchievementDatas.Count == 0)
    {
        yield return new WaitForSeconds(0.1f);
    }
    
    CreateProgressItem();
}

private void DelayedInit()
{
    InitializeGooglePlayGames();
    CreateProgressItem();
}

    private void InitializeGooglePlayGames()
    {
        // Activate Google Play Games Platform
        PlayGamesPlatform.Activate();

        // Sign in to Google Play Games
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("Google Play Games authentication successful!");
            // Load cloud achievements if needed
            LoadCloudAchievements();
        }
        else
        {
            Debug.LogWarning("Google Play Games authentication failed: " + status);
        }
    }

    private void LoadCloudAchievements()
    {
        // Load achievements using Unity's Social API
        Social.LoadAchievements(achievements =>
        {
            if (achievements != null)
            {
                foreach (var achievement in achievements)
                {
                    Debug.Log($"Cloud achievement: {achievement.id}, completed: {achievement.completed}");
                    // Sync with local data if needed
                }
            }
        });
    }

    public void UpdateProgress()
    {
        AchievementItemController[] items = achievementHolder.GetComponentsInChildren<AchievementItemController>();
        foreach (var item in items)
        {
            item.UpdateProgress();
        }
    }

   private void CreateProgressItem()
{
    if (achievementItemPrefab == null || achievementHolder == null)
    {
        Debug.LogError("Achievement prefab or holder is null!");
        return;
    }

    // Clear existing items
    foreach (Transform child in achievementHolder)
    {
        if (Application.isPlaying)
            Destroy(child.gameObject);
    }

    // Create items from DataLoader achievement data
    if (DataLoader.AchievementDatas != null && DataLoader.AchievementDatas.Count > 0)
    {
        foreach (var achievementData in DataLoader.AchievementDatas)
        {
            if (achievementData == null) continue;
            
            // Create one item per achievement (not per progress)
            GameObject itemObj = Instantiate(achievementItemPrefab.gameObject, achievementHolder);
            itemObj.name = $"Achievement_{achievementData.Id}";
            
            
            AchievementItemController itemController = itemObj.GetComponent<AchievementItemController>();
            if (itemController != null)
            {
                // Pass the current progress or first progress
                var currentProgress = achievementData.GetCurrentProgress() ?? 
                                    (achievementData.Progresses?.Length > 0 ? achievementData.Progresses[0] : null);
                
                if (currentProgress != null)
                {
                    itemController.Init(currentProgress);
                }
            }
        }
    }
}

    // Method to unlock Google Play Games achievements
    public void UnlockGooglePlayAchievement(string achievementId)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(achievementId, 100.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log($"Achievement {achievementId} unlocked successfully!");
                }
                else
                {
                    Debug.LogError($"Failed to unlock achievement {achievementId}");
                }
            });
        }
        else
        {
            Debug.LogWarning("Cannot unlock achievement - not authenticated with Google Play Games");
        }
    }

    // Method to report incremental progress for Google Play Games
    public void ReportProgressToGooglePlay(string achievementId, double progress)
    {
        if (Social.localUser.authenticated)
        {
            // Use PlayGamesPlatform.Instance for more precise progress reporting
            PlayGamesPlatform.Instance.ReportProgress(achievementId, progress, (bool success) =>
            {
                if (success)
                {
                    Debug.Log($"Progress {progress} reported for achievement {achievementId}");
                }
                else
                {
                    Debug.LogError($"Failed to report progress for achievement {achievementId}");
                }
            });
        }
    }

    // Show Google Play Games achievements UI
    public void ShowAchievementsUI()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            Debug.LogWarning("Cannot show achievements UI - not authenticated");
        }
    }

    // Method to trigger specific achievements based on game events
    public void TriggerAchievement(AchievementType type, float amount = 1f)
    {
        // Update local achievement progress
        if (DataLoader.AchievementDatas != null)
        {
            foreach (var achievementData in DataLoader.AchievementDatas)
            {
                if (IsAchievementOfType(achievementData, type))
                {
                    achievementData.AddProgress(amount);
                    
                    // Check if any progress item is completed and unlock in Google Play
                    foreach (var progress in achievementData.Progresses)
                    {
                        if (progress.Completed)
                        {
                            string googlePlayId = GetGooglePlayId(progress.ProgressId);
                            if (!string.IsNullOrEmpty(googlePlayId))
                            {
                                UnlockGooglePlayAchievement(googlePlayId);
                            }
                        }
                    }
                }
            }
        }
        
        UpdateProgress();
    }

    private bool IsAchievementOfType(AchievementData achievementData, AchievementType type)
    {
        // Map your achievement data IDs to types
        switch (type)
        {
            case AchievementType.Fish:
                return achievementData.Id.Contains("fish");
            case AchievementType.Chapter:
                return achievementData.Id.Contains("chapter") || achievementData.Id.Contains("moon") || 
                       achievementData.Id.Contains("forest") || achievementData.Id.Contains("cathedral");
            default:
                return false;
        }
    }

    private string GetGooglePlayId(string localProgressId)
    {
        // Map your local achievement IDs to Google Play achievement IDs
        switch (localProgressId.ToLower())
        {
            case var id when id.Contains("moon"):
                return GPGSIds.achievement_moon;
            case var id when id.Contains("who_am_i"):
                return GPGSIds.achievement_who_am_i;
            case var id when id.Contains("i_love_fish"):
                return GPGSIds.achievement_i_love_fish;
            case var id when id.Contains("fish_hunter"):
                return GPGSIds.achievement_i_am_fish_hunter;
            case var id when id.Contains("more_fish"):
                return GPGSIds.achievement_i_want_more_fish;
            case var id when id.Contains("forest"):
                return GPGSIds.achievement_forest;
            case var id when id.Contains("cathedral"):
                return GPGSIds.achievement_cathedral;
            default:
                return null;
        }
    }
}

public enum AchievementType
{
    Fish,
    Chapter,
    Skin,
    General
}