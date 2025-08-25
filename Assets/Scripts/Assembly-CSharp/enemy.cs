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
	}

	private void Awake()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerStay2D(Collider2D other)
	{
	}

	public void changeState(State s)
	{
	}

	public void shootB()
	{
	}

	public void takeDown()
	{
	}

	public enemy setPath(Vector2[] newPath)
	{
		return null;
	}

	public enemy moveNext()
	{
		return null;
	}
}
