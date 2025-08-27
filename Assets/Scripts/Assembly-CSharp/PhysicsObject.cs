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

	public Rigidbody2D Rb2d => rb2d;

	private void OnEnable()
	{
		rb2d = GetComponent<Rigidbody2D>();
		hitBuffer = new RaycastHit2D[16];
		hitBufferList = new List<RaycastHit2D>(16);
		
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
		
		canMove = true;
		minGroundNormalY = 0.65f;
		gravityModifier = 1f;
		maxDistance = 1f;
	}

	protected virtual void Update()
	{
		targetVelocity = Vector2.zero;
		ComputeVelocity();
	}

	protected virtual void ComputeVelocity()
	{
		// Override in derived classes to compute target velocity
	}

	protected virtual void stopCallback()
	{
		// Override in derived classes for stop events
	}

	protected virtual void FixedUpdate()
	{
		// Apply gravity
		if (!noChangeOnAir || grounded)
		{
			velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime;
		}

		// Apply target velocity for horizontal movement
		velocity.x = targetVelocity.x;

		// Handle force-based movement
		if (forceMove != Vector2.zero)
		{
			velocity += forceMove;
			forceMove = Vector2.zero;
		}

		grounded = false;

		Vector2 deltaPosition = velocity * Time.fixedDeltaTime;

		Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

		Vector2 move = moveAlongGround * deltaPosition.x;

		Movement(move, false);

		move = Vector2.up * deltaPosition.y;

		Movement(move, true);
	}

	public virtual void setTargetVelocity(Vector2 v2)
	{
		targetVelocity = v2;
	}

	public virtual void addVelocity(Vector2 v2)
	{
		velocity += v2;
	}

	public Vector2 getVelocity()
	{
		return velocity;
	}

	private void Movement(Vector2 move, bool yMovement)
	{
		if (!canMove) return;

		float distance = move.magnitude;

		if (distance > minMoveDistance)
		{
			int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
			hitBufferList.Clear();

			for (int i = 0; i < count; i++)
			{
				hitBufferList.Add(hitBuffer[i]);
			}

			for (int i = 0; i < hitBufferList.Count; i++)
			{
				Vector2 currentNormal = hitBufferList[i].normal;
				
				if (currentNormal.y > minGroundNormalY)
				{
					grounded = true;
					if (yMovement)
					{
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				float projection = Vector2.Dot(velocity, currentNormal);
				if (projection < 0)
				{
					velocity = velocity - projection * currentNormal;
				}

				float modifiedDistance = hitBufferList[i].distance - shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}

		rb2d.position = rb2d.position + move.normalized * distance;
	}

	public bool isGround()
	{
		return grounded;
	}

	public void setForce(Vector2 force)
	{
		forceMove = force;
	}
}