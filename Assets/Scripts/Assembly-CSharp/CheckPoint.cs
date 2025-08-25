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
		return default(Vector2);
	}

	private void Start()
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	private void OnTriggerExit2D(Collider2D other)
	{
	}

	public void activeSavePoint()
	{
	}

	public void activeSavePointEnum()
	{
	}

	private IEnumerator activeSavePointForReward()
	{
		return null;
	}

	public static void cleanUpCheckPoint()
	{
	}
}
