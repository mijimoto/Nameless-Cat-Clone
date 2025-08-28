using UnityEngine;

public class PixelSnap : MonoBehaviour
{
    private Sprite sprite;
    private Vector3 actualPosition;
    private bool shouldRestorePosition;

    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            sprite = spriteRenderer.sprite;
        }
        actualPosition = transform.position;
    }

    private void OnWillRenderObject()
    {
        if (sprite == null) return;

        // Store the actual position before snapping
        actualPosition = transform.position;
        shouldRestorePosition = true;

        // Get the camera that's about to render this object
        Camera renderingCamera = Camera.current;
        if (renderingCamera == null) return;

        // Calculate pixels per unit based on camera's orthographic size and screen height
        float pixelsPerUnit = Screen.height / (2f * renderingCamera.orthographicSize);
        
        // If we have a PixelPerfectCamera component, use its pixels per unit instead
        PixelPerfectCamera pixelPerfectCamera = renderingCamera.GetComponent<PixelPerfectCamera>();
        if (pixelPerfectCamera != null && pixelPerfectCamera.isInitialized)
        {
            pixelsPerUnit = pixelPerfectCamera.cameraPixelsPerUnit;
        }

        // Snap position to pixel grid
        Vector3 snappedPosition = actualPosition;
        snappedPosition.x = Mathf.Round(actualPosition.x * pixelsPerUnit) / pixelsPerUnit;
        snappedPosition.y = Mathf.Round(actualPosition.y * pixelsPerUnit) / pixelsPerUnit;

        // Apply the snapped position
        transform.position = snappedPosition;
    }

    private void OnRenderObject()
    {
        // Restore the original position after rendering
        if (shouldRestorePosition)
        {
            transform.position = actualPosition;
            shouldRestorePosition = false;
        }
    }
}