using UnityEngine;

public class enemy : MonoBehaviour
{
    public enum State
    {
        move = 0,
        attack = 1,
        shoot = 2,
        dush = 3
    }
    
    public float speed;
    public Vector2[] path;
    public Vector2 maxRange;
    private int pointIndex;
    protected RaycastHit2D hit;
    private SpriteRenderer sr;
    private Animator animator;
    private static int layerMask;
    private Vector3 oriPos;
    private float width;
    private float offsetY;
    
    public bool canAttack;
    public bool canShoot;
    public float shootTime;
    public float shootStartTime;
    private float shootTimer;
    public bool useForwardBullet;
    public Vector3 forwardBulletShootDirection;
    public bool shootByAnimation;
    
    public bool canDush;
    public float dushDetectDistance;
    public float dushWaitTime;
    public float dushSpeed;
    public float dushDistance;
    public int dushLayerMask;
    private float targetX;
    private float dushTimer;
    private int dushState;
    public float dushShakeLevel;
    
    public bool vertical;
    public bool fly;
    public bool lockFace;
    public GameObject bullet;
    
    private State state;
    private AttachBlock target;

    private void OnDrawGizmosSelected()
    {
        // Draw patrol path
        if (path != null && path.Length > 1)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < path.Length - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
            if (path.Length > 2)
                Gizmos.DrawLine(path[path.Length - 1], path[0]);
        }
        
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, maxRange * 2f);
        
        // Draw dash detection
        if (canDush)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, dushDetectDistance);
        }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        oriPos = transform.position;
        layerMask = LayerMask.GetMask("Player");
        
        width = GetComponent<Collider2D>()?.bounds.size.x ?? 1f;
        offsetY = GetComponent<Collider2D>()?.bounds.size.y / 2f ?? 0.5f;
        
        state = State.move;
        pointIndex = 0;
        shootTimer = 0f;
        dushState = 0;
        dushTimer = 0f;
    }

    private void Update()
    {
        // Only debug shooting enemies to reduce spam
        if (canShoot && Time.frameCount % 60 == 0) // Every 60 frames (1 second at 60fps)
        {
        }
        switch (state)
        {
            case State.move:
                UpdateMove();
                break;
            case State.attack:
                UpdateAttack();
                break;
            case State.shoot:
                UpdateShoot();
                break;
            case State.dush:
                UpdateDush();
                break;
        }
        
        UpdateAnimator();
    }
    
    private void UpdateMove()
    {
        // Patrol along path
        if (path != null && path.Length > 1) // Need at least 2 points for a path
        {
            Vector2 targetPoint = path[pointIndex];
            Vector2 currentPos = transform.position;
            Vector2 direction = (targetPoint - currentPos).normalized;
            
        
            
            // Move towards target point
            if (!vertical)
            {
                transform.position = new Vector3(
                    transform.position.x + direction.x * speed * Time.deltaTime,
                    transform.position.y,
                    transform.position.z
                );
            }
            else
            {
                transform.position = new Vector3(
                    transform.position.x + direction.x * speed * Time.deltaTime,
                    transform.position.y + direction.y * speed * Time.deltaTime,
                    transform.position.z
                );
            }
            
            // Flip sprite based on movement direction (normal facing)
            if (!lockFace && sr != null && direction.x != 0)
                sr.flipX = direction.x > 0; // Flip when moving right
            
            // Check if reached target point - move to next waypoint
            if (Vector2.Distance(transform.position, targetPoint) < 0.5f) // Increased detection range
            {
               
            }
        }
        else if (path != null && path.Length == 1)
        {
            // Single point path - just stay at that point
            transform.position = new Vector3(path[0].x, path[0].y, transform.position.z);
        }
        else if (path == null || path.Length == 0)
        {
        }
        
        // Always check for player detection regardless of path
        DetectPlayer();
    }
    
    private void UpdateAttack()
    {
        // Attack behavior - could involve melee combat
        // Return to move state after attack
        changeState(State.move);
    }
    
    private void UpdateShoot()
    {
        shootTimer += Time.deltaTime;
        
        // Handle case where shootStartTime is 0 - start immediately
        float actualStartTime = shootStartTime <= 0 ? 0f : shootStartTime;
        // Handle case where shootTime is 0 - default to 2 seconds
        float actualShootDuration = shootTime <= 0 ? 2f : shootTime;
        
        // Start shooting after delay (or immediately if 0)
        if (shootTimer >= actualStartTime && shootTimer <= actualStartTime + actualShootDuration)
        {
            if (!shootByAnimation)
            {
                // Shoot at intervals during shoot duration
                float shootInterval = 0.5f; // Shoot every 0.5 seconds
                float timeInShootPhase = shootTimer - actualStartTime;
                
                if (Mathf.FloorToInt(timeInShootPhase / shootInterval) > Mathf.FloorToInt((timeInShootPhase - Time.deltaTime) / shootInterval))
                { shootB();
                }
            }
        }
        
        // End shooting after full duration
        if (shootTimer >= actualStartTime + actualShootDuration)
        {
            Debug.Log($"Enemy {gameObject.name} finished shooting, returning to move");
            shootTimer = 0f;
            changeState(State.move);
        }
    }
    
    private void UpdateDush()
    {
        switch (dushState)
        {
            case 0: // Wait state
                dushTimer += Time.deltaTime;
                if (dushTimer >= dushWaitTime)
                {
                    dushState = 1;
                    dushTimer = 0f;
                    // Set target position for dash
                    Vector2 playerPos = PlayerPlatformerController._instance.transform.position;
                    targetX = playerPos.x;
                }
                break;
                
            case 1: // Dash state
                float dashDirection = Mathf.Sign(targetX - transform.position.x);
                transform.Translate(dashDirection * dushSpeed * Time.deltaTime, 0, 0);
                
                // Check if dash is complete or hit obstacle
                if (Mathf.Abs(transform.position.x - oriPos.x) >= dushDistance ||
                    Physics2D.Raycast(transform.position, Vector2.right * dashDirection, 0.5f, dushLayerMask))
                {
                    dushState = 0;
                    dushTimer = 0f;
                    changeState(State.move);
                }
                break;
        }
    }
    
    private void DetectPlayer()
    {
        if (PlayerPlatformerController._instance == null) return;
        
        Vector2 playerPos = PlayerPlatformerController._instance.transform.position;
        Vector2 myPos = transform.position;
        Vector2 diff = playerPos - myPos;
        
        // Check if player is in range
        bool inRange = Mathf.Abs(diff.x) <= maxRange.x && Mathf.Abs(diff.y) <= maxRange.y;
        
        // Debug player detection every 3 seconds
        if (Time.frameCount % 180 == 0)
        {
            Debug.Log($"Enemy {gameObject.name} - Player at {playerPos}, Enemy at {myPos}, Diff: {diff}, MaxRange: {maxRange}, InRange: {inRange}, CanShoot: {canShoot}");
        }
        
        if (inRange)
        {
            // Dash detection (closest range, highest priority)
            if (canDush && Vector2.Distance(playerPos, myPos) <= dushDetectDistance)
            {
                Debug.Log($"Enemy {gameObject.name} starting dash attack");
                changeState(State.dush);
                return;
            }
            
            // Shoot detection (medium range)
            if (canShoot && state != State.shoot) // Only start shooting if not already shooting
            {
                Debug.Log($"Enemy {gameObject.name} starting shoot state - player detected!");
                changeState(State.shoot);
                return;
            }
            
            // Attack detection (close range)
            if (canAttack && Vector2.Distance(playerPos, myPos) <= 2f)
            {
                Debug.Log($"Enemy {gameObject.name} starting attack");
                changeState(State.attack);
                return;
            }
        }
    }
    
    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetInteger("State", (int)state);
            animator.SetBool("Moving", state == State.move);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canAttack)
        {
            changeState(State.attack);
        }
    }

    public void changeState(State s)
    {
        Debug.Log($"Enemy {gameObject.name} changing state from {state} to {s}");
        state = s;
        
        // Reset timers when changing state
        if (s == State.shoot)
        {
            shootTimer = 0f;
            Debug.Log($"Enemy {gameObject.name} entering shoot state - shootStartTime: {shootStartTime}, shootTime: {shootTime}");
        }
        else if (s == State.dush)
        {
            dushState = 0;
            dushTimer = 0f;
        }
    }

    public void shootB()
{
    Debug.Log($"Enemy {gameObject.name} shootB() called - bullet prefab: {(bullet != null ? bullet.name : "NULL")}");
    if (bullet == null) 
    {
        Debug.LogError($"Enemy {gameObject.name} - bullet prefab is null!");
        return;
    }

    Vector3 spawnPos = transform.position;
    GameObject bulletObj = null;

    try
    {
        if (useForwardBullet)
        {
            bulletObj = Instantiate(bullet, spawnPos, Quaternion.identity);
            ForwardBullet fb = bulletObj.GetComponent<ForwardBullet>();
            Vector3 shootDir = Vector3.right;
            if (fb != null)
            {
                if (forwardBulletShootDirection != Vector3.zero)
                    fb.direction = forwardBulletShootDirection.normalized;
                else if (PlayerPlatformerController._instance != null)
                    fb.direction = (PlayerPlatformerController._instance.transform.position - spawnPos).normalized;
                else
                    fb.direction = Vector3.right;

                shootDir = fb.direction;
            }
            else
            {
                Debug.LogError("ForwardBullet component not found on bullet prefab!");
            }

            // offset spawn so bullet doesn't start overlapping the enemy
            bulletObj.transform.position = spawnPos + shootDir.normalized * (width * 0.6f + 0.05f);

            // ignore collision between bullet and this enemy
            Collider2D bulletCol = bulletObj.GetComponent<Collider2D>();
            Collider2D myCol = GetComponent<Collider2D>();
            if (bulletCol != null && myCol != null)
                Physics2D.IgnoreCollision(bulletCol, myCol);
        }
        else
        {
            bulletObj = Instantiate(bullet, spawnPos, Quaternion.identity);
            bullet bulletScript = bulletObj.GetComponent<bullet>();
            Vector3 shootDir = Vector3.right;
            if (bulletScript != null)
            {
                bool shootRight;
                if (PlayerPlatformerController._instance != null)
                {
                    shootRight = PlayerPlatformerController._instance.transform.position.x > transform.position.x;
                }
                else
                {
                    shootRight = sr != null ? !sr.flipX : true;
                }
                bulletScript.initialize(shootRight, vertical);
                shootDir = shootRight ? Vector3.right : Vector3.left;
            }
            else
            {
                Debug.LogError("bullet component not found on bullet prefab!");
            }

            bulletObj.transform.position = spawnPos + shootDir.normalized * (width * 0.6f + 0.05f);
            Collider2D bulletCol = bulletObj.GetComponent<Collider2D>();
            Collider2D myCol = GetComponent<Collider2D>();
            if (bulletCol != null && myCol != null)
                Physics2D.IgnoreCollision(bulletCol, myCol);
        }

        if (bulletObj != null)
            Debug.Log($"Successfully spawned bullet: {bulletObj.name}");
    }
    catch (System.Exception e)
    {
        Debug.LogError($"Error spawning bullet: {e.Message}");
    }
}


    public void takeDown()
    {
        // Called when enemy is defeated
        Destroy(gameObject);
    }

    public enemy setPath(Vector2[] newPath)
    {
        path = newPath;
        pointIndex = 0;
        return this;
    }

    public enemy moveNext()
    {
        if (path != null && path.Length > 0)
        {
            pointIndex = (pointIndex + 1) % path.Length;
        }
        return this;
    }
}