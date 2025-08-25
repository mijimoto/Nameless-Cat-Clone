using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
	public float minGroundNormalY;

	public float gravityModifier;

	public float maxDistance;

	public int ignoreLayer;

	public bool noChangeOnAir;

	public bool physicsMove;

	protected Vector2 targetVelocity;

	protected bool grounded;

	protected Vector2 groundNormal;

	protected Rigidbody2D rb2d;

	protected Vector2 velocity;

	protected ContactFilter2D contactFilter;

	protected RaycastHit2D[] hitBuffer;

	protected List<RaycastHit2D> hitBufferList;

	protected bool canMove;

	protected const float minMoveDistance = 0.001f;

	protected const float shellRadius = 0.01f;

	protected bool velocityForceUpdate;

	private bool jumpFrameSkip;

	private Vector2 forceMove;

	public Rigidbody2D Rb2d => null;

	private void OnEnable()
	{
	}

	protected virtual void Update()
	{
	}

	protected virtual void ComputeVelocity()
	{
	}

	protected virtual void stopCallback()
	{
	}

	protected virtual void FixedUpdate()
	{
	}

	public virtual void setTargetVelocity(Vector2 v2)
	{
	}

	public virtual void addVelocity(Vector2 v2)
	{
	}

	public Vector2 getVelocity()
	{
		return default(Vector2);
	}

	private void Movement(Vector2 move, bool yMovement)
	{
	}

	public bool isGround()
	{
		return false;
	}

	public void setForce(Vector2 force)
	{
	}
}
