using UnityEngine;

[CreateAssetMenu(fileName = "New Outer Key Reference", menuName = "Achievements/References/Outer Key Reference")]
[System.Serializable]
public class AchievementProgressReferenceOuterKey : AchievementProgressReference
{
    [SerializeField] private string outerReferenceKey;
    [SerializeField] private bool outerReferenceEmbedIndex;
    [SerializeField] private float multiplier = 1f; // Optional multiplier for the loaded value
    
    private int currentIndex;

    public void Init(int index)
    {
        currentIndex = index;
    }

    public override float LoadProgress()
    {
        if (string.IsNullOrEmpty(outerReferenceKey))
            return 0f;

        string key = outerReferenceEmbedIndex ? $"{outerReferenceKey}_{currentIndex}" : outerReferenceKey;
        float value = PlayerPrefs.GetFloat(key, 0f);
        return value * multiplier;
    }
}

