using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class AchievementData : ScriptableObject
{
    [SerializeField] protected string id;
    [SerializeField] protected string titleKey;
    [SerializeField] protected string descriptionKey;
    [SerializeField] private AchievementProgressReference outerReference;
    [SerializeField] protected AchievementProgress[] progresses;

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
}
