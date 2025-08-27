using SpriteGlow;
using UnityEngine;

public class playerShadow : MonoBehaviour
{
	public static playerShadow _instance;
	private global::SpriteGlow.SpriteGlow sg;
	private SpriteRenderer sr;
	private float speed;

private void Awake()
{
    _instance = this;
    sg = GetComponent<SpriteGlow.SpriteGlow>();
    sr = GetComponent<SpriteRenderer>();

    // Ensure shadow is behind the player
    if (sr != null)
    {
        sr.sortingLayerName = "Default"; // or whatever your player is
        sr.sortingOrder = PlayerPlatformerController._instance.GetComponent<SpriteRenderer>().sortingOrder - 1;
    }
}


	private void Update()
	{
		if (PlayerPlatformerController._instance != null)
		{
float lagDistance = 0.2f; // distance shadow lags behind
transform.position = Vector3.MoveTowards(transform.position, PlayerPlatformerController._instance.transform.position, lagDistance);
 // Match player flip
        sr.flipX = PlayerPlatformerController._instance.SpriteRenderer.flipX;
        }
	}

	public void Active(Sprite sprite, bool flipX)
	{
		if (sr != null)
		{
			sr.sprite = sprite;
			sr.flipX = flipX;
		}

		if (sg != null)
		{
			sg.enabled = true;
		}

		gameObject.SetActive(true);
	}
}