
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

		if (chatObj != null)
		{
			chatObj.SetActive(false);
		}

		if (autoPlay && (!blockAutoIfPlayed || !played))
		{
			callStory();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (chatObj != null && !played)
			{
				chatObj.SetActive(true);
			}

			if (autoPlay && (!blockAutoIfPlayed || !played))
			{
				callStory();
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
		if (storyList != null && storyList.Length > 0 && StoryManager._instance != null)
		{
			if (playOnceOnly && played)
				return;

			if (storyIndex < storyList.Length)
			{
				if (chatObj != null)
				{
					chatObj.SetActive(false);
				}

				StoryManager._instance.callStory(storyList[storyIndex], gameObject);
				played = true;
			}
		}
	}

	public void storyEnd()
	{
		if (!playOnceOnly)
		{
			storyIndex++;
			if (storyIndex >= storyList.Length)
			{
				storyIndex = 0;
			}
		}
	}
}