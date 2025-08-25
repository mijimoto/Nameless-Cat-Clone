using UnityEngine;

public class FanBlock : Block
{
	public bool active;

	public bool canSelect;

	private bool activing;

	private WindArea windArea;

	private ParticleSystem windEffect;

	private void Start()
	{
	}

	private void Update()
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
