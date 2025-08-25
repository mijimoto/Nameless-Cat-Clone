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
	}

	private void Update()
	{
	}

	public void hide(bool b)
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	public void dead()
	{
	}
}
