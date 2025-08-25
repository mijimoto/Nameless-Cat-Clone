using UnityEngine;

public class Boss2Bullet : MonoBehaviour
{
	public enum MoveType
	{
		rotate = 0,
		addSpeed = 1
	}

	public Transform target;

	public MoveType mt;

	public float baseSpeed;

	public float maxSpeed;

	public float acceleration;

	public bool lockRotate;

	public float rotateSpeed;

	public float stopRotateTime;

	private float stopRotateTimer;

	private float currentSpeed;

	private Vector2 velocity;

	public Vector2 currentRotation;

	public float deadTime;

	private float deadTimer;

	private bool die;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	private void updateRotation()
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	public void dead()
	{
	}

	private void remove()
	{
	}
}
