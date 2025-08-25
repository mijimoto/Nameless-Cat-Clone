using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	public static List<Block> blockList;

	public static Block target;

	public static Block selectedObject;

	public Standable attachedPlatform;

	protected bool touchPlayer;

	protected bool blockEnabled;

	public bool canBeTrade;

	private List<GameObject> childList;

	private BoxCollider2D objCollider;

	private Rigidbody2D rb2d;

	private bool isBlock;

	public BoxCollider2D Collider => null;

	public Rigidbody2D Rb2d => null;

	public static void cleanUp()
	{
	}

	protected virtual void Awake()
	{
	}

	protected virtual void selectObject(bool b, Block lastObject = null)
	{
	}

	public virtual bool conditionCheck()
	{
		return false;
	}

	public void clickOn()
	{
	}

	public static void unSelect()
	{
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
	}

	private void OnCollisionStay2D(Collision2D other)
	{
	}

	private void OnCollisionExit2D(Collision2D other)
	{
	}

	public void unattach()
	{
	}

	private void FixedUpdate()
	{
	}
}
