using System.Collections.Generic;
using UnityEngine;

public class Boss3SmokeCharger : MonoBehaviour
{
	public static Boss3SmokeCharger _instance;

	public static List<Boss3SmokeCharger> charger;

	public SpriteRenderer[] smokeSprite;

	public SpriteRenderer[] smokeFullSprite;

	public ParticleSystem[] smokePs;

	public float smokeChargeTime;

	public float targetAlpha;

	private Color color;

	private float smokeChargeTimer;

	private bool charging;

	public bool canCharge;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static void ableToCharge(bool b)
	{
	}

	public void reset()
	{
	}

	public void active(bool b)
	{
	}
}
