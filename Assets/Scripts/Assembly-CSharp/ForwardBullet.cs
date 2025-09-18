using UnityEngine;

public class ForwardBullet : MonoBehaviour
{
	public string killTag;
	public float speed;
	public float destoryTime;
	private float timer;
	public bool hideObject;
	public bool killTotem;
	public bool hitPlatform;
	private bool isHide;
	public bool destroyAnimation;
	public Vector3 direction;
	private Collider2D col;
	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		col = GetComponent<Collider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		timer = 0f;
	}

	private void Update()
	{
		// Move bullet forward
		transform.Translate(direction * speed * Time.deltaTime);
		
		// Update timer and destroy after destoryTime
		timer += Time.deltaTime;
		if (timer >= destoryTime)
		{
			dead();
		}
	}

	public void hide(bool b)
	{
		isHide = b;
		if (spriteRenderer != null)
			spriteRenderer.enabled = !b;
		if (col != null)
			col.enabled = !b;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Check if hit player with matching kill tag
		if (!string.IsNullOrEmpty(killTag) && other.CompareTag(killTag))
		{
			dead();
			return;
		}
		
		// Kill totems if enabled
		if (killTotem && other.CompareTag("object"))
		{
			AttachBlock attachBlock = other.GetComponent<AttachBlock>();
			if (attachBlock != null)
			{
				// Damage or destroy the totem
				dead();
				return;
			}
		}
		
		// Hit platforms if enabled
		if (hitPlatform && (other.CompareTag("platform") || other.CompareTag("Ground")))
		{
			dead();
			return;
		}
	}

	public void dead()
	{
		if (destroyAnimation)
		{
			// Play destruction animation if available
			Animator anim = GetComponent<Animator>();
			if (anim != null)
			{
				anim.SetTrigger("Destroy");
				// Destroy after animation completes (assume 0.5s)
				Destroy(gameObject, 0.5f);
			}
			else
			{
				Destroy(gameObject);
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}
}