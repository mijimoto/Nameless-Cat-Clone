using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public enum CoverAni
	{
		fadeIn = 0,
		fadeOut = 1,
		flash = 2
	}

	public enum ButtonMode
	{
		show = 0,
		hide = 1,
		transparent = 2
	}

	public static UIManager _instance;

	public RectTransform mainCanvas;
	public CanvasGroup buttons;
	public GameObject activeButton;
	public GameObject activeButtonLight;
	public GameObject topPanel;
	private Image[] topPanelImages;
	public GameObject pauseButton;
	public Animator whiteLight;
	public Animator backLight;
	public Animator movieBar;
	public RawImage whiteLightImage;
	public RawImage backLightImage;
	public RawImage movieBarTopImage;
	public RawImage movieBarDownImage;
	public RawImage backCover;
	public GameObject meunUI;
	public Image musicButton;
	public Image soundButton;
	public Image boxImage;
	public RectTransform boxRect;
	public RectTransform boxEffectRect;
	public Animator boxEffectAni;
	public GameObject skipButton;
	public CheckAdPanel checkAdPanel;
	public Animator GameLevel;
	public TextMeshProUGUI GameLevelText;
	public TextMeshProUGUI GameLevelTextFull;
	public GameObject skipInfo;
	public GameObject buttonInfo;
	public Image collectionImage;
	public Image collectionArrow;
	public Image collectionBackground;
	private bool meun;
	private bool playBoxEffect;
	private float boxMoveSpeed;
	public Image buttonRemoveAd;
	public Image buttonLeft;
	public Image buttonRight;
	public Image buttonJump;
	public Image buttonActive;
	public Sprite[] musicSprite;
	public Sprite[] soundSprite;
	public Sprite[] removeAdSprite;
	public Sprite[] boxSprite;
	public Sprite[] buttonLeftSprite;
	public Sprite[] buttonRightSprite;
	public Sprite[] buttonJumpSprite;
	public Sprite[] buttonActiveSprite;
	public PopupMessage popupPrefab;
	public GameObject flyButton;
	public GameObject downButton;
	private bool displayButton;
	private ButtonMode buttonMode;

	private void Awake()
	{
		_instance = this;
		if (topPanel != null)
		{
			topPanelImages = topPanel.GetComponentsInChildren<Image>();
		}
		
		buttonMode = (ButtonMode)PlayerPrefs.GetInt("ButtonMode", 0);
		displayButton = true;
		boxMoveSpeed = 200f;
	}

	private void Update()
	{
		if (playBoxEffect && boxEffectRect != null)
		{
			updateBoxEffect();
		}

		// Handle input for menu toggle
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			toggleMeun();
		}
	}

	public void showTopButton(bool b)
	{
		if (pauseButton != null)
		{
			pauseButton.SetActive(b);
		}
	}

	public void showButton(bool b)
	{
		displayButton = b;
		updateButton();
	}

	public void toggleButtonMode()
	{
		int currentMode = (int)buttonMode;
		currentMode = (currentMode + 1) % 3;
		setButtonMode((ButtonMode)currentMode);
	}

	public void setButtonMode(int mode)
	{
		setButtonMode((ButtonMode)mode);
	}

	public void setButtonMode(ButtonMode mode, bool save = true)
	{
		buttonMode = mode;
		if (save)
		{
			PlayerPrefs.SetInt("ButtonMode", (int)mode);
			PlayerPrefs.Save();
		}
		updateButton();
	}

	private void updateButton()
	{
		if (buttons == null) return;

		switch (buttonMode)
		{
			case ButtonMode.show:
				buttons.alpha = displayButton ? 1f : 0f;
				buttons.interactable = displayButton;
				buttons.blocksRaycasts = displayButton;
				break;
			case ButtonMode.hide:
				buttons.alpha = 0f;
				buttons.interactable = false;
				buttons.blocksRaycasts = false;
				break;
			case ButtonMode.transparent:
				buttons.alpha = displayButton ? 0.5f : 0f;
				buttons.interactable = displayButton;
				buttons.blocksRaycasts = displayButton;
				break;
		}
	}

	public void showTopPanel(bool b)
	{
		if (topPanel != null)
		{
			topPanel.SetActive(b);
		}
	}

	public bool topPanelActive()
	{
		return topPanel != null && topPanel.activeInHierarchy;
	}

public void showActiveButton(bool b, bool light = false)
{
	if (activeButton != null)
	{
		activeButton.SetActive(b);
	}
	if (activeButtonLight != null)
	{
		activeButtonLight.SetActive(b && light);
	}
}

	public void toggleMeun()
	{
		openMeun(!meun);
	}

	public void openMeun(bool b)
	{
		meun = b;
		if (meunUI != null)
		{
			meunUI.SetActive(b);
		}
		
		// Pause/unpause game
		Time.timeScale = b ? 0f : 1f;
		
		if (PlayerPlatformerController._instance != null)
		{
			PlayerPlatformerController._instance.gamePaused = b;
		}
	}

	public void tryToOpenMeun()
	{
		if (!meun)
		{
			openMeun(true);
		}
	}

	public void backButtonInGame()
	{
		if (meun)
		{
			openMeun(false);
		}
		else
		{
			tryToOpenMeun();
		}
	}

	public void updateMusic(bool b)
	{
		if (musicButton != null && musicSprite != null && musicSprite.Length >= 2)
		{
			musicButton.sprite = musicSprite[b ? 0 : 1];
		}
		PlayerPrefs.SetInt("Music", b ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void updateSound(bool b)
	{
		if (soundButton != null && soundSprite != null && soundSprite.Length >= 2)
		{
			soundButton.sprite = soundSprite[b ? 0 : 1];
		}
		PlayerPrefs.SetInt("Sound", b ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void whiteLightActive(CoverAni type)
	{
		if (whiteLight != null)
		{
			switch (type)
			{
				case CoverAni.fadeIn:
					whiteLight.SetTrigger("fadeIn");
					break;
				case CoverAni.fadeOut:
					whiteLight.SetTrigger("fadeOut");
					break;
				case CoverAni.flash:
					whiteLight.SetTrigger("flash");
					break;
			}
		}
	}

	public void blackLightActive(CoverAni type)
	{
		if (backLight != null)
		{
			switch (type)
			{
				case CoverAni.fadeIn:
					backLight.SetTrigger("fadeIn");
					break;
				case CoverAni.fadeOut:
					backLight.SetTrigger("fadeOut");
					break;
				case CoverAni.flash:
					backLight.SetTrigger("flash");
					break;
			}
		}
	}

	public void movieBarActive(CoverAni type)
	{
		if (movieBar != null)
		{
			switch (type)
			{
				case CoverAni.fadeIn:
					movieBar.SetTrigger("fadeIn");
					break;
				case CoverAni.fadeOut:
					movieBar.SetTrigger("fadeOut");
					break;
				case CoverAni.flash:
					movieBar.SetTrigger("flash");
					break;
			}
		}
	}

	public void getBox(bool b)
	{
		if (boxImage != null && boxSprite != null && boxSprite.Length >= 2)
		{
			boxImage.sprite = boxSprite[b ? 1 : 0];
		}
	}

	public void getBoxEffect(Vector3 pos)
	{
		if (boxEffectRect != null)
		{
			boxEffectRect.position = pos;
			playBoxEffect = true;
			if (boxEffectAni != null)
			{
				boxEffectAni.SetTrigger("play");
			}
		}
	}

	public void updateBoxEffect()
	{
		if (boxEffectRect != null && boxRect != null)
		{
			Vector3 direction = (boxRect.position - boxEffectRect.position).normalized;
			boxEffectRect.position += direction * boxMoveSpeed * Time.deltaTime;
			
			if (Vector3.Distance(boxEffectRect.position, boxRect.position) < 10f)
			{
				playBoxEffect = false;
				getBox(true);
			}
		}
	}

	public void showSkipBtn(bool b)
	{
		if (skipButton != null)
		{
			skipButton.SetActive(b);
		}
	}

	public void showCheckPointAdPanel(bool b)
	{
		if (checkAdPanel != null)
		{
			checkAdPanel.ShowPanel(b);
		}
	}

	public void playGameLevel(string levelText)
	{
		if (GameLevelText != null)
		{
			GameLevelText.text = levelText;
		}
		if (GameLevel != null)
		{
			GameLevel.SetTrigger("play");
		}
	}

	public void playGameLevelFull()
	{
		if (GameLevelTextFull != null && GameLevelText != null)
		{
			GameLevelTextFull.text = GameLevelText.text;
		}
		if (GameLevel != null)
		{
			GameLevel.SetTrigger("playFull");
		}
	}

	public void storyEnd()
	{
		showSkipBtn(false);
		showButton(true);
	}

	public void showCollection(bool show, Sprite photo = null)
	{
		if (collectionImage != null)
		{
			collectionImage.gameObject.SetActive(show);
			if (show && photo != null)
			{
				collectionImage.sprite = photo;
			}
		}
		if (collectionBackground != null)
		{
			collectionBackground.gameObject.SetActive(show);
		}
	}

	public void showCollectionArrow(bool show)
	{
		if (collectionArrow != null)
		{
			collectionArrow.gameObject.SetActive(show);
		}
	}

	public void removeAdBtn()
	{
		// Handle remove ads purchase
		PlayerPrefs.SetInt("RemoveAds", 1);
		PlayerPrefs.Save();
		UpdateButtonSpriteRemoveAd();
		PopupThanksForSupport();
	}

	private void UpdateButtonSpriteRemoveAd()
	{
		if (buttonRemoveAd != null && removeAdSprite != null && removeAdSprite.Length >= 2)
		{
			bool adRemoved = PlayerPrefs.GetInt("RemoveAds", 0) == 1;
			buttonRemoveAd.sprite = removeAdSprite[adRemoved ? 1 : 0];
		}
	}

	public void SetButtonSpriteLeft(bool down)
	{
		if (buttonLeft != null && buttonLeftSprite != null && buttonLeftSprite.Length >= 2)
		{
			buttonLeft.sprite = buttonLeftSprite[down ? 1 : 0];
		}
	}

	public void SetButtonSpriteRight(bool down)
	{
		if (buttonRight != null && buttonRightSprite != null && buttonRightSprite.Length >= 2)
		{
			buttonRight.sprite = buttonRightSprite[down ? 1 : 0];
		}
	}

	public void SetButtonSpriteJump(bool down)
	{
		if (buttonJump != null && buttonJumpSprite != null && buttonJumpSprite.Length >= 2)
		{
			buttonJump.sprite = buttonJumpSprite[down ? 1 : 0];
		}
	}

	public void SetButtonSpriteActive(bool down)
	{
		if (buttonActive != null && buttonActiveSprite != null && buttonActiveSprite.Length >= 2)
		{
			buttonActive.sprite = buttonActiveSprite[down ? 1 : 0];
		}
	}

	public void PopupThanksForSupport()
	{
		PopupText("thanks_for_support");
	}

	public void PopupNotAvailable()
	{
		PopupText("not_available");
	}

	private void PopupText(string msgKey)
	{
		if (popupPrefab != null)
		{
			PopupMessage popup = Instantiate(popupPrefab, mainCanvas);
			popup.Init(msgKey);
		}
	}
}
