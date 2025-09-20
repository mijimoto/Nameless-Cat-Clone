// Reference for skin-related achievements
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin Reference", menuName = "Achievements/References/Skin Reference")]
[System.Serializable]
public class AchievementProgressReferenceSkin : AchievementProgressReference
{
    [SerializeField] private string skinCountKey = "UnlockedSkins";
    [SerializeField] private bool countPurchasedSkins = true;

    public override float LoadProgress()
    {
        if (countPurchasedSkins && Purchaser._instance != null)
        {
            int purchasedSkins = 0;
            // Count purchased skins from Purchaser
            for (int i = 1; i <= 20; i++) // Adjust range based on your skin count
            {
                if (Purchaser._instance.HasSkin(i))
                {
                    purchasedSkins++;
                }
            }
            return purchasedSkins;
        }
        else
        {
            // Fallback to PlayerPrefs
            return PlayerPrefs.GetInt(skinCountKey, 0);
        }
    }
}