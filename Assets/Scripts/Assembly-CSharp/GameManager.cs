using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager _instance;

	public static bool firstPlay;

	public static float playTime;

	public static int adWatched;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
			loadData();
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		// Initialize first play if needed
		if (firstPlay)
		{
			commonClean();
		}
	}

	private void Update()
	{
		// Track play time
		playTime += Time.deltaTime;
	}

	public void resetLevel()
	{
		// Reset current level progress
		Time.timeScale = 1f;
		
		// Reset player state if exists
		if (PlayerPlatformerController._instance != null)
		{
			PlayerPlatformerController._instance.allowControl = true;
			PlayerPlatformerController._instance.gamePaused = false;
		}
		
		// Reset UI state
		if (UIManager._instance != null)
		{
			UIManager._instance.openMeun(false);
			UIManager._instance.showButton(true);
			UIManager._instance.showTopPanel(true);
		}
		
		// Reset story system
		StoryManager.resetStory();
		
		// Reload current scene
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}

	public void nextLevel()
	{
		// Save current progress
		int currentChapter = PlayerPrefs.GetInt("CurrentChapter", 1);
		int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
		
		// Increment level
		currentLevel++;
		PlayerPrefs.SetInt("CurrentLevel", currentLevel);
		PlayerPrefs.Save();
		
		// Clean up current level
		commonClean();
		
		// Load next scene
		int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
		if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
		}
		else
		{
			// End of game or return to main menu
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
	}

	public void quitLevel()
	{
		// Clean up and return to main menu
		fullClean();
		Time.timeScale = 1f;
		UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Assuming scene 0 is main menu
	}

	public void loadData()
	{
		// Load saved game data
		firstPlay = PlayerPrefs.GetInt("FirstPlay", 1) == 1;
		playTime = PlayerPrefs.GetFloat("PlayTime", 0f);
		adWatched = PlayerPrefs.GetInt("AdWatched", 0);
		
		// Initialize default values if first play
		if (firstPlay)
		{
			PlayerPrefs.SetInt("CurrentChapter", 1);
			PlayerPrefs.SetInt("CurrentLevel", 1);
			PlayerPrefs.SetInt("FirstPlay", 0);
			PlayerPrefs.Save();
			firstPlay = false;
		}
	}

	private void commonClean()
	{
		// Reset common game elements
		Time.timeScale = 1f;
		
		// Reset story system
		StoryManager.resetStory();
		
		// Reset UI state
		if (UIManager._instance != null)
		{
			UIManager._instance.openMeun(false);
			UIManager._instance.showButton(true);
			UIManager._instance.showTopPanel(true);
		}
	}

	private void fullClean()
	{
		// Perform full cleanup
		commonClean();
		
		// Save play time
		PlayerPrefs.SetFloat("PlayTime", playTime);
		PlayerPrefs.Save();
		
		// Reset any temporary game state
		if (PlayerPlatformerController._instance != null)
		{
			PlayerPlatformerController._instance.allowControl = true;
			PlayerPlatformerController._instance.gamePaused = false;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			// Save current progress when app is paused
			PlayerPrefs.SetFloat("PlayTime", playTime);
			PlayerPrefs.SetInt("AdWatched", adWatched);
			PlayerPrefs.Save();
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus)
		{
			// Save when losing focus
			PlayerPrefs.SetFloat("PlayTime", playTime);
			PlayerPrefs.Save();
		}
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			// Save before destruction
			PlayerPrefs.SetFloat("PlayTime", playTime);
			PlayerPrefs.Save();
		}
	}
}