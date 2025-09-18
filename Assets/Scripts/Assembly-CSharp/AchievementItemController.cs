using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItemController : MonoBehaviour
{
    [SerializeField] private Color completedColor = Color.green;
    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI completeDateText;
    [SerializeField] private TextMeshProUGUI progressText;

    private AchievementProgress achievementProgress;

    private void Awake()
    {
        UpdateTextSetting();
    }

    public void UpdateTextSetting()
    {
        TextBoxSetting setting = DataLoader.getCurrentTextBoxSetting();
        if (setting != null)
        {
            if (titleText != null) setting.InitTextMeshProUGUI(titleText, false);
            if (descriptionText != null) setting.InitTextMeshProUGUI(descriptionText, false);
            if (completeDateText != null) setting.InitTextMeshProUGUI(completeDateText, false);
            if (progressText != null) setting.InitTextMeshProUGUI(progressText, false);
        }
    }

    public void Init(AchievementProgress progress)
    {
        achievementProgress = progress;
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        if (achievementProgress == null) return;

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

        if (progressText != null)
        {
            float currentProgress = achievementProgress.Progress;
            float targetProgress = achievementProgress.TargetProgress;
            progressText.text = $"{currentProgress:F0}/{targetProgress:F0}";
        }

        // Update completion status
        bool isCompleted = achievementProgress.Completed;
        
        if (bgImage != null)
        {
            bgImage.color = isCompleted ? completedColor : lockedColor;
        }

        if (lockImage != null)
        {
            lockImage.gameObject.SetActive(!isCompleted);
        }

        if (completeDateText != null)
        {
            if (isCompleted)
            {
                completeDateText.text = achievementProgress.CompleteDate.ToString("dd/MM/yyyy");
                completeDateText.gameObject.SetActive(true);
            }
            else
            {
                completeDateText.gameObject.SetActive(false);
            }
        }
    }
}