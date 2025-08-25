using UnityEngine;

public class Boss3Cannon : Block
{
	public static Boss3Cannon _instance;

	private int charge;

	private Animator animator;

	private bool activing;

	private float shakeTime;

	private Vector3 initialPos;

	public Transform ball;

	public AttachBtn[] attachBtns;

	public Boss3Hitter hitEffect;

	private bool shooting;

	public float groundHeight;

	public float maxHeight;

	public Transform godTrans;

	public float shootSpeed;

	private int shootingStep;

	private float ScaleTimer;

	private float endTimer;

	private bool waitForStory;

	protected override void Awake()
	{
	}

	private void Update()
	{
	}

	public void startShoot()
	{
	}

	private void endShoot()
	{
	}

	public override bool conditionCheck()
	{
		return false;
	}

	protected override void selectObject(bool b, Block lastObject = null)
	{
	}

	public void chagreIt()
	{
	}

	public void resetCannon()
	{
	}

	public void reset()
	{
	}

	public void resetPrepare()
	{
	}

	public void waitStoryBeforeShoot()
	{
	}

	public void resetPos()
	{
	}

	public void showAttachBtn(bool show)
	{
	}
}
