using UnityEngine;

public class PlayerLooper : MonoBehaviour
{
	public Vector2 borderAndOffset;

	private void Start()
	{
		// Initialize if needed
	}

	private void Update()
	{
		Vector3 pos = transform.position;
		
		if (pos.x > borderAndOffset.x + borderAndOffset.y)
		{
			pos.x = -borderAndOffset.x - borderAndOffset.y;
			transform.position = pos;
		}
		else if (pos.x < -borderAndOffset.x - borderAndOffset.y)
		{
			pos.x = borderAndOffset.x + borderAndOffset.y;
			transform.position = pos;
		}
	}
}
