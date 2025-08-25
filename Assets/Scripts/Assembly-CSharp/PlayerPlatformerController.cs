using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
	public static PlayerPlatformerController _instance;

	public GameObject deadEffect;

	public GameObject spawnEffect;

	public JumpSmoke jumpSmoke;

	private bool isLeftDown;

	private bool isRightDown;

	private bool jumpDown;

	private float jumpTime;

	private const int basePlayAdDeadTime = 4;

	private static int playAdDeadTime;

	private const int deadTimePerIapItem = 2;

	public static int deadCount;

	public static int totalDead;

	private float dead;

	public bool isDead;

	public float deadHeight;

	private float steeringValue;

	private float steeringVelocity;

	public float maxSpeed;

	public float jumpTakeOffSpeed;

	public float pushForce;

	public float attachRange;

	public StoryTrigger storyZone;

	public PressurePlate pressZone;

	public CheckPoint checkPointAd;

	private SpriteRenderer spriteRenderer;

	private Animator animator;

	private BoxCollider2D objCollider;

	public Transform lockArrow;

	public bool allowControl;

	private bool downKeyDown;

	private bool lastJumpKeyDown;

	private bool playedJumpSound;

	public bool invincible;

	private static bool debugInvincible;

	private float origanalGra;

	private float origanalGraRb2d;

	public bool shipMode;

	public Vector2 continuedMovement;

	public bool gamePaused;

	public SpriteRenderer SpriteRenderer => spriteRenderer;

	public BoxCollider2D Collider => objCollider;

	public bool Invincible => invincible || debugInvincible;


	private void OnDrawGizmosSelected()
	{
		// Draw attach range visualization in editor
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, attachRange);

		// Draw deadHeight line
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(transform.position.x - 2f, deadHeight, 0),
					   new Vector3(transform.position.x + 2f, deadHeight, 0));
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}

		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		objCollider = GetComponent<BoxCollider2D>();
	}


	private void Start()
	{
		// Store original gravity values for ship mode
		origanalGra = gravityModifier;
		origanalGraRb2d = rb2d.gravityScale;

		// Initialize static variables
		if (playAdDeadTime == 0)
			playAdDeadTime = basePlayAdDeadTime;

		// Create spawn effect on start
		createSpawnEffect();
	}
	public void SwitchFly()
	{
		shipMode = !shipMode;

		if (shipMode)
		{
			// Enter ship/flying mode
			gravityModifier = 0f;
			rb2d.gravityScale = 0f;
			rb2d.linearDamping = 2f; // Add drag for smoother flying
		}
		else
		{
			// Return to normal mode
			gravityModifier = origanalGra;
			rb2d.gravityScale = origanalGraRb2d;
			rb2d.linearDamping = 0f;
		}
	}


	protected override void Update()
	{
		if (gamePaused) return;

		base.Update();

		if (!allowControl) return;

		UpdateMove();
		UpdateDie();
		UpdateBlock();

		// Handle jump timing
		if (jumpDown && grounded && !lastJumpKeyDown)
		{
			SetJump();
		}

		lastJumpKeyDown = jumpDown;
	}

	private void UpdateMove()
	{
		// Update steering for smooth movement
		float targetSteering = controlDirection();
		steeringValue = Mathf.SmoothDamp(steeringValue, targetSteering, ref steeringVelocity, 0.1f);

		// Update animator parameters
		if (animator != null)
		{
			animator.SetBool("grounded", grounded);
			animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
			animator.SetFloat("velocityY", velocity.y);

			// Handle sprite flipping
			if (velocity.x > 0.01f)
				spriteRenderer.flipX = false;
			else if (velocity.x < -0.01f)
				spriteRenderer.flipX = true;
		}
	}
	private void UpdateDie()
	{
		// Check if player fell below death height
		if (transform.position.y < deadHeight && !isDead && !Invincible)
		{
			die(true, "Fall");
		}

		// Handle death timer for ads
		if (isDead)
		{
			dead += Time.deltaTime;
			if (dead >= playAdDeadTime && deadCount > 0)
			{
				// Show ad logic would go here
				deadCount--;
				playAdDeadTime = basePlayAdDeadTime;
			}
		}
	}


	private void UpdateBlock()
	{
		// Handle story triggers and pressure plates
		if (storyZone != null && Vector2.Distance(transform.position, storyZone.transform.position) < 1f)
		{
			// Story zone interaction logic
		}

		if (pressZone != null && Vector2.Distance(transform.position, pressZone.transform.position) < 1f)
		{
			// Pressure plate logic
		}
	}

	public void deselectArrow()
	{
		if (lockArrow != null)
		{
			lockArrow.gameObject.SetActive(false);
		}
	}

	protected override void FixedUpdate()
	{
		if (gamePaused) return;

		base.FixedUpdate();

		// Apply continued movement (for moving platforms, etc.)
		if (continuedMovement != Vector2.zero)
		{
			rb2d.MovePosition(rb2d.position + continuedMovement * Time.fixedDeltaTime);
		}
	}

	public void activeAttach()
	{
		// Find nearby attachable objects
		Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, attachRange);

		foreach (var obj in nearbyObjects)
		{
			// Check for attachable/possessable objects
			if (obj.CompareTag("Attachable") || obj.CompareTag("Possessable"))
			{
				// Teleport to object position
				transform.position = obj.transform.position;

				// Hide player sprite and show attach effect
				Hide(true);

				// Play attach sound/effect
				if (lockArrow != null)
				{
					lockArrow.position = obj.transform.position;
					lockArrow.gameObject.SetActive(true);
				}

				break;
			}
		}
	}

	public int controlDirection()
	{
		if (isLeftDown && !isRightDown) return -1;
		if (isRightDown && !isLeftDown) return 1;
		return 0;
	}

	public int ControlDirectionVertical()
	{
		if (jumpDown) return 1;
		if (downKeyDown) return -1;
		return 0;
	}
	protected override void ComputeVelocity()
	{
		if (!allowControl || isDead || gamePaused)
		{
			targetVelocity = Vector2.zero;
			return;
		}

		Vector2 move = Vector2.zero;

		if (shipMode)
		{
			// Flying/ship mode movement
			move.x = controlDirection() * maxSpeed;
			move.y = ControlDirectionVertical() * maxSpeed * 0.7f; // Slower vertical movement
			targetVelocity = move;
		}
		else
		{
			// Normal platformer movement
			move.x = steeringValue * maxSpeed;

			if (jumpDown && grounded && jumpTime <= 0f)
			{
				velocity.y = jumpTakeOffSpeed;
				jumpTime = 0.2f;
				spawnJumpSmoke();

				if (!playedJumpSound)
				{
					// Play jump sound
					playedJumpSound = true;
				}
			}
			else if (!jumpDown)
			{
				playedJumpSound = false;
			}

			if (jumpTime > 0f)
				jumpTime -= Time.deltaTime;

			targetVelocity = move;
		}
	}


	private void OnCollisionStay2D(Collision2D c)
	{
		// Handle pushing objects
		if (c.gameObject.CompareTag("Pushable"))
		{
			Rigidbody2D pushRb = c.gameObject.GetComponent<Rigidbody2D>();
			if (pushRb != null)
			{
				Vector2 pushDirection = c.transform.position - transform.position;
				pushDirection.Normalize();
				pushRb.AddForce(pushDirection * pushForce);
			}
		}
	}

	public override void setTargetVelocity(Vector2 v2)
	{
		targetVelocity = v2;
	}


	public void createSpawnEffect()
	{
		if (spawnEffect != null)
		{
			GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);
			Destroy(effect, 2f);
		}
	}
	public void Hide(bool b)
	{
		spriteRenderer.enabled = !b;
		objCollider.enabled = !b;
	}

	public void OnLeftDown(bool down)
	{
		isLeftDown = down;
	}

	public void OnRightDown(bool down)
	{
		isRightDown = down;
	}

	public void jump(bool down)
	{
		jumpDown = down;
	}

	public void SetJump()
	{
		if (grounded && !isDead && allowControl)
		{
			velocity.y = jumpTakeOffSpeed;
			jumpTime = 0.2f;
			spawnJumpSmoke();
		}
	}

	public void down(bool down)
	{
		downKeyDown = down;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Handle collectibles
		if (other.CompareTag("Collectible"))
		{
			// Collect item logic
			Destroy(other.gameObject);
		}

		// Handle checkpoints
		if (other.CompareTag("CheckPoint"))
		{
			checkPointAd = other.GetComponent<CheckPoint>();
		}

		// Handle story triggers
		if (other.CompareTag("StoryTrigger"))
		{
			storyZone = other.GetComponent<StoryTrigger>();
		}

		// Handle death triggers
		if (other.CompareTag("DeathZone") && !Invincible)
		{
			die(true, other.name);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		// Handle enemy collisions
		if (other.gameObject.CompareTag("Enemy") && !Invincible)
		{
			die(true, "Enemy");
		}

		// Handle hazards
		if (other.gameObject.CompareTag("Hazard") && !Invincible)
		{
			die(true, "Hazard");
		}

		// Play landing sound when hitting ground
		if (grounded && velocity.y < -2f)
		{
			playDropSound();
		}
	}

	public void die(bool b, string killer = "Unknown")
	{
		if (isDead || Invincible) return;

		isDead = b;

		if (b)
		{
			deadCount++;
			totalDead++;
			dead = 0f;

			// Create death effect
			if (deadEffect != null)
			{
				GameObject effect = Instantiate(deadEffect, transform.position, Quaternion.identity);
				Destroy(effect, 3f);
			}

			// Hide player
			Hide(true);

			// Stop movement
			velocity = Vector2.zero;
			allowControl = false;

			// Reset to checkpoint after delay
			Invoke(nameof(RespawnAtCheckpoint), 1f);
		}
	}
	//addition
	private void RespawnAtCheckpoint()
	{
		if (checkPointAd != null)
		{
			transform.position = checkPointAd.transform.position;
		}

		isDead = false;
		allowControl = true;
		Hide(false);
		createSpawnEffect();
		resetKey();
	}
	public static void updatePlayAdDeadTimeByIapItem(int iapItem)
	{
		playAdDeadTime = basePlayAdDeadTime + (iapItem * deadTimePerIapItem);
	}

	public void playMoveSound()
	{
		// Play movement sound effect
		AudioSource audio = GetComponent<AudioSource>();
		if (audio != null && audio.clip != null)
		{
			audio.pitch = Random.Range(0.9f, 1.1f);
			audio.Play();
		}
	}

	public void playDropSound()
	{
	}

	public void spawnJumpSmoke()
	{
	}

	public void resetKey()
	{
		isLeftDown = false;
		isRightDown = false;
		jumpDown = false;
		downKeyDown = false;
	}
}
