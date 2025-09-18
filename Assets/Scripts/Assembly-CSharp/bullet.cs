using UnityEngine;

public class bullet : MonoBehaviour
{
	public float speed;
	public float destoryTime;
	private int side; // -1 for left, 1 for right
	private bool vertical;
	private float timer;

	private void Start()
	{
		timer = 0f;
		// Destroy bullet after destoryTime if it doesn't hit anything
		Destroy(gameObject, destoryTime);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		
		// Move bullet based on direction
		if (vertical)
		{
			// Move vertically (up/down)
			transform.Translate(0, side * speed * Time.deltaTime, 0);
		}
		else
		{
			// Move horizontally (left/right)
			transform.Translate(side * speed * Time.deltaTime, 0, 0);
		}
		
		// Auto-destroy after time limit
		if (timer >= destoryTime)
		{
			Destroy(gameObject);
		}
	}

	public void initialize(bool right, bool vertical = false)
	{
		this.vertical = vertical;
		
		if (vertical)
		{
			// For vertical movement: true = up, false = down
			side = right ? 1 : -1;
		}
		else
		{
			// For horizontal movement: true = right, false = left
			side = right ? 1 : -1;
		}
		
		// Flip sprite to match direction
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (sr != null && !vertical)
		{
			sr.flipX = !right; // Flip if moving left
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Hit player - cause damage
		if (other.CompareTag("Player"))
		{
			PlayerPlatformerController player = other.GetComponent<PlayerPlatformerController>();
			if (player != null && !player.Invincible)
			{
				player.die(true, "Bullet");
			}
			Destroy(gameObject);
			return;
		}
		
		// Hit platform or ground - destroy bullet
		if (other.CompareTag("platform") || other.CompareTag("Ground"))
		{
			Destroy(gameObject);
			return;
		}
		
		// Hit totem/objects - destroy bullet (bullets can't pass through totems)
		if (other.CompareTag("object"))
		{
			Destroy(gameObject);
			return;
		}
	}
}