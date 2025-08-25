using UnityEngine;

public class AttachBlock : Block
{
	private static ParticleSystem attachEffect;

	public Sprite[] sprites;

	public bool storeSpeed;

	private Vector2 playerStoredSpeed;

	public Vector2 placeOffset;

	private SpriteRenderer sr;

	private Animator animator;

	public float recoverTime;

	private float recoverTimer;

	public bool disableSpriteUpdate;

	private bool isActive;

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

	public void activeBlock(int isActive)
	{
	}

	public void activeBlock(bool isActive)
	{
	}

	protected override void selectObject(bool b, Block lastObject = null)
	{
	}
}
