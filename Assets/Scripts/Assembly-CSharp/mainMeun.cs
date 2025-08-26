using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMeun : MonoBehaviour
{
    private bool loading;
    public Animator cover;        // Fade-out cover (black screen)
    private float timer;
    private AsyncOperation asyncLoad;
    private bool started;

    public GameObject updateText; // "Press to Start" text

    private void Awake()
    {
        // Ensure cursor visible on menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        loading = false;
        started = false;

        if (updateText != null)
            updateText.SetActive(true); // Show "Press to Start"
    }

    private void Update()
    {
        // Wait for player input to start
        if (!started && Input.anyKeyDown)
        {
            startGame();
        }

        // Optional blinking effect for "Press to Start"
        if (!started && updateText != null)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Abs(Mathf.Sin(timer * 2f));
            updateText.GetComponent<CanvasGroup>().alpha = alpha;
        }
    }

    public void startGame()
    {
        if (started) return;
        started = true;

        if (updateText != null) updateText.SetActive(false);

        if (cover != null)
            cover.SetTrigger("FadeOut"); // Cover animator triggers fade

        StartCoroutine(ingame());
    }

    private IEnumerator ingame()
    {
        yield return new WaitForSeconds(1.0f); // Wait for fade animation
        yield return LoadYourAsyncScene();
    }

    private IEnumerator LoadYourAsyncScene()
    {
        if (loading) yield break;
        loading = true;

        asyncLoad = SceneManager.LoadSceneAsync("selectList");
        asyncLoad.allowSceneActivation = false;

        // Wait until load is almost complete
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Now activate scene
        asyncLoad.allowSceneActivation = true;
    }
}
