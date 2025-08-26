using UnityEngine;

[CreateAssetMenu(fileName = "New Skin Reference", menuName = "Achievements/Skin Reference")]
internal class AchievementProgressReferenceSkin : AchievementProgressReference
{
    [SerializeField]
    private string skinId;

    [SerializeField]
    private SkinProgressType progressType;

    private enum SkinProgressType
    {
        Unlocked,      // 1.0 if skin is unlocked, 0.0 if not
        TimesUsed,     // How many times the skin has been used
        UnlockTime     // Time since skin was unlocked (in days)
    }

    public override float LoadProgress()
    {
        if (string.IsNullOrEmpty(skinId))
            return 0f;

        switch (progressType)
        {
            case SkinProgressType.Unlocked:
                return PlayerPrefs.GetInt($"Skin_{skinId}_Unlocked", 0);

            case SkinProgressType.TimesUsed:
                return PlayerPrefs.GetFloat($"Skin_{skinId}_TimesUsed", 0f);

            case SkinProgressType.UnlockTime:
                string unlockDateString = PlayerPrefs.GetString($"Skin_{skinId}_UnlockDate", "");
                if (!string.IsNullOrEmpty(unlockDateString) && 
                    System.DateTime.TryParse(unlockDateString, out System.DateTime unlockDate))
                {
                    System.TimeSpan timeSinceUnlock = System.DateTime.Now - unlockDate;
                    return (float)timeSinceUnlock.TotalDays;
                }
                return 0f;

            default:
                return 0f;
        }
    }

    public void UnlockSkin()
    {
        if (string.IsNullOrEmpty(skinId))
            return;

        PlayerPrefs.SetInt($"Skin_{skinId}_Unlocked", 1);
        PlayerPrefs.SetString($"Skin_{skinId}_UnlockDate", System.DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    public void IncrementSkinUsage()
    {
        if (string.IsNullOrEmpty(skinId))
            return;

        float currentUsage = PlayerPrefs.GetFloat($"Skin_{skinId}_TimesUsed", 0f);
        PlayerPrefs.SetFloat($"Skin_{skinId}_TimesUsed", currentUsage + 1f);
        PlayerPrefs.Save();
    }

    public bool IsSkinUnlocked()
    {
        if (string.IsNullOrEmpty(skinId))
            return false;

        return PlayerPrefs.GetInt($"Skin_{skinId}_Unlocked", 0) == 1;
    }

    public float GetSkinUsageCount()
    {
        if (string.IsNullOrEmpty(skinId))
            return 0f;

        return PlayerPrefs.GetFloat($"Skin_{skinId}_TimesUsed", 0f);
    }
}