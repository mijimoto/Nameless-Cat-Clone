using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItemController : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField]
    private Color completedColor = Color.green;

    [SerializeField]
    private Color lockedColor = Color.gray;

    [Header("UI References")]
    [SerializeField]
    private Image bgImage;

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Image lockImage;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private TextMeshProUGUI completeDateText;

    [SerializeField]
    private TextMeshProUGUI progressText;

    [SerializeField]
    private Slider progressSlider;

    private AchievementProgress achievementProgress;

    private void Awake()
    {
        // Ensure all components are assigned
        if (bgImage == null)
            bgImage = GetComponent<Image>();
        
        // Find components if not assigned
        if (titleText == null)
            titleText = transform.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
            
        if (descriptionText == null)
            descriptionText = transform.Find("DescriptionText")?.GetComponent<TextMeshProUGUI>();
            
        if (progressText == null)
            progressText = transform.Find("ProgressText")?.GetComponent<TextMeshProUGUI>();
            
        if (completeDateText == null)
            completeDateText = transform.Find("CompleteDateText")?.GetComponent<TextMeshProUGUI>();
            
        if (progressSlider == null)
            progressSlider = GetComponentInChildren<Slider>();
    }

    public void UpdateTextSetting()
    {
        var currentSetting = DataLoader.getCurrentTextBoxSetting();
        if (currentSetting != null)
        {
            // Apply text settings to all text components
            if (titleText != null)
                currentSetting.InitTextMeshProUGUI(titleText, true);
                
            if (descriptionText != null)
                currentSetting.InitTextMeshProUGUI(descriptionText, true);
                
            if (progressText != null)
                currentSetting.InitTextMeshProUGUI(progressText, true);
                
            if (completeDateText != null)
                currentSetting.InitTextMeshProUGUI(completeDateText, true);
        }
    }

    public void Init(AchievementProgress progress)
    {
        achievementProgress = progress;
        
        if (achievementProgress == null)
        {
            Debug.LogError("AchievementProgress is null in AchievementItemController.Init");
            return;
        }

        // Apply text settings
        UpdateTextSetting();
        
        // Update all content
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        if (achievementProgress == null)
            return;

        // Update icon
        if (iconImage != null && achievementProgress.Icon != null)
        {
            iconImage.sprite = achievementProgress.Icon;
        }

        // Update texts
        if (titleText != null)
        {
            titleText.text = achievementProgress.Title;
        }

        if (descriptionText != null)
        {
            descriptionText.text = achievementProgress.Description;
        }

        // Update progress display
        float progressPercentage = achievementProgress.TargetProgress > 0 ? 
            achievementProgress.Progress / achievementProgress.TargetProgress : 0f;

        if (progressSlider != null)
        {
            progressSlider.value = progressPercentage;
        }

        if (progressText != null)
        {
            progressText.text = $"{achievementProgress.Progress:F0}/{achievementProgress.TargetProgress:F0}";
        }

        // Update completion status
        bool isCompleted = achievementProgress.Completed;
        
        // Update background color
        if (bgImage != null)
        {
            bgImage.color = isCompleted ? completedColor : lockedColor;
        }

        // Show/hide lock image
        if (lockImage != null)
        {
            lockImage.gameObject.SetActive(!isCompleted);
        }

        // Update completion date
        if (completeDateText != null)
        {
            if (isCompleted)
            {
                completeDateText.text = achievementProgress.CompleteDate.ToString("MM/dd/yyyy");
                completeDateText.gameObject.SetActive(true);
            }
            else
            {
                completeDateText.gameObject.SetActive(false);
            }
        }

        // Update visual alpha based on completion
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = isCompleted ? 1.0f : 0.7f;
        }
    }

    // Method to animate progress change
    public void AnimateProgressUpdate()
    {
        if (progressSlider != null && achievementProgress != null)
        {
            float targetValue = achievementProgress.TargetProgress > 0 ? 
                achievementProgress.Progress / achievementProgress.TargetProgress : 0f;
            
            // You can implement a smooth animation here using DOTween or Unity's Animation system
            // For now, just update immediately
            progressSlider.value = targetValue;
        }
    }

    // Method called when item is clicked (if needed)
    public void OnItemClick()
    {
        // Handle achievement item click
        // Could show detailed view, play sound, etc.
        Debug.Log($"Clicked achievement: {achievementProgress?.Title}");
    }
}