using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public static PlayerPlatformerController _instance;
    public GameObject deadEffect;
    public GameObject spawnEffect;
    public JumpSmoke jumpSmoke;

    // Controls
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
    public float attachRange = 2f;

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

    // NEW: attached state (player visually hidden and physics locked at totem)
    public bool attached;

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
        origanalGraRb2d = rb2d != null ? rb2d.gravityScale : 1f;

        playAdDeadTime = basePlayAdDeadTime;
    }

    private void Start()
    {
        allowControl = true;
        createSpawnEffect();
        StartCoroutine(CheckAndLoadStoryAfterStart());
    }

    private System.Collections.IEnumerator CheckAndLoadStoryAfterStart()
    {
        yield return null;
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
        else if (attached)
        {
            UpdateAttached();
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
        bool keyboardLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool keyboardRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool keyboardJump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow);
        bool keyboardDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        leftInput = isLeftDown || keyboardLeft;
        rightInput = isRightDown || keyboardRight;
        jumpInput = jumpDown || keyboardJump;
        downInput = downKeyDown || keyboardDown;

        if (animator != null)
        {
            animator.SetBool("grounded", grounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        }

        if (jumpInput && !lastJumpKeyDown && grounded)
            SetJump();

        if (grounded && !lastJumpKeyDown)
        {
            spawnJumpSmoke();
            playDropSound();
        }

        lastJumpKeyDown = jumpInput;

        if (transform.position.y < deadHeight)
            die(true, "Fall");
    }

    private void UpdateDie()
    {
        dead += Time.deltaTime;
        if (dead >= 2f)
        {
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
        // Player is blocked from movement - gradually stop movement
        velocity = Vector2.Lerp(velocity, Vector2.zero, Time.deltaTime * 5f);

        // Reset some inputs
        jumpDown = false;
        downKeyDown = false;
        lastJumpKeyDown = false;

        // Allow flipping while blocked
        bool leftPressed = isLeftDown || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool rightPressed = isRightDown || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (spriteRenderer != null)
        {
            if (leftPressed && !rightPressed) spriteRenderer.flipX = true;
            else if (rightPressed && !leftPressed) spriteRenderer.flipX = false;
        }
    }

    // Called when attached: keep sprite hidden, physics locked, allow flip input to be registered.
    private void UpdateAttached()
    {
        // Keep velocity zero (RB is kinematic but safe-guard)
        if (rb2d != null)
        {
            if (rb2d.bodyType != RigidbodyType2D.Kinematic)
            {
                rb2d.bodyType = RigidbodyType2D.Kinematic;
                rb2d.gravityScale = 0f;
                rb2d.linearVelocity = Vector2.zero;
            }
        }

        // Allow flipping visuals while attached (without moving)
        bool leftPressed = isLeftDown || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool rightPressed = isRightDown || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if (spriteRenderer != null)
        {
            if (leftPressed && !rightPressed) spriteRenderer.flipX = true;
            else if (rightPressed && !leftPressed) spriteRenderer.flipX = false;
        }

        // If user presses left/right/jump, corresponding input handlers will call Detach(...)
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

    // ACTIVE ATTACH: attempt attach/detach/teleport to another totem
  // ACTIVE ATTACH: attempt attach/detach/teleport to another totem
public void activeAttach()
{
    // If there's a selected target that is valid and attachable, try it first.
    if (Block.target != null)
    {
        AttachBlock attachBlock = Block.target as AttachBlock;
        if (attachBlock != null && attachBlock.conditionCheck())
        {
            attachBlock.activeBlock(true);
            return;
        }
    }

    // Detect if player is attached by flag
    bool playerIsAttached = attached;

    if (Block.blockList == null || Block.blockList.Count == 0) return;
    Vector2 myPos = transform.position;
    List<AttachBlock> candidates = new List<AttachBlock>();
    foreach (Block b in Block.blockList)
    {
        if (b == null) continue;
        AttachBlock ab = b as AttachBlock;
        if (ab == null) continue;
        if (!ab.conditionCheck()) continue;

        // REMOVED: The restriction that prevented reattaching to the same totem
        // Now allows reattaching to any totem within range, including the current one

        float d = Vector2.Distance(ab.transform.position, myPos);
        if (d <= attachRange)
            candidates.Add(ab);
    }

    if (candidates.Count == 0)
    {
        if (playerIsAttached)
        {
            // forced detach (no input) - clear inputs to avoid sticky facing
            Detach(false, true);
        }
        return;
    }

    // Prefer blocks in player's facing direction
    int facing = controlDirection(); // -1 left, 1 right, 0 none
    List<AttachBlock> facingList = new List<AttachBlock>();
    if (facing != 0)
    {
        foreach (var ab in candidates)
        {
            float dx = ab.transform.position.x - myPos.x;
            int side = dx < -0.01f ? -1 : (dx > 0.01f ? 1 : 0);
            if (side == facing) facingList.Add(ab);
        }
    }

    List<AttachBlock> pickList = (facingList.Count > 0) ? facingList : candidates;

    // Choose nearest
    AttachBlock chosen = null;
    float bestDist = float.MaxValue;
    foreach (var ab in pickList)
    {
        float d = Vector2.Distance(ab.transform.position, myPos);
        if (d < bestDist)
        {
            bestDist = d;
            chosen = ab;
        }
    }

    if (chosen != null)
    {
        // If we were attached, first detach from current (unhide + restore dynamic body) so attach sequence is clean.
        if (playerIsAttached)
        {
            Detach(false, true); // forced detach to reset physics/inputs before reattaching
        }

        chosen.clickOn(); // keep UI/selection consistent
        chosen.activeBlock(true);
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
        // If attached, we don't compute movement
        if (attached)
        {
            targetVelocity = Vector2.zero;
            return;
        }

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

        // FIX: Only update sprite flip when we have actual movement or input
        if (spriteRenderer != null)
        {
            // Only flip if there's clear directional input, not just from continued movement
            if (leftInput && !rightInput)
                spriteRenderer.flipX = true;
            else if (rightInput && !leftInput)
                spriteRenderer.flipX = false;
            // Don't change flip state if no clear input
        }

        targetVelocity = move * maxSpeed;
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (grounded && c.gameObject.CompareTag("platform"))
        {
            Standable standable = c.gameObject.GetComponent<Standable>();
            if (standable != null && standable.moveGoods)
            {
                PlatformEffector2D platformEffector = c.gameObject.GetComponent<PlatformEffector2D>();

                if (platformEffector != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, ~(1 << gameObject.layer));
                    if (hit.collider == c.collider)
                    {
                        standable.EnterGameObject(gameObject);
                    }
                }
                else if (standable.jumpOver)
                {
                    if (standable.onAbove(rb2d, objCollider, 0.1f))
                    {
                        standable.EnterGameObject(gameObject);
                    }
                }
                else
                {
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

    // Hide optionally disables collider. By default, keep old behaviour (disable collider).
    // This now syncs all GameObjects tagged "Player".
    public void Hide(bool hideVisuals, bool disableCollider = true)
    {
        // Hide/reveal *all* GameObjects with tag "Player" (so playershadow and separate Player-tagged objects are synced)
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null)
        {
            foreach (GameObject p in players)
            {
                if (p == null) continue;
                SpriteRenderer sr = p.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.enabled = !hideVisuals;
            }
        }

        // Main player collider behavior (existing behavior controlled by disableCollider param)
        if (objCollider != null && disableCollider)
            objCollider.enabled = !hideVisuals;

        // Ensure our cached spriteRenderer also follows the change (in case of non-tagged child)
        if (spriteRenderer != null)
            spriteRenderer.enabled = !hideVisuals;
    }


    /// Detach the player from a totem. Optionally apply a jump impulse.
    /// If clearInputs==true we clear input flags (used for non-key forced detaches).
    public void Detach(bool applyJump, bool clearInputs = false)
    {
        Rigidbody2D prb = GetComponent<Rigidbody2D>();
        if (prb != null)
        {
            prb.bodyType = RigidbodyType2D.Dynamic;
            prb.gravityScale = origanalGraRb2d;
            if (applyJump)
                prb.linearVelocity = new Vector2(prb.linearVelocity.x, jumpTakeOffSpeed);
            else
                prb.linearVelocity = Vector2.zero;
        }

        // Reveal all Player-tagged sprites and re-enable collider
        Hide(false, true);

        // re-enable control and clear attached flag
        allowControl = true;
        attached = false;

        // If detach was forced (not triggered by input), clear sticky inputs to avoid odd facing
        if (clearInputs)
        {
            resetKey();
        }
    }

    // FIX: Prevent detaching while attached
    public void OnLeftDown(bool down)
    {
        Debug.Log("OnLeftDown called: " + down);
        
        // If attached, don't process UI input - only allow detach through keyboard/jump
        if (attached && down)
        {
            return; // Ignore UI input while attached
        }
        
        // Always set flag so we can detach and then let movement continue if pressed
        isLeftDown = down;
    }

    // FIX: Prevent detaching while attached
    public void OnRightDown(bool down)
    {
        // If attached, don't process UI input - only allow detach through keyboard/jump  
        if (attached && down)
        {
            return; // Ignore UI input while attached
        }
        
        isRightDown = down;
    }

    public void jump(bool down)
    {
        // If control allowed, behave normally
        if (allowControl)
        {
            jumpDown = down;
            if (down && grounded)
            {
                SetJump();
            }
        }
        else
        {
            // If attached and jump pressed -> detach and apply jump
            if (attached && down)
            {
                // mark jump input so after detaching jump logic can see it if needed
                jumpDown = down;
                Detach(true, false); // apply upward velocity; keep input state (we want immediate jump)
            }
        }
    }

    public void SetJump()
    {
        // Normal grounded jump when control allowed
        if (grounded && allowControl)
        {
            velocity.y = jumpTakeOffSpeed;
            spawnJumpSmoke();
            playMoveSound();
            return;
        }

        // If somehow called while attached, ensure detach+jump
        if (attached)
        {
            Detach(true, false);
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
        // Handle death from Trap tag
        if (other.CompareTag("Trap") && !Invincible)
        {
            die(true, "Trap");
            return;
        }

        // Handle death from bullet tag
        if (other.CompareTag("bullet") && !Invincible)
        {
            die(true, "Bullet");
            return;
        }

        if (other.CompareTag("object"))
        {
            AttachBlock attachBlock = other.GetComponent<AttachBlock>();
            if (attachBlock != null)
            {
                if (attachBlock.conditionCheck())
                {
                    attachBlock.clickOn();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Handle death from Enemy tag
        if (other.gameObject.CompareTag("Enemy") && !Invincible)
        {
            die(true, "Enemy");
            return;
        }

        // Handle death from Trap tag (collision-based traps)
        if (other.gameObject.CompareTag("Trap") && !Invincible)
        {
            die(true, "Trap");
            return;
        }

        // Handle death from bullet tag (collision-based bullets)
        if (other.gameObject.CompareTag("bullet") && !Invincible)
        {
            die(true, "Bullet");
            return;
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
        if (GameManager._instance != null)
        {
            GameManager._instance.resetLevel();
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