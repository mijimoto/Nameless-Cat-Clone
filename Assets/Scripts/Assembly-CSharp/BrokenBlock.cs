using UnityEngine;

public class BrokenBlock : MonoBehaviour
{
	public enum BrokenState
	{
		idle = 0,
		breaking = 1,
		broken = 2
	}

	public BrokenState state;

	private Transform spriteObj;

	public float breakTime;

	private float breakTimer;

	public float recoverTime;

	private BoxCollider2D boxCollider2D;

	private Animator ani;

	private void Start()
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
}
