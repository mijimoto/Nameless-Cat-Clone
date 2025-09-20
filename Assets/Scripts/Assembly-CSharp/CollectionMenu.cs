using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionMenu : MonoBehaviour
{
    public GameObject holder;

    [SerializeField] private RectTransform pageHolder; // PagesHolder
    [SerializeField] private RectTransform collectionHolder; // CollectionHolder object
    [SerializeField] private Transform collectionIconHolder; // Upper icons container
    [SerializeField] private Transform underBar; // Under bar indicator
    [SerializeField] private RectTransform changePageButtons; // Navigation buttons

    // Individual page objects
    [SerializeField] private GameObject achObject; // Ach
    [SerializeField] private GameObject collectionHolderObject; // CollectionHolder  
    [SerializeField] private GameObject statPageObject; // StatPage

    // Collection images inside CollectionHolder
    [SerializeField] private GameObject collectionImage1;
    [SerializeField] private GameObject collectionImage2;
    [SerializeField] private GameObject collectionImage3;

    // Upper icons for indication (5 icons total)
    [SerializeField] private Transform[] upperIcons; // Should have 5 elements

    private float lastMoveRate;
    private int currentPage = 0; // 0=Ach, 1=collection1, 2=collection2, 3=collection3, 4=StatPage
    private int currentCategory = 0; // 0=Achievement, 1=Collection, 2=Stats
    private int[] categoryLastPage = new int[3]; // Track last page for each category
    private int lastPage;
    private Vector2 originalPagePosition;
    private float pageWidth;

    private void Start()
    {
        init();
    }

    public void init()
    {
        if (pageHolder != null)
        {
            originalPagePosition = pageHolder.anchoredPosition;
            // Calculate page width - each page should be the width of the container
            RectTransform parentRect = pageHolder.parent.GetComponent<RectTransform>();
            if (parentRect != null)
            {
                pageWidth = parentRect.rect.width;
            }
            else
            {
                pageWidth = 1920f; // Fallback width
            }
        }

        // Initialize category last pages
        categoryLastPage[0] = 0; // Achievement category starts at page 0 (Ach)
        categoryLastPage[1] = 1; // Collection category starts at page 1 (collection1)
        categoryLastPage[2] = 4; // Stats category at page 4 (StatPage)

        currentPage = 0;
        currentCategory = 0;

        updateCollectionPos();
        updateCategory();
        UpdateCurrentPageContent();
    }

    public void updateCollectionPos()
    {
        if (pageHolder == null) return;

        // Move the entire PagesHolder horizontally based on current page
        Vector2 targetPosition = originalPagePosition;
        targetPosition.x = originalPagePosition.x - (currentPage * pageWidth);

        pageHolder.anchoredPosition = targetPosition;

        // Update under bar position
        updateUnderBarPosition();

        // Update collection sub-page visibility
        UpdateCollectionSubPages();
    }

    private void updateUnderBarPosition()
    {
        if (underBar == null || upperIcons == null || currentPage >= upperIcons.Length)
            return;

        // Map pages to upper icons
        int iconIndex = GetIconIndexForPage(currentPage);
        if (iconIndex >= 0 && iconIndex < upperIcons.Length)
        {
            Transform targetIcon = upperIcons[iconIndex];
            if (targetIcon != null)
            {
                Vector3 iconPosition = targetIcon.position;
                Vector3 underBarPosition = underBar.position;
                underBarPosition.x = iconPosition.x;
                underBar.position = underBarPosition;
            }
        }
    }

    private int GetIconIndexForPage(int page)
    {
        // Map pages to upper icon indices
        switch (page)
        {
            case 0: return 0; // Ach -> first icon
            case 1:
            case 2:
            case 3: return 1; // Collection pages -> second icon
            case 4: return 2; // StatPage -> third icon
            default: return 0;
        }
    }

    private void UpdateCollectionSubPages()
    {
        // Show/hide collection sub-pages based on current page
        if (collectionImage1 != null)
            collectionImage1.SetActive(currentPage == 1);
        if (collectionImage2 != null)
            collectionImage2.SetActive(currentPage == 2);
        if (collectionImage3 != null)
            collectionImage3.SetActive(currentPage == 3);
    }

    private void updateCategory()
    {
        // Determine current category based on page
        if (currentPage == 0)
        {
            currentCategory = 0; // Achievement
        }
        else if (currentPage >= 1 && currentPage <= 3)
        {
            currentCategory = 1; // Collection
        }
        else if (currentPage == 4)
        {
            currentCategory = 2; // Stats
        }

        // Store the last page for this category
        categoryLastPage[currentCategory] = currentPage;

        updateChapterButton();
    }

    public void changePage(bool left)
    {
        int newPage = currentPage;

        if (left)
        {
            newPage = Mathf.Max(0, currentPage - 1);
        }
        else
        {
            newPage = Mathf.Min(4, currentPage + 1); // 5 pages total (0-4)
        }

        if (newPage != currentPage)
        {
            currentPage = newPage;
            updateCollectionPos();
            updateCategory();
            UpdateCurrentPageContent();
        }
    }

    public void goCategory(int category)
    {
        if (category >= 0 && category < 3)
        {
            // Go to the last page of the specified category
            int targetPage = categoryLastPage[category];

            if (targetPage != currentPage)
            {
                currentPage = targetPage;
                currentCategory = category;

                updateCollectionPos();
                updateCategory();
                UpdateCurrentPageContent();
            }
        }
    }

    private void updateChapterButton()
    {
        // Update navigation buttons visibility/state
        if (changePageButtons != null)
        {
            changePageButtons.gameObject.SetActive(true);

            // You can add logic here to disable left/right buttons at boundaries
            Button leftButton = changePageButtons.GetChild(0)?.GetComponent<Button>();
            Button rightButton = changePageButtons.GetChild(1)?.GetComponent<Button>();

            if (leftButton != null)
                leftButton.interactable = currentPage > 0;
            if (rightButton != null)
                rightButton.interactable = currentPage < 4;
        }
    }

    private void UpdateCurrentPageContent()
    {
        switch (currentPage)
        {
            case 0: // Ach page
                UpdateAchievementPage();
                break;
            case 1: // Collection 1
            case 2: // Collection 2  
            case 3: // Collection 3
                UpdateCollectionPage();
                break;
            case 4: // StatPage
                UpdateStatPage();
                break;
        }
    }

    private void UpdateAchievementPage()
    {
        if (achObject != null)
        {
            AchievementController achController = achObject.GetComponentInChildren<AchievementController>();
            if (achController != null)
            {
                achController.UpdateProgress();
            }
            else
            {
                Debug.LogWarning("AchievementController not found in Ach object hierarchy");
            }
        }
    }

    private void UpdateCollectionPage()
    {
        // Update collection unlock status for current collection page
        int collectionIndex = currentPage - 1; // Pages 1,2,3 map to collection indices 0,1,2

        if (collectionIndex >= 0 && collectionIndex < 3)
        {
            bool isUnlocked = ShouldUnlockCollection(collectionIndex);

            // Save unlock status
            if (isUnlocked)
            {
                PlayerPrefs.SetInt($"Collection_{collectionIndex}_Unlocked", 1);
            }

            // Update visual representation
            GameObject currentCollectionImage = GetCollectionImageObject(collectionIndex);
            if (currentCollectionImage != null)
            {
                Image[] images = currentCollectionImage.GetComponentsInChildren<Image>();
                foreach (var img in images)
                {
                    Color imageColor = img.color;
                    imageColor.a = isUnlocked ? 1f : 0.3f;
                    img.color = imageColor;
                }
            }
        }
    }

    private GameObject GetCollectionImageObject(int index)
    {
        switch (index)
        {
            case 0: return collectionImage1;
            case 1: return collectionImage2;
            case 2: return collectionImage3;
            default: return null;
        }
    }

private void UpdateStatPage()
{
    if (statPageObject == null) return;

    for (int i = 0; i < statPageObject.transform.childCount; i++)
    {
        Transform statHolder = statPageObject.transform.GetChild(i);
        if (!statHolder.name.Contains("StatHolder")) continue;

        // Get holder index from name
        string numberPart = System.Text.RegularExpressions.Regex.Match(statHolder.name, @"\d+").Value;
        if (string.IsNullOrEmpty(numberPart) || !int.TryParse(numberPart, out int holderIndex))
            continue;

        // Update the label (LocalizationText)
        LocalizationText localizationText = statHolder.GetComponentInChildren<LocalizationText>();
        if (localizationText != null)
        {
            string statKey = GetStatLocalizationKey(holderIndex);
            localizationText.SetKey(statKey);
        }

        // 🔑 Update the "Value" child
        Transform valueChild = statHolder.Find("Value");
        if (valueChild != null)
        {
            TMPro.TextMeshProUGUI valueText = valueChild.GetComponent<TMPro.TextMeshProUGUI>();
            if (valueText != null)
            {
                string statValue = GetStatValue(holderIndex);
                valueText.text = statValue;
            }
        }
    }
}



private string GetStatLocalizationKey(int holderIndex)
{
    switch (holderIndex)
    {
        case 7: return "stat_play_time";
        case 9: return "stat_dead_count";
        case 10: return "stat_move_distance";
        case 11: return "stat_jump_count";
        case 12: return "stat_block_attach_count";
        case 13: return "stat_block_trade_count";
        default: return "stat_unknown";
    }
}

private string GetStatValue(int holderIndex)
{
    switch (holderIndex)
    {
        case 7: // Play Time
            float gameTime = PlayerPrefs.GetFloat("PlayTime", 0f) + GameManager.playTime;
            int hours = (int)(gameTime / 3600);
            int minutes = (int)((gameTime % 3600) / 60);
            int seconds = (int)(gameTime % 60);
            if (hours > 0)
                return $"{hours}h {minutes}m";
            else if (minutes > 0)
                return $"{minutes}m {seconds}s";
            else
                return $"{seconds}s";

        case 9: return PlayerPrefs.GetInt("DeadCount", 0).ToString();
        case 10: 
            float moveDistance = PlayerPrefs.GetFloat("MoveDistance", 0f);
            return moveDistance >= 1000
                ? $"{moveDistance / 1000f:F1}km"
                : $"{moveDistance:F1}m";

        case 11: return PlayerPrefs.GetInt("JumpCount", 0).ToString();
        case 12: return PlayerPrefs.GetInt("BlockAttachCount", 0).ToString();
        case 13: return PlayerPrefs.GetInt("BlockTradeCount", 0).ToString();
        default: return "0";
    }
}
    private bool ShouldUnlockCollection(int collectionIndex)
    {
        // Check if already unlocked
        if (PlayerPrefs.GetInt($"Collection_{collectionIndex}_Unlocked", 0) == 1)
            return true;

        // Define unlock conditions
        switch (collectionIndex)
        {
            case 0: // First collection
                return PlayerPrefs.GetInt("Chapter_1_Completed", 0) == 1;
            case 1: // Second collection  
                return PlayerPrefs.GetInt("Chapter_2_Completed", 0) == 1;
            case 2: // Third collection
                return GetTotalAchievementsCompleted() >= 2; // Need at least 2 achievements
            default:
                return false;
        }
    }

    private int GetTotalAchievementsCompleted()
    {
        int completed = 0;
        if (DataLoader.AchievementDatas != null)
        {
            foreach (var achievementData in DataLoader.AchievementDatas)
            {
                if (achievementData?.Progresses != null)
                {
                    foreach (var progress in achievementData.Progresses)
                    {
                        if (progress != null && progress.Completed)
                        {
                            completed++;
                        }
                    }
                }
            }
        }
        return completed;
    }

    // UI Button Callbacks
    public void OnLeftButtonClick()
    {
        changePage(true);
    }

    public void OnRightButtonClick()
    {
        changePage(false);
    }

    public void OnAchievementTabClick()
    {
        goCategory(0);
    }

    public void OnCollectionTabClick()
    {
        goCategory(1);
    }

    public void OnStatTabClick()
    {
        goCategory(2);
    }
}