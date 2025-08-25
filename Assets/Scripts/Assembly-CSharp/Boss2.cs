using System.Collections;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
	public enum State
	{
		downShoot = 0,
		transforming = 1,
		singleShoot = 2,
		lastShoot = 3,
		dead = 4,
		wait = 5
	}

	public Animator spike;

	public Transform spikeObjectSet;

	public Transform spikeBulletSet;

	public PressurePlate[] killPressure;

	public SpriteRenderer eye;

	public SpriteRenderer eyeBg;

	public Sprite[] eyeSprite;

	public Sprite[] eyeBgSprite;

	private Transform[] spikeObjects;

	private ForwardBullet[] spikeBullets;

	private bool move;

	private Vector2 targetPos;

	private Vector2 currentPos;

	private Vector2 shakePos;

	private float shakeLevel;

	public float eyeShakeLevel;

	private Transform cameraTransform;

	private Vector3 eyeOriginalPos;

	public ParticleSystem bugParticleSystem;

	public GameObject bugPrefab;

	public Boss2Bullet bugsPrefab;

	public enemy scorpionPrefab;

	private float stateTimer;

	private bool canShoot;

	public float shootDownStateTime;

	public float[] shootDownTime;

	private float shootTimer;

	private int shootType;

	public float transformTime;

	public float[] shootSingleTime;

	private int lastShootType;

	private int sign;

	private float ySpeed;

	private float yAcceleration;

	public float downLevel;

	public float lastAttackWave1;

	public float lastAttackWave2;

	public float lastAttackWave3;

	private State state;

	public int bossLevel;

	private Animator bossAnimator;

	private float oriZ;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void downShootUpdate()
	{
	}

	private void transformingUpdate()
	{
	}

	private void singleShootUpdate()
	{
	}

	private void lastShootUpdate()
	{
	}

	private void changeState(State s)
	{
	}

	private void FixedUpdate()
	{
	}

	public void shootDown()
	{
	}

	public void shootSingle(float speed = -1f, Vector2 direction = default(Vector2))
	{
	}

	public void shakeCam()
	{
	}

	private IEnumerator activeBoss()
	{
		return null;
	}

	public void changeMove(bool b)
	{
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
	}

	private void breakEffect(Vector3 hitPosition)
	{
	}

	private void active(bool b)
	{
	}

	public void hit()
	{
	}
}
