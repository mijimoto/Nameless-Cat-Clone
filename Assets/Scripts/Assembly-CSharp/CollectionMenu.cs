using UnityEngine;
using UnityEngine.UI;

public class CollectionMenu : MonoBehaviour
{
    public GameObject holder;
    
    [SerializeField]
    private RectTransform pageHolder;
    [SerializeField]
    private RectTransform collectionHolder;
    [SerializeField]
    private Transform collectionIconHolder;
    [SerializeField]
    private Transform underBar;
    [SerializeField]
    private RectTransform changePageButtons;
    
    private float lastMoveRate;
    private int currentPage;
    private int currentCategory;
    private int[] categoryLastPage;
    private int lastPage;
    
    // Additional fields that might be needed
    private int totalCategories = 3; // Adjust based on your game's categories
    private float pageWidth = 800f; // Adjust based on your UI layout
    private float animationSpeed = 5f;
    
    private void Start()
    {
        init();
    }
    
    public void init()
    {
        // Initialize category pages tracking
        categoryLastPage = new int[totalCategories];
        
        // Set default values
        currentPage = 0;
        currentCategory = 0;
        lastMoveRate = 0f;
        
        // Calculate last pages for each category
        // This would typically be based on your collection data
        for (int i = 0; i < totalCategories; i++)
        {
            categoryLastPage[i] = calculateLastPageForCategory(i);
        }
        
        lastPage = categoryLastPage[currentCategory];
        
        // Update initial positions and UI
        updateCollectionPos();
        updateCategory();
        updateChapterButton();
    }
    
    private int calculateLastPageForCategory(int category)
    {
        // This should return the number of pages for each category
        // You'll need to implement this based on your collection data
        // For example, if you have a CollectionManager or GameData
        
        // Placeholder logic - adjust based on your game's structure
        switch (category)
        {
            case 0: return 2; // Category 0 has 3 pages (0, 1, 2)
            case 1: return 1; // Category 1 has 2 pages (0, 1)
            case 2: return 3; // Category 2 has 4 pages (0, 1, 2, 3)
            default: return 0;
        }
    }
    
    public void updateCollectionPos()
    {
        if (collectionHolder != null)
        {
            // Calculate target position based on current page
            float targetX = -currentPage * pageWidth;
            
            // Smooth movement animation
            Vector3 currentPos = collectionHolder.anchoredPosition;
            float newX = Mathf.Lerp(currentPos.x, targetX, Time.deltaTime * animationSpeed);
            
            collectionHolder.anchoredPosition = new Vector2(newX, currentPos.y);
            
            // Update move rate for animation detection
            lastMoveRate = Mathf.Abs(newX - currentPos.x);
        }
        
        // Update underbar position to indicate current page
        if (underBar != null)
        {
            float underBarTargetX = currentPage * (pageWidth / (lastPage + 1));
            Vector3 underBarPos = underBar.localPosition;
            underBar.localPosition = new Vector3(underBarTargetX, underBarPos.y, underBarPos.z);
        }
    }
    
    private void updateCategory()
    {
        // Update category-specific UI elements
        if (collectionIconHolder != null)
        {
            // This might involve highlighting the current category icon
            for (int i = 0; i < collectionIconHolder.childCount; i++)
            {
                Transform icon = collectionIconHolder.GetChild(i);
                // You might want to change color, scale, or other properties
                icon.GetComponent<Image>().color = (i == currentCategory) ? Color.white : Color.gray;
            }
        }
        
        // Update the last page for current category
        lastPage = categoryLastPage[currentCategory];
        
        // Reset to first page when switching categories
        currentPage = 0;
        
        updateChapterButton();
    }
    
    public void changePage(bool left)
    {
        // Play UI sound
        if (SoundManager._instance != null)
            SoundManager._instance.uiSound("click");
            
        if (left)
        {
            // Move to previous page
            if (currentPage > 0)
            {
                currentPage--;
            }
        }
        else
        {
            // Move to next page
            if (currentPage < lastPage)
            {
                currentPage++;
            }
        }
        
        updateChapterButton();
    }
    
    public void goCategory(int category)
    {
        if (category < 0 || category >= totalCategories || category == currentCategory)
            return;
            
        // Play UI sound
        if (SoundManager._instance != null)
            SoundManager._instance.uiSound("click");
            
        currentCategory = category;
        updateCategory();
    }
    
    private void updateChapterButton()
    {
        // Update page navigation buttons visibility/interactability
        if (changePageButtons != null)
        {
            // Assuming the changePageButtons has left and right button children
            if (changePageButtons.childCount >= 2)
            {
                Button leftButton = changePageButtons.GetChild(0).GetComponent<Button>();
                Button rightButton = changePageButtons.GetChild(1).GetComponent<Button>();
                
                if (leftButton != null)
                    leftButton.interactable = currentPage > 0;
                    
                if (rightButton != null)
                    rightButton.interactable = currentPage < lastPage;
            }
        }
        
        // Update any page indicators or progress bars
        // You might have dots or numbers showing current page
    }
    
    private void Update()
    {
        // Continuously update collection position for smooth animation
        if (lastMoveRate > 0.1f) // Only update if there's significant movement
        {
            updateCollectionPos();
        }
    }
}