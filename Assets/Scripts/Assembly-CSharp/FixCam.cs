
using UnityEngine;

public class FixCam : MonoBehaviour
{
    public float offsetRate;
    public Vector2 camCenter;
    public bool activing;
    public bool cannotLeave;
    private BoxCollider2D boxCollider2D;

    private void OnDrawGizmosSelected()
    {
        if (boxCollider2D == null)
            boxCollider2D = GetComponent<BoxCollider2D>();
        
        if (boxCollider2D != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, boxCollider2D.size);
        }
    }

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D == null)
        {
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;
        }
    }

    public Vector3 camPos()
    {
        Vector3 center = new Vector3(camCenter.x, camCenter.y, Camera.main.transform.position.z);
        return Vector3.Lerp(Camera.main.transform.position, center, offsetRate);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CameraManager._instance != null)
            {
                CameraManager._instance.setFixCam(this, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !cannotLeave)
        {
            if (CameraManager._instance != null)
            {
                CameraManager._instance.setFixCam(null, false);
            }
        }
    }

    public void active(bool b)
    {
        activing = b;
        if (boxCollider2D != null)
        {
            boxCollider2D.enabled = b;
        }
    }
}