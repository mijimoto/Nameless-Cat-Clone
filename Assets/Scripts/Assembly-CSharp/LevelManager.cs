using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

	public bool CompletelyFirstPlay => false;

	public static void Reset()
	{
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void updateChapterButton()
	{
	}

	public void changeScene(string sceneName)
	{
	}

	public void changeChapter(bool left)
	{
	}

	public static string currentLevel()
	{
		return null;
	}

	public List<string> getCurrentStory()
	{
		return null;
	}

	public void loadChapters()
	{
	}

	private void loadChapter(int chapter)
	{
	}

	public void reloadCurrentLevel()
	{
	}

	public static string currentLevelName()
	{
		return null;
	}

	public void toNextLevel(bool win)
	{
	}

	public void toLevelView()
	{
	}

	public static string getLevelName(int chapter, int level)
	{
		return null;
	}

	public static int getChapter(string levelName)
	{
		return 0;
	}

	public void updateChapter(string levelName)
	{
	}

	public bool isLastLevel(string sceneName)
	{
		return false;
	}

	public string nextChapterString(string sceneName, int level = -1)
	{
		return null;
	}

	public string nextLevelString(string sceneName)
	{
		return null;
	}

	public int getCurrentChapter()
	{
		return 0;
	}

	public static int getCurrentLevelWithoutChapter()
	{
		return 0;
	}

	public void goMenu()
	{
	}

	public void playLevelMusic()
	{
	}

	public void UpdateBox()
	{
	}
}
