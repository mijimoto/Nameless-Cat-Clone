using UnityEngine;

public class BrokenBlock : MonoBehaviour
{
	public enum BrokenState
	{
		idle = 0,
		breaking = 1,
		broken = 2
	}

	public BrokenState state = BrokenState.idle;
	
	private Transform spriteObj;
	
	public float breakTime = 1f;
	private float breakTimer;
	
	public float recoverTime = 3f;
	
	private BoxCollider2D boxCollider2D;
	private Animator ani;

	private void Start()
	{
		boxCollider2D = GetComponent<BoxCollider2D>();
		ani = GetComponent<Animator>();
		spriteObj = transform;
		
		breakTimer = 0f;
		state = BrokenState.idle;
	}

	private void Update()
	{
		switch (state)
		{
			case BrokenState.idle:
				// Block is solid and intact
				break;
				
			case BrokenState.breaking:
				breakTimer += Time.deltaTime;
				if (breakTimer >= breakTime)
				{
					// Transition to broken state
					state = BrokenState.broken;
					breakTimer = 0f;
					
					// Disable collider when broken
					if (boxCollider2D != null)
					{
						boxCollider2D.enabled = false;
					}
					
					// Play broken animation
					if (ani != null)
					{
						ani.SetBool("broken", true);
					}
					
					// Start recovery timer
					StartCoroutine(RecoverAfterDelay());
				}
				break;
				
			case BrokenState.broken:
				// Block is broken and passable
				break;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (state == BrokenState.idle && other.gameObject.CompareTag("Player"))
		{
			// Check if player is landing on top of the block
			PlayerPlatformerController player = other.gameObject.GetComponent<PlayerPlatformerController>();
			if (player != null)
			{
				// Check if player is above the block (landing on it)
				if (other.transform.position.y > transform.position.y + 0.5f)
				{
					TriggerBreaking();
				}
			}
		}
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		if (state == BrokenState.idle && other.gameObject.CompareTag("Player"))
		{
			// Continue breaking if player stays on the block
			PlayerPlatformerController player = other.gameObject.GetComponent<PlayerPlatformerController>();
			if (player != null && player.isGround())
			{
				// Check if player is standing on this block
				if (other.transform.position.y > transform.position.y + 0.3f)
				{
					if (state == BrokenState.idle)
					{
						TriggerBreaking();
					}
				}
			}
		}
	}
	
	private void TriggerBreaking()
	{
		if (state != BrokenState.idle) return;
		
		state = BrokenState.breaking;
		breakTimer = 0f;
		
		// Play breaking animation
		if (ani != null)
		{
			ani.SetBool("breaking", true);
			ani.SetBool("broken", false);
		}
	}
	
	private System.Collections.IEnumerator RecoverAfterDelay()
	{
		yield return new WaitForSeconds(recoverTime);
		
		// Recover the block
		state = BrokenState.idle;
		
		// Re-enable collider
		if (boxCollider2D != null)
		{
			boxCollider2D.enabled = true;
		}
		
		// Reset animations
		if (ani != null)
		{
			ani.SetBool("breaking", false);
			ani.SetBool("broken", false);
		}
	}
}