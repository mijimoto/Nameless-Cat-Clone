using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
	public static StoryManager _instance;

	public static GameObject outerStoryController;

	private static bool dontShowStory;

	private bool waitForResponse;

	private bool waitingForInput;

	private bool useMessageBox;

	public TextBox textPrefab;

	private TextBox textBox;

	private bool spawnEffect;

	private bool skipMessage;

	private bool skipButton;

	private GameObject callbackObject;

	private Transform godTrans;

	private Dictionary<string, List<string>> storyData;
	private List<string> currentStoryLines;
	private int currentLineIndex;
	private bool isPlayingStory;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
			LoadStoryData();
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		spawnEffect = true;
	}

	public static void resetStory()
	{
		dontShowStory = false;
	}

	private void Update()
	{
		if (waitingForInput && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
		{
			nextStory();
		}
	}

	public bool canLoadStory()
	{
		string currentChapter = PlayerPrefs.GetInt("CurrentChapter", 1).ToString();
		string currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1).ToString();
		string storyId = currentChapter + "-" + currentLevel;
		return storyData != null && storyData.ContainsKey(storyId) && !dontShowStory;
	}

	public void loadStory()
	{
		if (canLoadStory())
		{
			string currentChapter = PlayerPrefs.GetInt("CurrentChapter", 1).ToString();
			string currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1).ToString();
			string storyId = currentChapter + "-" + currentLevel;
			callStory(storyId);
		}
	}

	public void callStory(string id, GameObject callback = null)
	{
		if (isPlayingStory) return;
		
		List<string> storyLines = PrepareCallStory(id);
		if (storyLines != null && storyLines.Count > 0)
		{
			callbackObject = callback;
			StartCoroutine(playStory(id, storyLines));
		}
		else if (callback != null)
		{
			callback.SendMessage("storyEnd", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void callStoryAndWait(string id, GameObject callback = null)
	{
		callStory(id, callback);
		waitForResponse = true;
	}

	private List<string> PrepareCallStory(string id)
	{
		if (storyData == null || !storyData.ContainsKey(id))
		{
			return null;
		}
		return new List<string>(storyData[id]);
	}

	public void playEmptyStory(GameObject callback, int outerAmount = 0)
	{
		callbackObject = callback;
		if (callback != null)
		{
			callback.SendMessage("storyEnd", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void playCustomStory(GameObject callback, string command)
	{
		List<string> commands = new List<string> { command };
		playCustomStory(callback, commands);
	}

	public void playCustomStory(GameObject callback, List<string> tmp)
	{
		callbackObject = callback;
		StartCoroutine(playStory("custom", tmp));
	}

	private IEnumerator playStory(string id, List<string> tmp)
	{
		isPlayingStory = true;
		currentStoryLines = tmp;
		currentLineIndex = 0;

		// Enter cinema mode
		EnterCinemaMode();

		while (currentLineIndex < currentStoryLines.Count)
		{
			string line = currentStoryLines[currentLineIndex].Trim();
			
			if (string.IsNullOrEmpty(line))
			{
				currentLineIndex++;
				continue;
			}

			if (line.StartsWith("#"))
			{
				yield return StartCoroutine(ProcessCommand(line.Substring(1)));
			}
			else if (line.Contains("|"))
			{
				yield return StartCoroutine(ProcessDialogue(line));
			}

			currentLineIndex++;
		}

		EndStory();
	}

	private void EnterCinemaMode()
	{
		// Disable player control
		if (PlayerPlatformerController._instance != null)
		{
			PlayerPlatformerController._instance.allowControl = false;
		}

		// Hide all UI elements except skip button
		if (UIManager._instance != null)
		{
			UIManager._instance.showButton(false); // Hide control buttons
			UIManager._instance.showTopPanel(false); // Hide top panel
			UIManager._instance.showSkipBtn(true); // Show skip button
			
			// Show movie bars for cinema effect
			UIManager._instance.movieBarActive(UIManager.CoverAni.fadeIn);
		}
	}

	private void ExitCinemaMode()
	{
		// Hide movie bars
		if (UIManager._instance != null)
		{
			UIManager._instance.movieBarActive(UIManager.CoverAni.fadeOut);
		}
		
		// Small delay to let movie bars fade out before showing UI
		StartCoroutine(RestoreUIAfterDelay());
	}

	private IEnumerator RestoreUIAfterDelay()
	{
		yield return new WaitForSeconds(0.5f); // Wait for movie bars to fade out
		
		if (UIManager._instance != null)
		{
			UIManager._instance.showButton(true); // Show control buttons
			UIManager._instance.showTopPanel(true); // Show top panel
			UIManager._instance.showSkipBtn(false); // Hide skip button
		}
	}

	private IEnumerator ProcessCommand(string command)
	{
		switch (command.ToLower())
		{
			case "nospawneffect": noSpawnEffect(); break;
			case "hideactivebutton": hideActiveButton(); break;
			case "dropdown": dropDown(); break;
			case "godleave": godLeave(); break;
			case "waitforground": yield return StartCoroutine(checkPlayerOnground()); break;
			case "unlockskill": unlockSkill(); break;
			case "activebuttonlight": activeButtonLight(); break;
			case "wait1second": wait1Second(); yield return new WaitForSeconds(1f); break;
			case "wait3second": wait3Second(); yield return new WaitForSeconds(3f); break;
			case "hideskipbutton": hideSkipButton(); break;
			case "nextlevel": nextLevel(); break;
			case "justwait": justWait(); break;
			case "waitforinput": waitForInput(); break;
			case "useoutercontroller": useOuterController(); break;
			case "useoutercontroller2": useOuterController2(); break;
			case "useoutercontroller3": useOuterController3(); break;
			case "rotatecamera": rotateCamera(); break;
			case "trailertitleshowafter3seound": trailertitleShowAfter3Seound(); break;
		}

		yield break;
	}

private IEnumerator ProcessDialogue(string line)
{
    string[] parts = line.Split('|');
    if (parts.Length >= 2)
    {
        string speaker = parts[0].Trim();
        string message = parts[1].Trim().Replace("\\n", "\n");

        // Try to find speaker in scene
        GameObject speakerObj = GameObject.Find(speaker);

        if (speakerObj != null)
        {
            // If an existing textbox exists but parent is not the speaker, remove it so we can attach to correct speaker
            if (textBox != null && textBox.transform.parent != speakerObj.transform)
            {
                Destroy(textBox.gameObject);
                textBox = null;
            }

            // Instantiate textBox as child of speaker (world-space)
            if (textBox == null && textPrefab != null)
            {
                textBox = Instantiate(textPrefab, speakerObj.transform);

                if (DataLoader.getCurrentTextBoxSetting() != null)
                    textBox.Init(DataLoader.getCurrentTextBoxSetting());

                // Force any Canvas in the prefab to WorldSpace so it renders in world coordinates
                Canvas[] childCanvases = textBox.GetComponentsInChildren<Canvas>(true);
                foreach (Canvas c in childCanvases)
                {
                    c.renderMode = RenderMode.WorldSpace;
                    c.worldCamera = Camera.main;
                }

                // Small world-space scale tweak (tweak multiplier to fit your scene)
                textBox.transform.localScale = Vector3.one * 0.01f;

                // Place above the speaker's head (local space) - lowered slightly
                textBox.transform.localPosition = new Vector3(-0.75f, 0.9f, 0f);
                textBox.transform.localRotation = Quaternion.identity;
            }
            else if (textBox != null)
            {
                // Ensure it's parented and positioned correctly if already exists
                textBox.transform.SetParent(speakerObj.transform, false);
                textBox.transform.localPosition = new Vector3(-0.75f, 0.9f, 0f);
            }

            if (textBox != null)
            {
                // Update text (no speaker name)
                textBox.UpdateText(message);

                textBox.playAnimation(false);

                waitingForInput = true;
                yield return new WaitUntil(() => !waitingForInput);

                textBox.playAnimation(true);
                yield return new WaitForSeconds(0.3f);
            }
        }
        else
        {
            // Fallback: if speaker not found, use existing behaviour but instantiate without canvas parenting
            if (textBox == null && textPrefab != null)
            {
                textBox = Instantiate(textPrefab);
                if (DataLoader.getCurrentTextBoxSetting() != null)
                    textBox.Init(DataLoader.getCurrentTextBoxSetting());

                // Try to set any canvas to WorldSpace so it is visible regardless of UI mode
                Canvas[] childCanvases = textBox.GetComponentsInChildren<Canvas>(true);
                foreach (Canvas c in childCanvases)
                {
                    c.renderMode = RenderMode.WorldSpace;
                    c.worldCamera = Camera.main;
                }

                textBox.transform.position = new Vector3(0f, 1.5f, 0f); // basic fallback position
                textBox.transform.localScale = Vector3.one * 0.01f;
            }

            if (textBox != null)
            {
                textBox.UpdateText(message);

                textBox.playAnimation(false);

                waitingForInput = true;
                yield return new WaitUntil(() => !waitingForInput);

                textBox.playAnimation(true);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}

	private void SetupTextBox()
	{
		if (textBox == null) return;

		// The main TextBox object uses regular Transform, TextBox2Body has the RectTransform
		Transform textBoxBody = textBox.transform.Find("TextBox2Body");
		if (textBoxBody != null)
		{
			RectTransform bodyRect = textBoxBody.GetComponent<RectTransform>();
			if (bodyRect != null)
			{
				// Set anchors to stretch bottom portion of screen
				bodyRect.anchorMin = new Vector2(0f, 0f);
				bodyRect.anchorMax = new Vector2(1f, 0.35f);
				
				// Set offsets to create padding
				bodyRect.offsetMin = new Vector2(50f, 50f); // Left and bottom padding
				bodyRect.offsetMax = new Vector2(-50f, -20f); // Right and top padding
				
				// Ensure it's positioned correctly
				bodyRect.localPosition = Vector3.zero;
				bodyRect.localScale = Vector3.one;
				
				Debug.Log($"TextBox2Body RectTransform configured");
				Debug.Log($"TextBox2Body position: {bodyRect.anchoredPosition}");
				Debug.Log($"TextBox2Body size: {bodyRect.sizeDelta}");
			}
		}
		else
		{
			Debug.LogError("TextBox2Body not found as child of TextBox!");
		}

		// Debug the TextBox setup
		Debug.Log($"TextBox created: {textBox.name}");
		Debug.Log($"TextBox2Body found: {textBoxBody != null}");
	}

	private void EndStory()
	{
		isPlayingStory = false;
		waitingForInput = false;

		// Destroy textbox
		if (textBox != null)
		{
			Destroy(textBox.gameObject);
			textBox = null;
		}

		// Exit cinema mode
		ExitCinemaMode();

		// Re-enable player control
		if (PlayerPlatformerController._instance != null)
		{
			PlayerPlatformerController._instance.allowControl = true;
		}

		// Call story end on UIManager
		if (UIManager._instance != null)
		{
			UIManager._instance.storyEnd();
		}

		// Call callback if exists
		if (callbackObject != null)
		{
			callbackObject.SendMessage("storyEnd", SendMessageOptions.DontRequireReceiver);
			callbackObject = null;
		}
	}

	private void LoadStoryData()
	{
		storyData = new Dictionary<string, List<string>>();
		
		// Try to load story file based on current language
		string languageKey = DataLoader.usingFileLan ?? "en";
		TextAsset storyFile = Resources.Load<TextAsset>("story_" + languageKey);
		
		// Fallback to default language if current language file doesn't exist
		if (storyFile == null)
		{
			storyFile = Resources.Load<TextAsset>("story_" + DataLoader.DEFAULT_LANGUAGE_KEY);
		}

		// Final fallback to English
		if (storyFile == null)
		{
			storyFile = Resources.Load<TextAsset>("story_en");
		}

		if (storyFile != null)
		{
			ParseStoryFile(storyFile.text);
		}
		else
		{
			Debug.LogWarning("No story file found! Make sure you have story files in Resources folder.");
		}
	}

	private void ParseStoryFile(string content)
	{
		string[] lines = content.Split('\n');
		string currentStoryId = null;
		List<string> currentStory = null;

		foreach (string line in lines)
		{
			string trimmedLine = line.Trim();

			if (string.IsNullOrEmpty(trimmedLine))
				continue;

			if (trimmedLine.EndsWith(":"))
			{
				// Save previous story if exists
				if (currentStoryId != null && currentStory != null)
				{
					storyData[currentStoryId] = currentStory;
				}

				// Start new story
				currentStoryId = trimmedLine.Substring(0, trimmedLine.Length - 1);
				currentStory = new List<string>();
			}
			else if (trimmedLine == ";")
			{
				// End current story
				if (currentStoryId != null && currentStory != null)
				{
					storyData[currentStoryId] = currentStory;
				}
				currentStoryId = null;
				currentStory = null;
			}
			else if (currentStory != null)
			{
				// Add line to current story
				currentStory.Add(trimmedLine);
			}
		}

		// Handle case where file doesn't end with ";"
		if (currentStoryId != null && currentStory != null)
		{
			storyData[currentStoryId] = currentStory;
		}

		Debug.Log($"Loaded {storyData.Count} stories from story file");
	}

	public void skipStory()
	{
		if (isPlayingStory)
		{
			StopAllCoroutines();
			EndStory();
		}
	}

	public void nextStory()
	{
		waitingForInput = false;
	}

	// Command implementations
	private void noSpawnEffect()
	{
		spawnEffect = false;
	}

	private void justWait()
	{
		// Empty implementation - just waits for the coroutine
	}

	private void useOuterController()
	{
		if (outerStoryController != null)
		{
			outerStoryController.SetActive(true);
		}
	}

	private void useOuterController2()
	{
		// Implementation for outer controller 2
	}

	private void useOuterController3()
	{
		// Implementation for outer controller 3
	}

	private void nextLevel()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
	}

	private void wait1Second()
	{
		// Empty - time is handled in ProcessCommand
	}

	private void wait3Second()
	{
		// Empty - time is handled in ProcessCommand
	}

	private IEnumerator waitTime(float time)
	{
		yield return new WaitForSeconds(time);
	}

	private void hideSkipButton()
	{
		if (UIManager._instance != null)
		{
			UIManager._instance.showSkipBtn(false);
		}
	}

	private void hideActiveButton()
	{
		if (UIManager._instance != null)
		{
			UIManager._instance.showActiveButton(false);
		}
	}

	private void dropDown()
	{
		if (PlayerPlatformerController._instance != null)
		{
			PlayerPlatformerController._instance.continuedMovement = Vector2.down * 2f;
		}
	}

	private void trailertitleShowAfter3Seound()
	{
		StartCoroutine(wait3ShowTitle());
	}

	private IEnumerator wait3ShowTitle()
	{
		yield return new WaitForSeconds(3f);
		// Add title show logic here
	}

	private void waitForGround()
	{
		// Empty - handled in ProcessCommand
	}

	private IEnumerator checkPlayerOnground()
	{
		yield return new WaitUntil(() => 
			PlayerPlatformerController._instance != null && 
			PlayerPlatformerController._instance.isGround());
	}

	private IEnumerator checkPlayer()
	{
		yield return new WaitUntil(() => PlayerPlatformerController._instance != null);
	}

	private void rotateCamera()
	{
		StartCoroutine(checkCamReady());
	}

	private IEnumerator checkCamReady()
	{
		yield return new WaitForSeconds(1f);
		// Add camera rotation logic here
	}

	private void godLeave()
	{
		GameObject god = GameObject.Find("God");
		if (god != null)
		{
			god.SetActive(false);
		}
		if (godTrans != null)
		{
			godTrans.gameObject.SetActive(false);
		}
	}

	private void unlockSkill()
	{
		StartCoroutine(ballSkill());
	}

	private IEnumerator ballSkill()
	{
		yield return new WaitForSeconds(1f);
		// Add skill unlock logic here
	}

	private void activeButtonLight()
	{
		if (UIManager._instance != null)
		{
			UIManager._instance.showActiveButton(true, true);
		}
	}

	private void waitForInput()
	{
		waitingForInput = true;
	}

	private void messageBox(string message)
	{
		useMessageBox = true;
	}
}
