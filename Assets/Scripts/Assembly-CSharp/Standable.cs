using System.Collections.Generic;
using UnityEngine;

public class Standable : MonoBehaviour
{
    private float myColliderOffset;
    private float myColliderHeight;
    public bool jumpOver;
    protected Rigidbody2D rb;
    private Collider2D myCollider;
    protected Vector2 targetVelocity;
    public bool moveGoods;
    private Vector2 lastPos;
    private bool activing;
    private List<GameObject> childList;
    private List<Rigidbody2D> childRigList;
    public bool canUseActive;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        childList = new List<GameObject>();
        childRigList = new List<Rigidbody2D>();

        if (myCollider != null)
        {
            myColliderOffset = myCollider.bounds.center.y - myCollider.bounds.min.y;
            myColliderHeight = myCollider.bounds.size.y;
        }

        lastPos = transform.position;
    }

    protected virtual void Update()
    {
        // placeholder for derived classes
    }

    protected void FixedUpdate()
    {
        // Handle movement for child objects attached to moving platforms
        if (moveGoods && childList.Count > 0)
        {
            Vector2 deltaMovement = (Vector2)transform.position - lastPos;

            // Move all attached children with the platform
            for (int i = 0; i < childList.Count; i++)
            {
                if (childList[i] != null && childRigList[i] != null)
                {
                    // Use MovePosition for smooth movement without jitter
                    childRigList[i].MovePosition(childRigList[i].position + deltaMovement);
                }
            }
        }

        lastPos = transform.position;
    }

    public bool onAbove(Rigidbody2D targetRigidbody, BoxCollider2D collider, float offset = 0f)
    {
        if (targetRigidbody == null || collider == null || myCollider == null)
            return false;

        // Check if the target is above this platform
        float platformTop = myCollider.bounds.max.y;
        float targetBottom = collider.bounds.min.y;

        // Allow small tolerance for floating point precision
        return targetBottom >= (platformTop - offset - 0.1f);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        Rigidbody2D playerRb = other.rigidbody;
        BoxCollider2D playerCollider = other.collider as BoxCollider2D;

        if (playerRb == null || playerCollider == null) return;

        // Check if this platform has a Platform Effector 2D
        PlatformEffector2D platformEffector = GetComponent<PlatformEffector2D>();

        if (platformEffector != null)
        {
            // Platform Effector is present - examine contact normals to ensure player is on top
            bool hasValidContact = false;
            ContactPoint2D[] contacts = other.contacts;

            for (int i = 0; i < contacts.Length; i++)
            {
                // contact normal points from this collider to the other; when player is above platform,
                // contact.normal.y will be negative (pointing down from player into platform), so check magnitude
                if (contacts[i].normal.y < -0.7f)
                {
                    hasValidContact = true;
                    break;
                }
            }

            if (hasValidContact && moveGoods)
            {
                EnterGameObject(other.gameObject);
            }

            return; // let PlatformEffector handle collisions itself
        }

        // Handle jumpOver platforms WITHOUT Platform Effector
        if (jumpOver)
        {
            // Check player's velocity and position
            bool movingUp = playerRb.linearVelocity.y > 0.1f;
            bool comingFromBelow = playerRb.position.y < myCollider.bounds.center.y;

            if (movingUp && comingFromBelow)
            {
                // Player is jumping from below - temporarily ignore collision
                Physics2D.IgnoreCollision(other.collider, myCollider, true);
                StartCoroutine(ReEnableCollisionAfterDelay(other.collider, 0.3f));
                return;
            }

            // Check if player is landing on platform from above
            if (onAbove(playerRb, playerCollider, 0.2f))
            {
                EnterGameObject(other.gameObject);
            }
            else
            {
                // Player hitting from side or wrong angle - ignore collision briefly
                Physics2D.IgnoreCollision(other.collider, myCollider, true);
                StartCoroutine(ReEnableCollisionAfterDelay(other.collider, 0.1f));
            }
        }
        else
        {
            // Normal solid platform - always allow collision and attach if needed
            EnterGameObject(other.gameObject);
        }
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        Rigidbody2D playerRb = other.rigidbody;
        BoxCollider2D playerCollider = other.collider as BoxCollider2D;

        if (playerRb == null || playerCollider == null) return;

        PlatformEffector2D platformEffector = GetComponent<PlatformEffector2D>();

        if (platformEffector != null)
        {
            ContactPoint2D[] contacts = other.contacts;
            bool playerStandingOn = false;

            for (int i = 0; i < contacts.Length; i++)
            {
                if (contacts[i].normal.y < -0.7f) // player above platform
                {
                    playerStandingOn = true;
                    break;
                }
            }

            if (playerStandingOn && moveGoods && !childList.Contains(other.gameObject))
            {
                EnterGameObject(other.gameObject);
            }
            else if (!playerStandingOn && moveGoods)
            {
                ExitGameObject(other.gameObject);
            }

            return;
        }

        if (jumpOver)
        {
            if (onAbove(playerRb, playerCollider, 0.2f))
            {
                if (!childList.Contains(other.gameObject))
                {
                    EnterGameObject(other.gameObject);
                }
            }
            else
            {
                ExitGameObject(other.gameObject);
                Physics2D.IgnoreCollision(other.collider, myCollider, true);
                StartCoroutine(ReEnableCollisionAfterDelay(other.collider, 0.1f));
            }
        }
        else
        {
            if (!childList.Contains(other.gameObject))
            {
                EnterGameObject(other.gameObject);
            }
        }
    }

    private System.Collections.IEnumerator ReEnableCollisionAfterDelay(Collider2D otherCollider, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (otherCollider != null && myCollider != null)
        {
            Physics2D.IgnoreCollision(otherCollider, myCollider, false);
        }
    }

    public void EnterGameObject(GameObject otherObj)
    {
        if (otherObj.CompareTag("Player"))
        {
            AddToChild(otherObj);
        }
    }

    private void AddToChild(GameObject targetGameObj)
    {
        if (targetGameObj == null || childList.Contains(targetGameObj))
            return;

        Rigidbody2D targetRb = targetGameObj.GetComponent<Rigidbody2D>();
        if (targetRb != null)
        {
            childList.Add(targetGameObj);
            childRigList.Add(targetRb);
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ExitGameObject(other.gameObject);
        }
    }

    public void ExitGameObject(GameObject otherObj)
    {
        if (otherObj.CompareTag("Player"))
        {
            unattachChild(otherObj);
        }
    }

    private void unattachChild(GameObject other)
    {
        if (other == null || !childList.Contains(other))
            return;

        int index = childList.IndexOf(other);
        if (index >= 0 && index < childRigList.Count)
        {
            childList.RemoveAt(index);
            childRigList.RemoveAt(index);
        }
    }

    public void UnattachAll()
    {
        childList.Clear();
        childRigList.Clear();
    }

    public virtual void active(bool b)
    {
        activing = b;
        gameObject.SetActive(b);
    }

    public void forcActiveStandable(bool b)
    {
        if (canUseActive)
        {
            active(b);
        }
    }
}
