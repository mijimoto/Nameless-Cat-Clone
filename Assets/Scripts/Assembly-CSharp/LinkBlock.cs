using UnityEngine;

public class LinkBlock : Block
{
	private static GameObject link;

	private static GameObject linkBreakEffect;

	private Vector2 offset;

	private Rigidbody2D Rb;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
	}

	public override bool conditionCheck()
	{
		return false;
	}

	protected override void selectObject(bool b, Block lastObject = null)
	{
	}
}
