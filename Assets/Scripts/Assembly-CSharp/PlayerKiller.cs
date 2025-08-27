using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
	public string killType;
	public bool destroyBlock;
	public bool invest;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			var player = other.GetComponent<PlayerPlatformerController>();
			if (player != null && !player.Invincible)
			{
				player.die(true, killType);
			}
		}
		else if (destroyBlock && other.CompareTag("Block"))
		{
			Destroy(other.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		// Handle exit logic if needed
	}
}