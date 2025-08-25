using UnityEngine;

public class ShooterBlock : MonoBehaviour
{
	public enum ShooterState
	{
		idle = 0,
		shooting = 1
	}

	public Transform handHead;

	public Transform handBody;

	public Transform pushBlock;

	private Rigidbody2D rb;

	public Sprite[] shooterSprites;

	private SliderJoint2D sj;

	private JointTranslationLimits2D limits2D;

	private JointMotor2D jm;

	private float jmSpeed;

	private SpriteRenderer sr;

	private PressurePlate pp;

	private int sign;

	public float mass;

	private float oriMax;

	private ShooterState state;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void UpdateIdle(float distance)
	{
	}

	private void UpdateShooting(float distance)
	{
	}

	public void setPush(Transform m)
	{
	}
}
