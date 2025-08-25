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
	}

	private void Update()
	{
	}

	public void showTopButton(bool b)
	{
	}

	public void showButton(bool b)
	{
	}

	public void toggleButtonMode()
	{
	}

	public void setButtonMode(int mode)
	{
	}

	public void setButtonMode(ButtonMode mode, bool save = true)
	{
	}

	private void updateButton()
	{
	}

	public void showTopPanel(bool b)
	{
	}

	public bool topPanelActive()
	{
		return false;
	}

	public void showActiveButton(bool b, bool light = false)
	{
	}

	public void toggleMeun()
	{
	}

	public void openMeun(bool b)
	{
	}

	public void tryToOpenMeun()
	{
	}

	public void backButtonInGame()
	{
	}

	public void updateMusic(bool b)
	{
	}

	public void updateSound(bool b)
	{
	}

	public void whiteLightActive(CoverAni type)
	{
	}

	public void blackLightActive(CoverAni type)
	{
	}

	public void movieBarActive(CoverAni type)
	{
	}

	public void getBox(bool b)
	{
	}

	public void getBoxEffect(Vector3 pos)
	{
	}

	public void updateBoxEffect()
	{
	}

	public void showSkipBtn(bool b)
	{
	}

	public void showCheckPointAdPanel(bool b)
	{
	}

	public void playGameLevel(string levelText)
	{
	}

	public void playGameLevelFull()
	{
	}

	public void storyEnd()
	{
	}

	public void showCollection(bool show, Sprite photo = null)
	{
	}

	public void showCollectionArrow(bool show)
	{
	}

	public void removeAdBtn()
	{
	}

	private void UpdateButtonSpriteRemoveAd()
	{
	}

	public void SetButtonSpriteLeft(bool down)
	{
	}

	public void SetButtonSpriteRight(bool down)
	{
	}

	public void SetButtonSpriteJump(bool down)
	{
	}

	public void SetButtonSpriteActive(bool down)
	{
	}

	public void PopupThanksForSupport()
	{
	}

	public void PopupNotAvailable()
	{
	}

	private void PopupText(string msgKey)
	{
	}
}
