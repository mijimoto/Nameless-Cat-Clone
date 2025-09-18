using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
internal class AchievementProgressReferenceOuterKey : AchievementProgressReference
{
    [SerializeField] private string outerReferenceKey;
    [SerializeField] private bool outerReferenceEmbedIndex;
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
        return PlayerPrefs.GetFloat(key, 0f);
    }
}
