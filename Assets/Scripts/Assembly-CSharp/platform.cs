using UnityEngine;

public class platform : Standable
{
	public enum PathWay
	{
		pingpon = 0,
		loop = 1,
		forward = 2
	}

	public float speed;

	public float slowRange;

	public float slowRate;

	public Vector2[] path;

	public PathWay pathType;

	private int pointIndex;

	private Vector2 oriPos;

	protected int sign;

	public bool move;

	public bool bulletBlock;

	public override void active(bool b)
	{
	}

	private void OnDrawGizmosSelected()
	{
	}

	protected override void Start()
	{
	}

	protected override void Update()
	{
	}

	private int nextPoint(int pointIndex, bool inverse = false)
	{
		return 0;
	}

	private void updatePos(Vector3 pos)
	{
	}

	private void addPos(Vector3 pos)
	{
	}
}
