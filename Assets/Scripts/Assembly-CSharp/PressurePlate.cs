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
		sr = GetComponent<SpriteRenderer>();
		inTrigger = new List<GameObject>();
		pressDown = false;
		noMoreActive = false;
		closeTimer = 0f;
	}

	private void Update()
	{
		if (activeType == ActiveType.time && pressDown && closeTime > 0)
		{
			closeTimer += Time.deltaTime;
			if (closeTimer >= closeTime)
			{
				press(false);
				closeTimer = 0f;
			}
		}

		if (triggerMode && inTrigger.Count > 0)
		{
			bool shouldPress = false;
			foreach (GameObject obj in inTrigger)
			{
				if (obj != null && (!playerOnly || obj.CompareTag("Player")))
				{
					shouldPress = true;
					break;
				}
			}
			if (shouldPress != pressDown)
			{
				press(shouldPress);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (noMoreActive) return;
		
		if (!playerOnly || other.CompareTag("Player"))
		{
			if (triggerMode)
			{
				if (!inTrigger.Contains(other.gameObject))
					inTrigger.Add(other.gameObject);
			}
			else
			{
				trigger();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (triggerMode && inTrigger.Contains(other.gameObject))
		{
			inTrigger.Remove(other.gameObject);
		}
	}

	public void trigger()
	{
		if (noMoreActive) return;
		
		if (activeType == ActiveType.delay)
		{
			Invoke("press", 0.5f);
		}
		else
		{
			press(true);
		}
	}

	public void press(bool b)
{
    if (noMoreActive && b) return; // One-time plates cannot activate again

    pressDown = b;

    // Change sprite if assigned
    if (sprites.Length >= 2 && sr != null)
    {
        sr.sprite = sprites[b ? 1 : 0];
    }

    // Play sound if needed
    if (playSound && b)
    {
        // Example: AudioSource.PlayClipAtPoint(pressSound, transform.position);
    }

    // Activate or deactivate all target GameObjects
    foreach (GameObject target in activeTarget)
    {
        if (target != null)
        {
            target.SetActive(b);
        }
    }

    // If it's a one-time pressure plate, prevent future activation
    if (oneTime && b)
    {
        noMoreActive = true;
    }

    // Reset timer if this is a timed plate
    if (activeType == ActiveType.time && b)
    {
        closeTimer = 0f;
    }
}


	public void reable()
	{
		noMoreActive = false;
	}

	public void disable()
	{
		noMoreActive = true;
	}

	public float getTimerRate()
	{
		if (activeType == ActiveType.time && closeTime > 0)
		{
			return closeTimer / closeTime;
		}
		return 0f;
	}
}