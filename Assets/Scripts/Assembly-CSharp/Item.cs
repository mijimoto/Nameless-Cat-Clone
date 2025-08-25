using UnityEngine;

public class Item : MonoBehaviour
{
	public enum FishState
	{
		NotGet = 0,
		Got = 1,
		FirstGet = 2,
		GetAgain = 3
	}

	public static bool itemCollected;

	public static bool firstCollect;

	public bool hideOnStart;

	public bool noResetItem;

	private void Start()
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	private void destoryObj()
	{
	}

	public static FishState GetFishState()
	{
		return default(FishState);
	}
}
