using UnityEngine;

public class AttachBlock : Block
{
    private static ParticleSystem attachEffect;
    public Sprite[] sprites;
    public bool storeSpeed;
    private Vector2 playerStoredSpeed;
    public Vector2 placeOffset;
    private SpriteRenderer sr;
    private Animator animator;
    public float recoverTime = 1f;
    private float recoverTimer;
    public bool disableSpriteUpdate;
    private bool isActive;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isActive = true;
        recoverTimer = 0f;

        // Prefer the AttachEffect located under BaseGamePackage
        if (attachEffect == null)
        {
            GameObject basePkg = GameObject.Find("BaseGamePackage");
            if (basePkg != null)
            {
                Transform t = basePkg.transform.Find("AttachEffect");
                if (t != null)
                    attachEffect = t.GetComponent<ParticleSystem>();
                else
                    attachEffect = basePkg.GetComponentInChildren<ParticleSystem>();
            }

            // Fallback: create a simple one if nothing found
            if (attachEffect == null)
            {
                GameObject effectObj = new GameObject("AttachEffect");
                attachEffect = effectObj.AddComponent<ParticleSystem>();
            }
        }
    }

    private void Update()
    {
        if (recoverTimer > 0f)
        {
            recoverTimer -= Time.deltaTime;
            if (recoverTimer <= 0f)
            {
                isActive = true;
                if (!disableSpriteUpdate && sprites.Length > 0 && sr != null)
                {
                    sr.sprite = sprites[0]; // Active sprite
                }
            }
        }

        if (!disableSpriteUpdate && sprites.Length >= 2 && sr != null)
        {
            sr.sprite = isActive ? sprites[0] : sprites[1];
        }
    }

    // FIX: Allow mid-air attachment by removing grounded requirement
    public override bool conditionCheck()
    {
        if (!isActive) return false;

        PlayerPlatformerController ppc = PlayerPlatformerController._instance;
        if (ppc != null)
        {
            // Allow attaching regardless of grounded state; only require distance within attachRange
            float distance = Vector2.Distance(transform.position, ppc.transform.position);
            return distance <= ppc.attachRange;
        }
        else
        {
            // fallback to small hardcoded range
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                return distance <= 2f;
            }
        }
        return false;
    }

    public void activeBlock(int isActiveInt)
    {
        activeBlock(isActiveInt != 0);
    }

    public void activeBlock(bool isActiveState)
    {
        if (!isActiveState || !conditionCheck())
            return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        PlayerPlatformerController ppc = PlayerPlatformerController._instance;

        // store player's velocity if requested
        if (storeSpeed && playerRb != null)
        {
            playerStoredSpeed = playerRb.linearVelocity;
        }

        // Teleport player to totem position + offset (world position)
        Vector3 attachPosition = transform.position + (Vector3)placeOffset;
        player.transform.position = attachPosition;

        // Make player's Rigidbody kinematic so they stay fixed in place and do not fall
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            playerRb.bodyType = RigidbodyType2D.Kinematic;
            playerRb.gravityScale = 0f;
        }

        // Hide player visuals but KEEP collider active so player does not fall through
        if (ppc != null)
        {
            ppc.Hide(true, false);   // hide sprite only, keep collider enabled
            ppc.allowControl = false;
            ppc.attached = true;     // mark attached state
        }

        // Play attach effect as a single burst
        if (attachEffect == null)
        {
            GameObject basePkg = GameObject.Find("BaseGamePackage");
            if (basePkg != null)
            {
                Transform t = basePkg.transform.Find("AttachEffect");
                if (t != null) attachEffect = t.GetComponent<ParticleSystem>();
                else attachEffect = basePkg.GetComponentInChildren<ParticleSystem>();
            }
        }
        if (attachEffect != null)
        {
            attachEffect.transform.position = transform.position;
            attachEffect.Clear();
            attachEffect.Emit(12);
        }

        // Mark totem temporarily inactive and start recovery timer
        isActive = false;
        recoverTimer = recoverTime;

        // Update visual sprite for totem
        if (!disableSpriteUpdate && sprites.Length >= 2 && sr != null)
        {
            sr.sprite = sprites[1]; // inactive look
        }
        if (animator != null)
        {
            animator.SetTrigger("Attach");
        }

        // Keep it selected
        selectObject(true);
    }

    protected override void selectObject(bool b, Block lastObject = null)
    {
        base.selectObject(b, lastObject);

        if (sr == null) sr = GetComponent<SpriteRenderer>();

        if (b && conditionCheck())
        {
            if (sr != null)
            {
                sr.color = new Color(1f, 1f, 1f, 0.8f);
            }
        }
        else
        {
            if (sr != null)
            {
                sr.color = Color.white;
            }
        }
    }
}