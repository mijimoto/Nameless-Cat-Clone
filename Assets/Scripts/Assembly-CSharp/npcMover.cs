using UnityEngine;

public class npcMover : MonoBehaviour
{
	public float waitTime;

	private float timer;

	public float moveTime;

	public float jumpHeight;

	private bool waiting;

	public Vector2 range;

	private float targetX;

	private Vector2 basePos;

	private Animator animator;

	private SpriteRenderer spriteRenderer;

	private Vector2 speed;

	private float baseYSpeed;

	private float a;

	private bool canMove;

	private void OnDrawGizmosSelected()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void activeNPC(bool b)
	{
	}
}
