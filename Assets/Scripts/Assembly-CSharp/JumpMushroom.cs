using UnityEngine;

public class JumpMushroom : MonoBehaviour
{
	public float force;

	public bool fixDirection;

	public bool useDegree;

	public float degree;

	private Vector2 direction;

	private Animator ani;

	private bool entered;

	private void OnDrawGizmosSelected()
	{
	}

	private void Start()
	{
	}

	private void OnTriggerStay2D(Collider2D col)
	{
	}

	private void OnTriggerExit2D(Collider2D col)
	{
	}

	public static Vector2 RadianToVector2(float radian)
	{
		return default(Vector2);
	}

	public static Vector2 DegreeToVector2(float degree)
	{
		return default(Vector2);
	}
}
