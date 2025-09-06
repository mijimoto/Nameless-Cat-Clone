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
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, attachRange);
	}

	private void Awake()
	{
		_instance = this;
		Debug.Log("PlayerPlatformerController initialized: " + _instance);

		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		objCollider = GetComponent<BoxCollider2D>();

		// Make sure rb2d is assigned
		rb2d = GetComponent<Rigidbody2D>();
		origanalGra = gravityModifier;
		origanalGraRb2d = rb2d.gravityScale; // safe now

		playAdDeadTime = basePlayAdDeadTime;
	}

	private void Start()
	{
		allowControl = true;
		createSpawnEffect();

		// Use a coroutine to check and load story after a small delay
		StartCoroutine(CheckAndLoadStoryAfterStart());
	}

	private System.Collections.IEnumerator CheckAndLoadStoryAfterStart()
	{
		// Wait a frame to ensure everything is initialized
		yield return null;
		
		// Check if story should be loaded at level start
		if (StoryManager._instance != null && StoryManager._instance.canLoadStory())
		{
			allowControl = false;
			StoryManager._instance.loadStory();
		}
	}

	public void SwitchFly()
	{
		shipMode = !shipMode;
		if (shipMode)
		{
			gravityModifier = 0f;
			rb2d.gravityScale = 0f;
		}
		else
		{
			gravityModifier = origanalGra;
			rb2d.gravityScale = origanalGraRb2d;
		}
	}

	protected override void Update()
	{
		base.Update();

		if (gamePaused) return;

		if (isDead)
		{
			UpdateDie();
		}
		else if (!allowControl)
		{
			UpdateBlock();
		}
		else
		{
			UpdateMove();
		}
	}

	private bool leftInput;
	private bool rightInput;
	private bool jumpInput;
	private bool downInput;
	
	private void UpdateMove()
	{
		// Keyboard input
		bool keyboardLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
		bool keyboardRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
		bool keyboardJump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow);
		bool keyboardDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

		// Combine with UI buttons
		leftInput = isLeftDown || keyboardLeft;
		rightInput = isRightDown || keyboardRight;
		jumpInput = jumpDown || keyboardJump;
		downInput = downKeyDown || keyboardDown;

		if (animator != null)
		{
			animator.SetBool("grounded", grounded);
			animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
		}

		// Jump detection
		if (jumpInput && !lastJumpKeyDown && grounded)
			SetJump();

		// Landing effect: trigger only on transition from air -> ground
		if (grounded && !lastJumpKeyDown)
		{
			spawnJumpSmoke();
			playDropSound();
		}

		lastJumpKeyDown = jumpInput;

		// Death by falling
		if (transform.position.y < deadHeight)
			die(true, "Fall");
	}

	private void UpdateDie()
	{
		dead += Time.deltaTime;
		if (dead >= 2f) // 2 second death timer
		{
			// Respawn at checkpoint
			Vector2 spawnPos = CheckPoint.getCheckPointPosition();
			if (spawnPos != Vector2.zero)
			{
				transform.position = spawnPos;
			}

			isDead = false;
			dead = 0f;
			allowControl = true;
			createSpawnEffect();
		}
	}

	private void UpdateBlock()
	{
		// Player is blocked from control - gradually stop movement
		velocity = Vector2.Lerp(velocity, Vector2.zero, Time.deltaTime * 5f);
		
		// Reset input states while blocked to prevent stuck buttons
		isLeftDown = false;
		isRightDown = false;
		jumpDown = false;
		downKeyDown = false;
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
		base.FixedUpdate();
	}

	public void activeAttach()
	{
		// Handle attachment mechanics
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attachRange);
		foreach (Collider2D col in colliders)
		{
			if (col.CompareTag("Attachable"))
			{
				// Attach to object
				break;
			}
		}
	}

	public int controlDirection()
	{
		bool leftInput = isLeftDown || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
		bool rightInput = isRightDown || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

		if (leftInput && !rightInput) return -1;
		if (rightInput && !leftInput) return 1;
		return 0;
	}

	public int ControlDirectionVertical()
	{
		if (downKeyDown) return -1;
		if (jumpDown) return 1;
		return 0;
	}

	protected override void ComputeVelocity()
	{
		Vector2 move = Vector2.zero;

		if (shipMode)
		{
			move.x = (leftInput ? -1 : 0) + (rightInput ? 1 : 0);
			move.y = (downInput ? -1 : 0) + (jumpInput ? 1 : 0);
			move = move.normalized * maxSpeed;
		}
		else
		{
			move.x = (leftInput ? -1 : 0) + (rightInput ? 1 : 0);
		}

		if (continuedMovement != Vector2.zero)
		{
			move += continuedMovement;
			continuedMovement = Vector2.Lerp(continuedMovement, Vector2.zero, Time.deltaTime * 2f);
		}

		// Flip sprite
		if (spriteRenderer != null)
		{
			spriteRenderer.flipX = move.x < -0.01f;
		}

		targetVelocity = move * maxSpeed;
	}

	private void OnCollisionStay2D(Collision2D c)
	{
		// Handle collision with moving platforms
		if (grounded && c.gameObject.CompareTag("platform"))
		{
			Standable standable = c.gameObject.GetComponent<Standable>();
			if (standable != null && standable.moveGoods)
			{
				// Check if it's a platform effector or jumpOver platform
				PlatformEffector2D platformEffector = c.gameObject.GetComponent<PlatformEffector2D>();
				
				if (platformEffector != null)
				{
					// For platform effector, check if player is actually supported by the platform
					// Use a small downward raycast to verify ground contact
					RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 
						~(1 << gameObject.layer)); // Exclude player's own layer
					
					if (hit.collider == c.collider)
					{
						standable.EnterGameObject(gameObject);
					}
				}
				else if (standable.jumpOver)
				{
					// For jumpOver platforms, check if player is above
					if (standable.onAbove(rb2d, objCollider, 0.1f))
					{
						standable.EnterGameObject(gameObject);
					}
				}
				else
				{
					// Normal platform - just enter
					standable.EnterGameObject(gameObject);
				}
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
			Instantiate(spawnEffect, transform.position, Quaternion.identity);
		}
	}

	public void Hide(bool b)
	{
		if (spriteRenderer != null)
			spriteRenderer.enabled = !b;
		if (objCollider != null)
			objCollider.enabled = !b;
	}

	public void OnLeftDown(bool down)
	{
		Debug.Log("OnLeftDown called: " + down);
		if (allowControl)
		{
			isLeftDown = down;
		}
	}

	public void OnRightDown(bool down)
	{
		if (allowControl)
		{
			isRightDown = down;
		}
	}

	public void jump(bool down)
	{
		if (allowControl)
		{
			jumpDown = down;
			if (down && grounded)
			{
				SetJump();
			}
		}
	}

	public void SetJump()
	{
		if (grounded && allowControl)
		{
			velocity.y = jumpTakeOffSpeed;
			spawnJumpSmoke();
			playMoveSound();
		}
	}

	public void down(bool down)
	{
		if (allowControl)
		{
			downKeyDown = down;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{

	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Enemy") && !Invincible)
		{
			die(true, "Enemy");
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
			allowControl = false;

			if (deadEffect != null)
			{
				Instantiate(deadEffect, transform.position, Quaternion.identity);
			}

			Debug.Log("Player died by: " + killer);
		}
	}

	public static void updatePlayAdDeadTimeByIapItem(int iapItem)
	{
		playAdDeadTime = basePlayAdDeadTime + (iapItem * deadTimePerIapItem);
	}

	public void playMoveSound()
	{
		// Play movement sound
	}

	public void playDropSound()
	{
		// Play drop sound
	}

	public void spawnJumpSmoke()
	{
		if (jumpSmoke != null)
		{
			jumpSmoke.spawnSmoke();
		}
	}
	
	public void resetKey()
	{
		isLeftDown = false;
		isRightDown = false;
		jumpDown = false;
		downKeyDown = false;
		lastJumpKeyDown = false;
	}
}
