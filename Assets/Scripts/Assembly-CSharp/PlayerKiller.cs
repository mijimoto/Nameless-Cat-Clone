using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
	public string killType;

	public bool destroyBlock;

	public bool invest;

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	private void OnTriggerExit2D(Collider2D other)
	{
	}
}
