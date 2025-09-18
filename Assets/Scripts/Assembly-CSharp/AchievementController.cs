using UnityEngine;

public class AchievementController : MonoBehaviour
{
    [SerializeField] private Transform achievementHolder;
    [SerializeField] private AchievementItemController achievementItemPrefab;

    private void Start()
    {
        CreateProgressItem();
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
            return;

        // Clear existing items
        foreach (Transform child in achievementHolder)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        // Create items from DataLoader achievement data
        if (DataLoader.AchievementDatas != null)
        {
            foreach (var achievementData in DataLoader.AchievementDatas)
            {
                if (achievementData?.Progresses != null)
                {
                    foreach (var progress in achievementData.Progresses)
                    {
                        if (progress != null)
                        {
                            AchievementItemController item = Instantiate(achievementItemPrefab, achievementHolder);
                            item.Init(progress);
                        }
                    }
                }
            }
        }
    }
}
