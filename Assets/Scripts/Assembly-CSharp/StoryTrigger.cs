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
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	private void OnTriggerExit2D(Collider2D other)
	{
	}

	public void callStory()
	{
	}

	public void storyEnd()
	{
	}
}
