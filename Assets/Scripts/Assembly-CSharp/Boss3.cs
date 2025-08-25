using UnityEngine;

public class Boss3 : MonoBehaviour
{
	public enum AttackType
	{
		wait = 0,
		fan = 1,
		drop = 2,
		laser = 3
	}

	private float attackTimer;

	public float attackGapTime;

	public Vector2 fanPower;

	public float fanDuration;

	public GameObject dropObject;

	public float dropDuration;

	public float dropAttackTime;

	private float dropAttackTimer;

	public Animator laserShooter;

	public float laserDuration;

	public float laserReadyTime;

	public float laserWaitTime;

	public float laserFollowSpeed;

	private int laserActiveType;

	private Vector2 laserPos;

	private Vector2 camBorder;

	private AttackType attackType;

	private AttackType lastType;

	private void changeAttackType(bool forceChange = false)
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
	}

	private void UpdateWait()
	{
	}

	private void UpdateFan()
	{
	}

	private void UpdateDrop()
	{
	}

	private void UpdateLaser()
	{
	}

	private void FixedUpdateLaser()
	{
	}
}
