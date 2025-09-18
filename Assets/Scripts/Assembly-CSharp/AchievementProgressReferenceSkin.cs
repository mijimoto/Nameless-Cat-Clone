using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
internal class AchievementProgressReferenceSkin : AchievementProgressReference
{
    public override float LoadProgress()
    {
        // Example: return number of unlocked skins
        int unlockedSkins = PlayerPrefs.GetInt("UnlockedSkins", 0);
        return unlockedSkins;
    }
}