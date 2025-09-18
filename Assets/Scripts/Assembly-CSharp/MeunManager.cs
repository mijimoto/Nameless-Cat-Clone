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
    // singleton
    if (_instance == null)
    {
        _instance = this;
    }
    else if (_instance != this)
    {
        Destroy(gameObject);
        return;
    }

    if (chapterMeun != null && chapterMeun.childCount > 0)
        chapterInnter = chapterMeun.GetChild(0) as RectTransform;

    if (languagePanel != null)
    {
        languagePanel.OnButtonClick += ChangeLanguage;
    }

    // --- Auto-wire back buttons inside collection and skin rows ---
    // Find Buttons under collectionMenuTrans whose name contains "back" and wire to close collection.
    if (collectionMenuTrans != null)
    {
        Button[] colButtons = collectionMenuTrans.GetComponentsInChildren<Button>(true);
        foreach (var b in colButtons)
        {
            if (b == null) continue;
            if (b.name.ToLower().Contains("back"))
            {
                // avoid duplicate listeners if any (safer)
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(() => openCollectionMenu(false));
                break; // only wire the first matching back button
            }
        }
    }

    // Find Buttons under skinMenuTrans whose name contains "back" and wire to close skin.
    if (skinMenuTrans != null)
    {
        Button[] skinButtons = skinMenuTrans.GetComponentsInChildren<Button>(true);
        foreach (var b in skinButtons)
        {
            if (b == null) continue;
            if (b.name.ToLower().Contains("back"))
            {
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(() => openSkinMeun(false));
                break;
            }
        }
    }
    // --- end auto-wire ---

    clearWaitTime = 0f;
    quitPopupShown = false;
    // allowToControl true so first button can start movement
    allowToControl = true;
}


    private void Start()
    {
        // default to the "home" row (chapters/buttons)
        windowType = Window.achievement;

        if (versionNum != null)
            versionNum.text = Application.version;

        if (loadingCover != null)
            loadingCover.SetActive(false);

        if (cheatMenu != null)
            cheatMenu.SetActive(false);

        if (clearPanel != null)
            clearPanel.SetActive(false);

        initWindowPos();

        updateHideButton();
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;

        if (languagePanel != null)
            languagePanel.OnButtonClick -= ChangeLanguage;
    }

    private void initWindowPos()
    {
        // Find the target row for the current window
        RectTransform target = chapterMeun;
        if (windowType == Window.skin && skinMenuTrans != null) target = skinMenuTrans;
        if (windowType == Window.collection && collectionMenuTrans != null) target = collectionMenuTrans;
        if (windowType == Window.setting && settingMeun != null) target = settingMeun;
        if (windowType == Window.achievement && chapterMeun != null) target = chapterMeun;

        if (target == null)
        {
            Debug.LogWarning("[MeunManager] initWindowPos: no valid target for initial window.");
            return;
        }

        // We want the target row to be at focusY = 0. Compute delta required and instantly apply to all row rects.
        float deltaY = -GetRowAnchoredY(target);

        ApplyDeltaToRowsImmediate(deltaY);

        allowToControl = true;
    }

    private void Update()
    {
        // animate rows toward target each frame
        updateWindowPos();
        updateClearPanel();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backButton();
        }
    }

    // Helper: read anchored Y or fallback to local Y
    private float GetRowAnchoredY(RectTransform r)
    {
        if (r == null) return 0f;
        if (!Mathf.Approximately(r.anchoredPosition.y, 0f))
            return r.anchoredPosition.y;
        return r.localPosition.y;
    }

    // Immediately add delta to each row (used on init)
    private void ApplyDeltaToRowsImmediate(float deltaY)
    {
        if (chapterMeun != null)
            chapterMeun.anchoredPosition = new Vector2(chapterMeun.anchoredPosition.x, chapterMeun.anchoredPosition.y + deltaY);
        if (skinMenuTrans != null)
            skinMenuTrans.anchoredPosition = new Vector2(skinMenuTrans.anchoredPosition.x, skinMenuTrans.anchoredPosition.y + deltaY);
        if (collectionMenuTrans != null)
            collectionMenuTrans.anchoredPosition = new Vector2(collectionMenuTrans.anchoredPosition.x, collectionMenuTrans.anchoredPosition.y + deltaY);
        if (settingMeun != null)
            settingMeun.anchoredPosition = new Vector2(settingMeun.anchoredPosition.x, settingMeun.anchoredPosition.y + deltaY);
        if (buttons != null)
            buttons.anchoredPosition = new Vector2(buttons.anchoredPosition.x, buttons.anchoredPosition.y + deltaY);
        if (chapterButton != null)
            chapterButton.anchoredPosition = new Vector2(chapterButton.anchoredPosition.x, chapterButton.anchoredPosition.y + deltaY);
    }

    private void updateWindowPos()
    {
        // Decide target row
        RectTransform target = null;
        switch (windowType)
        {
            case Window.skin: target = skinMenuTrans; break;
            case Window.collection: target = collectionMenuTrans; break;
            case Window.setting: target = settingMeun; break;
            case Window.achievement: target = chapterMeun; break;
            case Window.off:
            default: target = null; break;
        }

        // If no target, do nothing
        if (target == null) return;

        // Delta required to bring target to focusY (0)
        // Using anchoredPosition if available, otherwise localPosition
        float targetAnchY = GetRowAnchoredY(target);
        float delta = -targetAnchY; // we want target to move by delta so its new anchoredY = 0

        // Movement speed (units per second) — tweak if needed
        float speed = 1500f;
        float maxDelta = speed * Time.deltaTime;

        // For each row, compute desired Y = currentY + delta, then move currentY towards desiredY with MoveTowards
        MoveRowTowards(chapterMeun, delta, maxDelta);
        MoveRowTowards(skinMenuTrans, delta, maxDelta);
        MoveRowTowards(collectionMenuTrans, delta, maxDelta);
        MoveRowTowards(settingMeun, delta, maxDelta);
        MoveRowTowards(buttons, delta, maxDelta);
        MoveRowTowards(chapterButton, delta, maxDelta);

        // If the target is close enough to focus (anchored or local), allow control again
        float newTargetAnchY = GetRowAnchoredY(target); // value after MoveRowTowards
        if (Mathf.Abs(newTargetAnchY) < 1f)
        {
            // Snap precisely to zero
            SnapRowToZero(target);
            // Ensure all rows snap exactly (to avoid tiny fractional offsets)
            float snapDelta = -GetRowAnchoredY(target);
            if (Mathf.Abs(snapDelta) > 0f)
                ApplyDeltaToRowsImmediate(snapDelta);

            allowToControl = true;
        }
        else
        {
            allowToControl = false;
        }

        // Debug when movement occurs
        if (!Mathf.Approximately(targetAnchY, newTargetAnchY))
        {
            Debug.Log($"[MeunManager] moveRows -> window:{windowType} delta:{delta:F1} targetBefore:{targetAnchY:F1} targetAfter:{newTargetAnchY:F1}");
        }
    }

    private void MoveRowTowards(RectTransform row, float delta, float maxDelta)
    {
        if (row == null) return;

        // Prefer anchoredPosition; if it's zero and likely unused, operate on localPosition
        if (!Mathf.Approximately(row.anchoredPosition.y, 0f) || Mathf.Approximately(delta, 0f))
        {
            float current = row.anchoredPosition.y;
            float desired = current + delta;
            float next = Mathf.MoveTowards(current, desired, maxDelta);
            row.anchoredPosition = new Vector2(row.anchoredPosition.x, next);
        }
        else
        {
            float current = row.localPosition.y;
            float desired = current + delta;
            float next = Mathf.MoveTowards(current, desired, maxDelta);
            row.localPosition = new Vector3(row.localPosition.x, next, row.localPosition.z);
        }
    }

    private void SnapRowToZero(RectTransform row)
    {
        if (row == null) return;
        if (!Mathf.Approximately(row.anchoredPosition.y, 0f))
            row.anchoredPosition = new Vector2(row.anchoredPosition.x, 0f);
        else
            row.localPosition = new Vector3(row.localPosition.x, 0f, row.localPosition.z);
    }

private void backButton()
{
    // If we're in Collection row, call the collection close routine so it hides its holder and
    // sets the window state back to the home row (chapter/buttons).
    if (windowType == Window.collection)
    {
        openCollectionMenu(false);
        return;
    }

    // If we're in Skin row, call the skin close routine so it hides its holder and returns home.
    if (windowType == Window.skin)
    {
        openSkinMeun(false);
        return;
    }

    // If in Setting row, close it and return home as well.
    if (windowType == Window.setting)
    {
        openSettingMeun(false);
        return;
    }

    // Already at home/achievement row -> toggle quit popup
    toggleQuitPopup();
}


    public void toggleQuitPopup()
    {
        quitPopupShown = !quitPopupShown;
        if (quitPopup != null) quitPopup.SetActive(quitPopupShown);
        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    public void quitGame()
    {
        PlayerPrefs.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private float lerpTool(float currentY, float target)
    {
        // kept for compatibility; not used by movement now
        float speed = 12f;
        return Mathf.Lerp(currentY, target, 1f - Mathf.Exp(-speed * Time.deltaTime));
    }

    public void openSkinMeun(bool b)
    {
        // allow calling even if allowToControl stale; set windowType and start move
        if (b)
        {
            windowType = Window.skin;
            if (skinMenu != null && skinMenu.holder != null) skinMenu.holder.SetActive(true);
            Debug.Log("[MeunManager] openSkinMeun(true) called -> target skin");
        }
        else
        {
            windowType = Window.achievement;
            if (skinMenu != null && skinMenu.holder != null) skinMenu.holder.SetActive(false);
            Debug.Log("[MeunManager] openSkinMeun(false) called -> back to achievement");
        }

        // start movement
        allowToControl = false;
        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    public void openSettingMeun(bool b)
    {
        if (b)
        {
            windowType = Window.setting;
            Debug.Log("[MeunManager] openSettingMeun(true) called -> target setting");
        }
        else
        {
            windowType = Window.achievement;
            Debug.Log("[MeunManager] openSettingMeun(false) called -> back to achievement");
        }

        allowToControl = false;
        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    public void openCollectionMenu(bool b)
    {
        if (b)
        {
            windowType = Window.collection;
            if (collectionMenu != null && collectionMenu.holder != null) collectionMenu.holder.SetActive(true);
            Debug.Log("[MeunManager] openCollectionMenu(true) called -> target collection");
        }
        else
        {
            windowType = Window.achievement;
            if (collectionMenu != null && collectionMenu.holder != null) collectionMenu.holder.SetActive(false);
            Debug.Log("[MeunManager] openCollectionMenu(false) called -> back to achievement");
        }

        allowToControl = false;
        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    public bool noWindow()
    {
        return windowType == Window.off;
    }

    public void callAchievementPanel()
    {
        windowType = Window.achievement;
        allowToControl = false;
        if (achievementController != null) achievementController.UpdateProgress();
        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
        Debug.Log("[MeunManager] callAchievementPanel -> focused achievement");
    }

    public void showClearPanel(bool show)
    {
        if (clearPanel == null) return;

        clearPanel.SetActive(show);

        if (show)
        {
            clearWaitTime = 3f;
            if (clearBtn != null) clearBtn.interactable = false;
        }
        else
        {
            clearWaitTime = 0f;
            if (clearBtn != null) clearBtn.interactable = true;
        }

        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    public void updateClearPanel()
    {
        if (clearPanel == null || !clearPanel.activeSelf)
            return;

        if (clearWaitTime > 0f)
        {
            clearWaitTime -= Time.deltaTime;
            if (clearTimerText != null)
            {
                int sec = Mathf.CeilToInt(Mathf.Max(0f, clearWaitTime));
                clearTimerText.UpdateValue(sec.ToString());
            }
        }

        if (clearBtn != null)
            clearBtn.interactable = (clearWaitTime <= 0f);
    }

    public void clearBtnFunction(bool confirm)
    {
        if (!confirm)
        {
            PopupText("clear_confirm");
            return;
        }

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        PopupText("clear_success");
        showClearPanel(false);

        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    public void removeAdBtn()
    {
        PopupNotAvailable();
    }

    public void showLanguagePanel(bool show)
    {
        if (languagePanel != null)
        {
            if (languagePanel.gameObject != null)
                languagePanel.gameObject.SetActive(show);
        }

        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    public void ChangeLanguage(string lan)
    {
        DataLoader.changeLan(lan, true);

        if (languagePanel != null && languagePanel.gameObject != null)
            languagePanel.gameObject.SetActive(false);
    }

    public void toggleHideButton()
    {
        int current = PlayerPrefs.GetInt("HideButton", 0);
        int next = current == 0 ? 1 : 0;
        PlayerPrefs.SetInt("HideButton", next);
        PlayerPrefs.Save();
        updateHideButton();
        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    private void updateHideButton()
    {
        int current = PlayerPrefs.GetInt("HideButton", 0);
        bool hidden = (current == 1);

        if (hideButtonImage != null && hideButtonSprites != null && hideButtonSprites.Length >= 2)
            hideButtonImage.sprite = hidden ? hideButtonSprites[1] : hideButtonSprites[0];

        if (hideButtonText != null && hideButtonTextKeys != null && hideButtonTextKeys.Length >= 2)
        {
            string key = hidden ? hideButtonTextKeys[1] : hideButtonTextKeys[0];
            hideButtonText.SetKey(key);
        }

        if (hideButtonUnmask != null)
            hideButtonUnmask.enabled = !hidden;
    }

    public void privacyPolicy()
    {
        Application.OpenURL("https://example.com/privacy");
        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    public void PopupThanksForSupport()
    {
        PopupText("thanks_support");
    }

    public void PopupNotAvailable()
    {
        PopupText("not_available");
    }

    public void PopupRestoreIAPSuccess()
    {
        PopupText("restore_success");
    }

    public void PopupRestoreIAPFail()
    {
        PopupText("restore_fail");
    }

    public void ShowLoadingCover()
    {
        if (loadingCover != null)
            loadingCover.SetActive(true);
    }

    public void HideLoadingCover()
    {
        if (loadingCover != null)
            loadingCover.SetActive(false);
    }

    public void ToggleCheatMenu()
    {
        if (cheatMenu == null) return;

        bool next = !cheatMenu.activeSelf;
        cheatMenu.SetActive(next);

        if (cheatButton != null)
            cheatButton.SetActive(!next);

        if (SoundManager._instance != null) SoundManager._instance.uiSound("click");
    }

    private void PopupText(string msgKey)
    {
        if (popupPrefab == null) return;

        var go = Instantiate(popupPrefab.gameObject, transform);
        var popup = go.GetComponent<PopupMessage>();
        if (popup != null)
            popup.Init(msgKey);
    }
}