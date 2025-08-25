using UnityEngine;

public class FixCam : MonoBehaviour
{
	public float offsetRate;

	public Vector2 camCenter;

	public bool activing;

	public bool cannotLeave;

	private BoxCollider2D boxCollider2D;

	private void OnDrawGizmosSelected()
	{
	}

	private void Start()
	{
	}

	public Vector3 camPos()
	{
		return default(Vector3);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	private void OnTriggerExit2D(Collider2D other)
	{
	}

	public void active(bool b)
	{
	}
}
