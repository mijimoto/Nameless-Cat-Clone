using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UVAnimation : MonoBehaviour
{
    private RawImage image;
    private Rect uvRect;

    [Header("Scroll Settings")]
    public float speedX = 0.1f;   // Horizontal speed
    public float speed = 0f;      // Vertical speed
    public Vector2 wh = new Vector2(1, 1); // Width/Height tiling
    public bool local = false;    // If true, resets each time on enable

    private void Awake()
    {
        image = GetComponent<RawImage>();
        uvRect = image.uvRect;

        // Apply tiling
        uvRect.width = wh.x;
        uvRect.height = wh.y;
        image.uvRect = uvRect;
    }

    private void OnEnable()
    {
        if (local)
        {
            uvRect.x = 0f;
            uvRect.y = 0f;
            image.uvRect = uvRect;
        }
    }

    private void Update()
    {
        // Move UV coordinates
        uvRect.x += speedX * Time.deltaTime;
        uvRect.y += speed * Time.deltaTime;

        // Apply to image
        image.uvRect = uvRect;
    }
}
