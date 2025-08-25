using UnityEngine;

public class WindArea : MonoBehaviour
{
	public Vector2 driection;

	public float speed;

	public float blockMultiply;

	public float maxSpeed;

	public Animator animator;

	private float aniSpeed;

	private bool activing;

	public bool defaultActive;

	public Moveable root;

	private ParticleSystem windParicleSystem;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void active(bool b)
	{
	}

	private void OnTriggerStay2D(Collider2D other)
	{
	}
}
