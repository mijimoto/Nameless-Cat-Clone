using UnityEngine;

[CreateAssetMenu(fileName = "New Outer Key Reference", menuName = "Achievements/Outer Key Reference")]
internal class AchievementProgressReferenceOuterKey : AchievementProgressReference
{
    [SerializeField]
    private string outerReferenceKey;

    [SerializeField]
    private bool outerReferenceEmbedIndex;

    private int currentIndex;

    public void Init(int index)
    {
        currentIndex = index;
    }

    public override float LoadProgress()
    {
        if (string.IsNullOrEmpty(outerReferenceKey))
            return 0f;

        string keyToUse = outerReferenceKey;
        
        // Embed index in the key if required
        if (outerReferenceEmbedIndex)
        {
            keyToUse = $"{outerReferenceKey}_{currentIndex}";
        }

        // Load progress from PlayerPrefs using the constructed key
        return PlayerPrefs.GetFloat(keyToUse, 0f);
    }

    public void SaveProgress(float progress)
    {
        if (string.IsNullOrEmpty(outerReferenceKey))
            return;

        string keyToUse = outerReferenceKey;
        
        if (outerReferenceEmbedIndex)
        {
            keyToUse = $"{outerReferenceKey}_{currentIndex}";
        }

        PlayerPrefs.SetFloat(keyToUse, progress);
        PlayerPrefs.Save();
    }

    // Method to check if this reference is valid
    public bool IsValidReference()
    {
        return !string.IsNullOrEmpty(outerReferenceKey);
    }

    // Method to get the full key that would be used
    public string GetFullKey()
    {
        if (string.IsNullOrEmpty(outerReferenceKey))
            return "";

        return outerReferenceEmbedIndex ? $"{outerReferenceKey}_{currentIndex}" : outerReferenceKey;
    }
}