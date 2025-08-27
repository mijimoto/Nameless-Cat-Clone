using SpriteGlow;
using UnityEngine;

public class playerShadow : MonoBehaviour
{
    public static playerShadow _instance;
    private global::SpriteGlow.SpriteGlow sg;
    private SpriteRenderer sr;
    private float speed;
    private void Start()
    {
        // Ensure shadow is behind the player
        if (sr != null && PlayerPlatformerController._instance != null)
        {
            sr.sortingLayerName = "Default";
            sr.sortingOrder = PlayerPlatformerController._instance.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
    }

    private void Awake()
    {
        _instance = this;
        sg = GetComponent<SpriteGlow.SpriteGlow>();
        sr = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        if (PlayerPlatformerController._instance != null)
        {
            // Get player velocity magnitude
            float playerSpeed = PlayerPlatformerController._instance.Rb2d.linearVelocity.magnitude;

            // Calculate lag distance proportional to speed
            float lagDistance = 0.1f + playerSpeed * 0.05f; // tweak multiplier for effect

            // Move shadow toward player with calculated lag
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