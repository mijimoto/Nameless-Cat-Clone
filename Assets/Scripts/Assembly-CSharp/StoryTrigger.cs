using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
	public string[] storyList;
	private int storyIndex;
	public bool playOnceOnly;
	public bool autoPlay;
	public bool blockAutoIfPlayed;
	private bool played;
	public GameObject chatObj;

	private void Start()
	{
		storyIndex = 0;
		played = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (autoPlay && (!blockAutoIfPlayed || !played))
			{
				callStory();
			}
			else if (chatObj != null)
			{
				chatObj.SetActive(true);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (chatObj != null)
			{
				chatObj.SetActive(false);
			}
		}
	}

	public void callStory()
	{
		if (playOnceOnly && played) return;
		if (storyList == null || storyList.Length == 0) return;

		// Trigger story/dialogue system
		if (storyIndex < storyList.Length)
		{
			string currentStory = storyList[storyIndex];
			// Show dialogue/story UI
			Debug.Log("Story: " + currentStory);
			
			storyIndex++;
			if (storyIndex >= storyList.Length)
			{
				storyIndex = 0;
				played = true;
			}
		}
	}

	public void storyEnd()
	{
		played = true;
		if (chatObj != null)
		{
			chatObj.SetActive(false);
		}
	}
}