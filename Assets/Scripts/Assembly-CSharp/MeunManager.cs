using Coffee.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeunManager : MonoBehaviour
{
	public enum Window
	{
		off = 0,
		skin = 1,
		collection = 2,
		achievement = 3,
		setting = 4
	}

	public static MeunManager _instance;

	public RectTransform mainCanvas;

	public Transform bg;

	public RectTransform buttons;

	public RectTransform chapterButton;

	public RectTransform chapterMeun;

	private RectTransform chapterInnter;

	private int chapterNum;

	public RectTransform skinMenuTrans;

	public RectTransform collectionMenuTrans;

	public RectTransform settingMeun;

	public GameObject quitPopup;

	public bool allowToControl;

	private Window windowType;

	private float windowHeight;

	public LanguagePanelController languagePanel;

	public SkinMenu skinMenu;

	public CollectionMenu collectionMenu;

	public AchievementController achievementController;

	public GameObject clearPanel;

	public Button clearBtn;

	public LocalizationText clearTimerText;

	private float clearWaitTime;

	public PopupMessage popupPrefab;

	private bool quitPopupShown;

	[SerializeField]
	private Image hideButtonImage;

	[SerializeField]
	private Sprite[] hideButtonSprites;

	[SerializeField]
	private LocalizationText hideButtonText;

	[SerializeField]
	private string[] hideButtonTextKeys;

	[SerializeField]
	private Unmask hideButtonUnmask;

	[SerializeField]
	private TextMeshProUGUI versionNum;

	[SerializeField]
	private GameObject loadingCover;

	[SerializeField]
	private GameObject cheatButton;

	[SerializeField]
	private GameObject cheatMenu;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void initWindowPos()
	{
	}

	private void Update()
	{
	}

	private void updateWindowPos()
	{
	}

	private void backButton()
	{
	}

	public void toggleQuitPopup()
	{
	}

	public void quitGame()
	{
	}

	private float lerpTool(float currentY, float target)
	{
		return 0f;
	}

	public void openSkinMeun(bool b)
	{
	}

	public void openSettingMeun(bool b)
	{
	}

	public void openCollectionMenu(bool b)
	{
	}

	public bool noWindow()
	{
		return false;
	}

	public void callAchievementPanel()
	{
	}

	public void showClearPanel(bool show)
	{
	}

	public void updateClearPanel()
	{
	}

	public void clearBtnFunction(bool confirm)
	{
	}

	public void removeAdBtn()
	{
	}

	public void showLanguagePanel(bool show)
	{
	}

	public void ChangeLanguage(string lan)
	{
	}

	public void toggleHideButton()
	{
	}

	private void updateHideButton()
	{
	}

	public void privacyPolicy()
	{
	}

	public void PopupThanksForSupport()
	{
	}

	public void PopupNotAvailable()
	{
	}

	public void PopupRestoreIAPSuccess()
	{
	}

	public void PopupRestoreIAPFail()
	{
	}

	public void ShowLoadingCover()
	{
	}

	public void HideLoadingCover()
	{
	}

	public void ToggleCheatMenu()
	{
	}

	private void PopupText(string msgKey)
	{
	}
}
