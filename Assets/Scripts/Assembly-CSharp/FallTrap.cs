using UnityEngine;

public class FallTrap : MonoBehaviour
{
	public GameObject breakEffect;

	public float acceleration;

	public bool shakeEffect;

	private float speed;

	private bool fall;

	protected ContactFilter2D contactFilter;

	private RaycastHit2D[] results;

	private bool skipped;

	private Rigidbody2D rb;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	private void active(bool b)
	{
	}
}
