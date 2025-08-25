using UnityEngine;

public class Disappear : MonoBehaviour
{
	public enum DisappearState
	{
		disappearing = 0,
		recovering = 1
	}

	public float disappearTime;

	public float recoverTime;

	public float timer;

	public DisappearState startState;

	private BoxCollider2D boxCollider2D;

	private SpriteRenderer spriteRen;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
