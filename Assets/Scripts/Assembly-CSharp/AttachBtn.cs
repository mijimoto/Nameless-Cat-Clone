using System.Collections.Generic;
using UnityEngine;

public class AttachBtn : MonoBehaviour
{
	public class Chain
	{
		public Transform transform;

		public Vector2 targetPos;

		public Vector2 velocity;

		public static float acceleration;

		public static float length;

		public static void setup(float acceleration, float length)
		{
		}

		public Chain(Transform transform)
		{
		}

		public bool updatePos(Vector2 newTargetPos)
		{
			return false;
		}

		public bool updatePos()
		{
			return false;
		}

		public void physicsUpdate()
		{
		}

		public void updateRotation(Vector2 pos1, Vector2 pos2)
		{
		}
	}

	private bool activing;

	private Vector2 godPos;

	private Vector2 targetPos;

	public Sprite[] sprites;

	private SpriteRenderer sr;

	private Animator animator;

	public Animator landingEffect;

	private Vector2 crossDirection;

	private List<Chain> chains;

	public float chainAcceleration;

	private Vector2 chainPivot;

	public float chainPivotMoveSpeed;

	private bool chainActive;

	private float exploseTime;

	private bool goDown;

	private void OnDrawGizmosSelected()
	{
	}

	private void Awake()
	{
	}

	private void Update()
	{
	}

	public void reset(Vector2 startPos)
	{
	}

	public void explose(bool left)
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	public void showObj(bool show)
	{
	}
}
