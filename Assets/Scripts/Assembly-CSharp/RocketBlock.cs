using UnityEngine;

public class RocketBlock : MonoBehaviour
{
	public enum RayType
	{
		mid = 0,
		side = 1,
		triple = 2
	}

	private int state;

	public Vector2 direction;

	public float speed;

	private float sign;

	public float backSpeedRate;

	public bool goBack;

	public bool camShake;

	public bool destoryOnWall;

	private BoxCollider2D col;

	private Vector2 originalPos;

	private Vector2 offset;

	private float shakeSize;

	private bool shaking;

	private int layerMask;

	private Vector2 normal;

	public RayType rayType;

	private Vector2 sideNormal;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void stopCallback()
	{
	}

	private void active(bool b)
	{
	}

	private void preActive(bool b)
	{
	}

	private void setState(int n)
	{
	}
}
