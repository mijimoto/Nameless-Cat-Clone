using UnityEngine;

public class Mover : MonoBehaviour
{
	private int id;

	private GameObject callBackObject;

	public Vector3 baseSpeed;

	public Vector3 acceleration;

	public float lifeTime;

	private Vector3 speed;

	private Vector3 initialPos;

	public bool useTargetPos;

	public bool useMoveTime;

	public float moveTime;

	private float moveTimer;

	public Vector3 targetPos;

	public Vector3 targetPosOffset;

	public Vector3 moveInPosOffset;

	public float offsetMoveTime;

	private float offsetMoveTimer;

	private bool offsetMoveFinished;

	private bool offsetMoveFollowCam;

	public float waitTime;

	private float waitTimer;

	private bool canMove;

	private Transform chaseTarget;

	private void Awake()
	{
	}

	private void Update()
	{
	}

	public void setChaseTarget(Transform chaseTarget, bool updateOffset)
	{
	}

	public void changeDirection(Vector3 direction, bool updateOffset = false)
	{
	}

	public void setCallback(int id, GameObject go)
	{
	}

	public Mover setTargetPos(Vector3 targetPos, Vector3 offset)
	{
		return null;
	}

	public Mover setSpeed(float newSpeed)
	{
		return null;
	}

	public Mover setMoveTime(float moveTime)
	{
		return null;
	}

	public static Vector3 DirFromAngle(float angleInDegree)
	{
		return default(Vector3);
	}

	public static float angleFormDir(Vector2 direction)
	{
		return 0f;
	}

	public static float ParametricBlend(float t)
	{
		return 0f;
	}

	public static float sinEase(float t)
	{
		return 0f;
	}
}
