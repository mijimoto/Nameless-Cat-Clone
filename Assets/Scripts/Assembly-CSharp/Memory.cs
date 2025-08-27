using UnityEngine;
using UnityEngine.SceneManagement;

public class Memory : MonoBehaviour
{
	public GameObject storyCallback;
	private bool endLevel;
	private MemoryStar memoryStar;

	private void Start()
	{
		memoryStar = GetComponentInChildren<MemoryStar>();
		endLevel = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			string currentScene = SceneManager.GetActiveScene().name;

			// Example: "level1-1"
			string[] parts = currentScene.Replace("level", "").Split('-');
			int chapter = int.Parse(parts[0]);
			int stage = int.Parse(parts[1]);

			// Next level name
			string nextScene = $"level{chapter}-{stage + 1}";

			Debug.Log("Loading: " + nextScene);

			if (Application.CanStreamedLevelBeLoaded(nextScene))
			{
				SceneManager.LoadScene(nextScene);
			}
			else
			{
				Debug.Log("No more levels, back to menu");
				SceneManager.LoadScene("MainMenu");
			}
		}
	}
	public void createMemoryStar()
	{
		if (memoryStar != null)
		{
			memoryStar.collectMemory();
		}
	}
}