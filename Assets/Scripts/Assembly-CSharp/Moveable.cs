using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
	public static List<Moveable> moveList;

	public Standable attachedPlatform;

	protected bool touchPlayer;

	private List<GameObject> childList;

	public float yOffset;

	public bool ableToPush;

	private bool canAttach;

	private BoxCollider2D objCollider;

	private Rigidbody2D rb2d;

	public BoxCollider2D Collider => null;

	public Rigidbody2D Rb2d => null;

	private void Awake()
	{
	}

	public static void cleanUp()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
	}

	private void OnCollisionStay2D(Collision2D other)
	{
	}

	private void EnterGameObject(GameObject otherObj)
	{
	}

	private void unattach()
	{
	}

	private void unatttachChild(bool destroying = false)
	{
	}

	private void OnCollisionExit2D(Collision2D other)
	{
	}

	public void setCanAttach(bool b)
	{
	}

	private void OnDestroy()
	{
	}
}
