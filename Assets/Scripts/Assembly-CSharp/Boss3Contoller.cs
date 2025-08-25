using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boss3Contoller : MonoBehaviour
{
	public enum Boss3State
	{
		waiting = 0,
		delayShoot = 1,
		dush = 2,
		rowShoot = 3,
		transform = 4,
		darkMode = 5,
		eyeOpenMode = 6,
		snakeShoot = 7
	}

	public enum Boss3Form
	{
		normal = 0,
		dark = 1,
		eye = 2,
		openArea = 3,
		eyewaiting = 4,
		eyeopen = 5
	}

	public static Boss3Contoller _instance;

	public AudioClip nonStartMusic;

	public Transform godObject;

	public Transform platformInitPoint;

	public Transform floor;

	public Animator[] eyesAni;

	public Tilemap tilemapCom;

	public GameObject portal;

	public PortalLooper portalLooper;

	public ParticleSystem portalPS;

	private Material portalMaterial;

	private float portalDissolve;

	private float portalDissolveSign;

	public GameObject gameEndPortal;

	public bool started;

	public int bossHp;

	private Boss3State state;

	private Boss3Form form;

	private float stateTimer;

	private float stateTimer2;

	private Boss3State nextState;

	private bool onLeft;

	public GameObject tradeBlock;

	public GameObject attachPoints;

	public float godLerpSpeed;

	public float godFlatSpeed;

	public float pushForce;

	public float floorLine;

	public GameObject delayShootBullet;

	public float delayShootMoveTime;

	public int delayShootAmount;

	public float clamp;

	private float delayShootTimeStep;

	private int delayShootPivot;

	private Vector2 crossDirection;

	public float curveLength;

	public float delayShootEyeTimeStep;

	public ParticleSystem dushWaitEffect;

	public float dushWaitTime;

	public Vector2 dushDirection;

	public float dushBaseSpeed;

	public float dushMaxSpeed;

	public float dushAcceleration;

	private float dushSpeed;

	public float dushTime;

	private bool dushing;

	private int dushRound;

	private int dushSide;

	public Mover rowShootBullet;

	public float rowShootTimeStep;

	public int rowShootAngleStep;

	public int rowShootAmount;

	public int rowShootWave;

	private int rowShootSide;

	private int rowShootWavePivot;

	private int rowShootPivot;

	private float rowShootCurrentAngle;

	private float rowShootBaseAngle;

	private int rowShootRound;

	public GameObject Laser;

	public float laserEyeAttackTime;

	public Vector2 laserEyeOffsetRange;

	public bool laserEyeOffEnabled;

	public GameObject snakeBullet;

	public float snakeTime;

	public Transform focusPoint;

	public GameObject focusAttackPrefab;

	public float focusTime;

	private float focusTimer;

	public int focusAttackTime;

	private float focusAttackCounter;

	public float focusAttackTimeGap;

	private bool focusAttacking;

	public float focusInitTime;

	private Vector2[] attackPoints;

	private Vector2 initPos;

	private Vector2 targetPos;

	private Vector2 formual_slop_Yintercept;

	private bool baseMove;

	private bool waitForStory;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void BossInit()
	{
	}

	private void Update()
	{
	}

	private void constantUpdate()
	{
	}

	private void changeState(Boss3State s)
	{
	}

	private void changeWait(Boss3State nextState, float waitTime, int resetPos = -1)
	{
	}

	private void updateWaiting()
	{
	}

	private void changeDelayShoot(Vector2 targetPos)
	{
	}

	private void updateDelayShoot()
	{
	}

	private void updateDelayShootEye()
	{
	}

	private void changeDush()
	{
	}

	private void updateDush()
	{
	}

	private void changeRowShoot()
	{
	}

	private void updateRowShoot()
	{
	}

	private void changeTransform(Boss3State nextState)
	{
	}

	private IEnumerator prepearCharger()
	{
		return null;
	}

	private IEnumerator transformEffect()
	{
		return null;
	}

	private IEnumerator openEyes(bool open)
	{
		return null;
	}

	private void updateTransform()
	{
	}

	private void updateDarkMode()
	{
	}

	private void changeEyeOpen()
	{
	}

	private void updateEyeOpenMode()
	{
	}

	private void changeSnakeShoot()
	{
	}

	private void updateSnakeShoot()
	{
	}

	private IEnumerator laserAttackRound()
	{
		return null;
	}

	private void changeForm(Boss3Form f)
	{
	}

	private void updateDark()
	{
	}

	private void updateEye()
	{
	}

	private int getSign()
	{
		return 0;
	}

	private void focusAttack()
	{
	}

	private void resetFocusAttack()
	{
	}

	private Vector2 formual(Vector2 pos1, Vector2 pos2)
	{
		return default(Vector2);
	}

	private void FixedUpdate()
	{
	}

	public void stopGod()
	{
	}

	public void attackGod()
	{
	}

	private IEnumerator attackGodReset()
	{
		return null;
	}

	public void startStory()
	{
	}

	public void startFight(bool b)
	{
	}

	public void storyStart()
	{
	}

	private void showPortal(bool show)
	{
	}

	public void showAttachPoint(bool show, bool disable = true)
	{
	}
}
