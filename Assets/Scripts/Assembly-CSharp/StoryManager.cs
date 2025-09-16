using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private void Awake()
    {
    }
    private void Start()
    {
    }
    public static void resetStory()
    {
    }
    private void Update()
    {
    }
    public bool canLoadStory()
    {
        return false;
    }
    public void loadStory()
    {
    }
    public void callStory(string id, GameObject callback = null)
    {
    }
    public void callStoryAndWait(string id, GameObject callback = null)
    {
    }
    private List<string> PrepareCallStory(string id)
    {
        return null;
    }
    public void playEmptyStory(GameObject callback, int outerAmount = 0)
    {
    }
    public void playCustomStory(GameObject callback, string command)
    {
    }
    public void playCustomStory(GameObject callback, List<string> tmp)
    {
    }
    private IEnumerator playStory(string id, List<string> tmp)
    {
        return null;
    }
    public void skipStory()
    {
    }
    public void nextStory()
    {
    }
    private void noSpawnEffect()
    {
    }
    private void justWait()
    {
    }
    private void useOuterController()
    {
    }
    private void useOuterController2()
    {
    }
    private void useOuterController3()
    {
    }
    private void nextLevel()
    {
    }
    private void wait1Second()
    {
    }
    private void wait3Second()
    {
    }
    private IEnumerator waitTime(float time)
    {
        return null;
    }
    private void hideSkipButton()
    {
    }
    private void hideActiveButton()
    {
    }
    private void dropDown()
    {
    }
    private void trailertitleShowAfter3Seound()
    {
    }
    private IEnumerator wait3ShowTitle()
    {
        return null;
    }
    private void waitForGround()
    {
    }
    private IEnumerator checkPlayerOnground()
    {
        return null;
    }
    private IEnumerator checkPlayer()
    {
        return null;
    }
    private void rotateCamera()
    {
    }
    private IEnumerator checkCamReady()
    {
        return null;
    }
    private void godLeave()
    {
    }
    private void unlockSkill()
    {
    }
    private IEnumerator ballSkill()
    {
        return null;
    }
    private void activeButtonLight()
    {
    }
    private void waitForInput()
    {
    }
    private void messageBox(string message)
    {
    }
}