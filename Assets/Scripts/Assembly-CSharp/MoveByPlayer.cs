using UnityEngine;

public class MoveByPlayer : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector2 offset;

    [Header("Movement Rates")]
    public float rate = 0.5f;     // X-axis parallax rate
    public float rateY = 0.5f;    // Y-axis parallax rate

    [Header("Custom Position")]
    public bool initFromCustomPos = false;
    public Vector3 customPos;

    private void Start()
    {
        // Initialize starting position
        if (initFromCustomPos)
        {
            initialPosition = customPos;
            transform.position = initialPosition;
        }
        else
        {
            initialPosition = transform.position;
        }
    }

    private void Update()
    {
        // Apply parallax offset
        Vector3 newPos = initialPosition;
        newPos.x += offset.x * rate;
        newPos.y += offset.y * rateY;
        transform.position = newPos;
    }

    public void updatePosition(float x, float y)
    {
        // Called by external script (player/camera movement)
        offset.x = x;
        offset.y = y;
    }

    public void addToInitialPos(Vector2 extraOffset)
    {
        // Adjust initial base position (e.g. scene transitions)
        initialPosition += new Vector3(extraOffset.x, extraOffset.y, 0f);
    }
}
