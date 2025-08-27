using System.Collections;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	public enum AdCheckPointState
	{
		NonActive = 0,
		AdActive = 1,
		PaidActive = 2
	}

	public static CheckPoint currentCheckPoint;
	public static CheckPoint destoryCheckPoint;
	public static CheckPoint adStoreCheckPoint;
	public static bool itemCollected;

	public bool watchAD;
	public GameObject displayArrow;
	public static AdCheckPointState state;
	private bool adCheckPoint;

	public Animator animator { get; private set; }

	public static Vector2 getCheckPointPosition()
	{
		if (currentCheckPoint != null)
		{
			return currentCheckPoint.transform.position;
		}
		return Vector2.zero;
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
		if (displayArrow != null)
		{
			displayArrow.SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (watchAD && state == AdCheckPointState.NonActive)
			{
				adStoreCheckPoint = this;
				if (displayArrow != null)
				{
					displayArrow.SetActive(true);
				}
			}
			else
			{
				activeSavePoint();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (adStoreCheckPoint == this)
			{
				adStoreCheckPoint = null;
				if (displayArrow != null)
				{
					displayArrow.SetActive(false);
				}
			}
		}
	}

	public void activeSavePoint()
	{
		if (currentCheckPoint != this)
		{
			if (currentCheckPoint != null && currentCheckPoint.animator != null)
			{
				currentCheckPoint.animator.SetBool("active", false);
			}
			
			currentCheckPoint = this;
			if (animator != null)
			{
				animator.SetBool("active", true);
			}
			
			// Save game progress
			PlayerPrefs.SetFloat("CheckPointX", transform.position.x);
			PlayerPrefs.SetFloat("CheckPointY", transform.position.y);
			PlayerPrefs.Save();
		}
	}

	public void activeSavePointEnum()
	{
		StartCoroutine(activeSavePointForReward());
	}

	private IEnumerator activeSavePointForReward()
	{
		// Show ad or handle reward system
		yield return new WaitForSeconds(1f);
		state = AdCheckPointState.AdActive;
		activeSavePoint();
	}

	public static void cleanUpCheckPoint()
	{
		currentCheckPoint = null;
		destoryCheckPoint = null;
		adStoreCheckPoint = null;
		itemCollected = false;
		state = AdCheckPointState.NonActive;
	}
}
