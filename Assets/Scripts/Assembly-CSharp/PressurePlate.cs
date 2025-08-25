using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
	public enum ActiveType
	{
		normal = 0,
		oneTime = 1,
		time = 2,
		delay = 3
	}

	public Sprite[] sprites;

	public ActiveType activeType;

	public bool pressDown;

	public bool oneTime;

	private bool noMoreActive;

	public bool playSound;

	public float closeTime;

	private float closeTimer;

	public bool playerOnly;

	public bool triggerMode;

	private List<GameObject> inTrigger;

	private SpriteRenderer sr;

	public GameObject[] activeTarget;

	private void OnEnable()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	private void OnTriggerExit2D(Collider2D other)
	{
	}

	public void trigger()
	{
	}

	public void press(bool b)
	{
	}

	public void reable()
	{
	}

	public void disable()
	{
	}

	public float getTimerRate()
	{
		return 0f;
	}
}
