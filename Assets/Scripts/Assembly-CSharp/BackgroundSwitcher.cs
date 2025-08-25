using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
	public Transform bg1;

	public Transform bg2;

	public ParticleSystemRenderer particleSystem1;

	public ParticleSystemRenderer particleSystem2;

	private Color color1;

	private Color color2;

	private SpriteRenderer[] spriteRenderer1;

	private SpriteRenderer[] spriteRenderer2;

	private float lerpValue;

	public Transform swtichPoint;

	public float switchXRange;

	public Transform target;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
