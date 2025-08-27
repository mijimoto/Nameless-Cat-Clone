using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public static LevelManager _instance;

	public static LevelLoader levelLoader;

	public static int[] chapterLastLevel;

	private static int currentChapter;

	private int lastChapter;

	public RectTransform chapterButton;

	public RectTransform chapterList;

	public TextMeshProUGUI boxText;

	public GameObject fishSprite;

	private float lastMoveRate;

	private bool disableControl;

	public bool CompletelyFirstPlay => PlayerPrefs.GetInt("FirstPlay", 1) == 1;

	public static void Reset()
	{
		currentChapter = 0;
		chapterLastLevel = null;
		levelLoader = null;
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
			levelLoader = new LevelLoader();
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		loadChapters();
		updateChapterButton();
		UpdateBox();

		// Hide left arrow initially
		if (chapterButton != null)
		{
			Transform leftArrow = chapterButton.Find("left");
			if (leftArrow != null)
				leftArrow.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		// Handle input or update logic here if needed
	}

	private void updateChapterButton()
	{
		if (chapterButton != null)
		{
			// Update chapter button UI based on current chapter
		}
	}

	public void changeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void changeChapter(bool left)
	{
		if (left)
			currentChapter = Mathf.Max(0, currentChapter - 1);
		else
			currentChapter = Mathf.Min(chapterList.childCount - 1, currentChapter + 1);

		string newLevel = getLevelName(currentChapter, 1);
		PlayerPrefs.SetString("CurrentLevel", newLevel);

		loadChapter(currentChapter);
		updateChapterButton();
		UpdateBox();

		if (chapterList != null && chapterList.childCount > currentChapter)
		{
			RectTransform targetChapter = chapterList.GetChild(currentChapter) as RectTransform;
			if (targetChapter != null)
			{
				// Force layout rebuild to get correct positions
				LayoutRebuilder.ForceRebuildLayoutImmediate(chapterList);

				// Move chapterList
				chapterList.anchoredPosition = new Vector2(-targetChapter.anchoredPosition.x, chapterList.anchoredPosition.y);
			}
		}

		// --- Hide/Show arrow buttons ---
		if (chapterButton != null)
		{
			Transform leftArrow = chapterButton.Find("left");
			Transform rightArrow = chapterButton.Find("right");

			if (leftArrow != null)
				leftArrow.gameObject.SetActive(currentChapter > 0);

			if (rightArrow != null)
				rightArrow.gameObject.SetActive(currentChapter < chapterList.childCount - 1);
		}
	}

	public static string currentLevel()
	{
		return PlayerPrefs.GetString("CurrentLevel", "1-1");
	}

	public List<string> getCurrentStory()
	{
		string currentLevelName = currentLevel();
		if (levelLoader != null)
		{
			return levelLoader.getLevelStory(currentLevelName);
		}
		return new List<string>();
	}

	public void loadChapters()
	{
		if (chapterLastLevel == null)
		{
			chapterLastLevel = new int[] { 15, 16, 16, 1 };
		}

		currentChapter = getChapter(currentLevel());
		loadChapter(currentChapter);
	}

	private void loadChapter(int chapter)
	{
		currentChapter = chapter;
		// Load chapter-specific data here
	}

	public void reloadCurrentLevel()
	{
		string levelName = currentLevel();
		changeScene(levelName);
	}

	public static string currentLevelName()
	{
		return currentLevel();
	}

	public void toNextLevel(bool win)
	{
		if (win)
		{
			string nextLevel = nextLevelString(currentLevel());
			if (!string.IsNullOrEmpty(nextLevel))
			{
				PlayerPrefs.SetString("CurrentLevel", nextLevel);
				changeScene(nextLevel);
			}
		}
		else
		{
			reloadCurrentLevel();
		}
	}

	public void toLevelView()
	{
		changeScene("LevelSelect");
	}

public static string getLevelName(int chapter, int level)
{
    return $"level{chapter + 1}-{level}";
}


	public static int getChapter(string levelName)
	{
		if (string.IsNullOrEmpty(levelName))
			return 0;

		string[] parts = levelName.Split('-');
		if (parts.Length >= 1 && int.TryParse(parts[0], out int chapter))
		{
			return chapter - 1; // Convert to 0-based index
		}

		return 0;
	}

	public void updateChapter(string levelName)
	{
		currentChapter = getChapter(levelName);
		loadChapter(currentChapter);
	}

	public bool isLastLevel(string sceneName)
	{
		int chapter = getChapter(sceneName);
		int level = getCurrentLevelWithoutChapter();

		if (chapterLastLevel != null && chapter < chapterLastLevel.Length)
		{
			return level >= chapterLastLevel[chapter];
		}

		return false;
	}

	public string nextChapterString(string sceneName, int level = -1)
	{
		int chapter = getChapter(sceneName);
		int nextChapter = chapter + 1;

		if (chapterLastLevel != null && nextChapter < chapterLastLevel.Length)
		{
			return getLevelName(nextChapter, 1);
		}

		return null;
	}

	public string nextLevelString(string sceneName)
	{
		int chapter = getChapter(sceneName);
		int level = getCurrentLevelWithoutChapter();
		int nextLevel = level + 1;

		if (chapterLastLevel != null && chapter < chapterLastLevel.Length)
		{
			if (nextLevel <= chapterLastLevel[chapter])
			{
				return getLevelName(chapter, nextLevel); // next level in the same chapter
			}
			else
			{
				return nextChapterString(sceneName); // go to next chapter first level
			}
		}

		return null; // no next level
	}


	public int getCurrentChapter()
	{
		return currentChapter;
	}

	public static int getCurrentLevelWithoutChapter()
	{
		string levelName = currentLevel();
		if (string.IsNullOrEmpty(levelName))
			return 1;

		string[] parts = levelName.Split('-');
		if (parts.Length >= 2 && int.TryParse(parts[1], out int level))
		{
			return level;
		}

		return 1;
	}

	public void goMenu()
	{
		changeScene("MainMenu");
	}

	public void playLevelMusic()
	{
		// Play level-specific music here
	}

	public void UpdateBox()
	{
		if (boxText != null)
		{
			boxText.text = TextLoader.getText("current_level") + ": " + currentLevelName();
		}
	}
}