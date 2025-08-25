using System.Collections.Generic;
using UnityEngine;

public class Standable : MonoBehaviour
{
	private float myColliderOffset;

	private float myColliderHeight;

	public bool jumpOver;

	protected Rigidbody2D rb;

	private Collider2D myCollider;

	protected Vector2 targetVelocity;

	public bool moveGoods;

	private Vector2 lastPos;

	private bool activing;

	private List<GameObject> childList;

	private List<Rigidbody2D> childRigList;

	public bool canUseActive;

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	protected void FixedUpdate()
	{
	}

	public bool onAbove(Rigidbody2D targetRigidbody, BoxCollider2D collider, float offset = 0f)
	{
		return false;
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
	}

	public void OnCollisionStay2D(Collision2D other)
	{
	}

	public void EnterGameObject(GameObject otherObj)
	{
	}

	private void AddToChild(GameObject targetGameObj)
	{
	}

	public void OnCollisionExit2D(Collision2D other)
	{
	}

	public void ExitGameObject(GameObject otherObj)
	{
	}

	private void unattachChild(GameObject other)
	{
	}

	public void UnattachAll()
	{
	}

	public virtual void active(bool b)
	{
	}

	public void forcActiveStandable(bool b)
	{
	}
}
