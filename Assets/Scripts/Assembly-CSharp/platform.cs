using UnityEngine;

public class platform : Standable
{
    public enum PathWay
    {
        pingpon = 0,
        loop = 1,
        forward = 2
    }

    public float speed;
    public float slowRange;
    public float slowRate;
    public Vector2[] path;
    public PathWay pathType;
    private int pointIndex;
    private Vector2 oriPos;
    protected int sign;
    public bool move;
    public bool bulletBlock;

    public override void active(bool b)
    {
        base.active(b);
        move = b;
    }

    private void OnDrawGizmosSelected()
    {
        if (path == null || path.Length == 0)
            return;

        Vector3 startPos = transform.position;
        
        // Draw path points and connections
        Gizmos.color = Color.yellow;
        for (int i = 0; i < path.Length; i++)
        {
            Vector3 worldPos = startPos + (Vector3)path[i];
            Gizmos.DrawWireSphere(worldPos, 0.3f);
            
            if (i > 0)
            {
                Vector3 prevWorldPos = startPos + (Vector3)path[i - 1];
                Gizmos.DrawLine(prevWorldPos, worldPos);
            }
        }
        
        // Connect last point to first for loop type
        if (pathType == PathWay.loop && path.Length > 1)
        {
            Vector3 firstWorldPos = startPos + (Vector3)path[0];
            Vector3 lastWorldPos = startPos + (Vector3)path[path.Length - 1];
            Gizmos.color = Color.green;
            Gizmos.DrawLine(lastWorldPos, firstWorldPos);
        }
    }

    protected override void Start()
    {
        base.Start();

        // origin in physics space if rb is present
        if (rb != null)
            oriPos = rb.position;
        else
            oriPos = transform.position;

        pointIndex = 0;
        sign = 1;
        moveGoods = move;

        if (path != null && path.Length == 1)
        {
            move = false;
            pointIndex = 0;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (!move || path == null || path.Length == 0)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        // clamp index
        pointIndex = Mathf.Clamp(pointIndex, 0, path.Length - 1);

        Vector2 targetPos = oriPos + path[pointIndex];
        Vector2 currentPos = rb != null ? rb.position : (Vector2)transform.position;
        Vector2 toTarget = targetPos - currentPos;
        float distanceToTarget = toTarget.magnitude;
        Vector2 direction = toTarget.sqrMagnitude > 0f ? toTarget.normalized : Vector2.zero;

        // update sign for horizontal logic
        if (direction.x > 0.1f) sign = 1;
        else if (direction.x < -0.1f) sign = -1;

        // close enough -> snap and advance
        if (distanceToTarget < 0.01f)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.position = targetPos;
                transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
            }

            pointIndex = nextPoint(pointIndex);
            pointIndex = Mathf.Clamp(pointIndex, 0, path.Length - 1);
            return;
        }

        float moveSpeed = speed;
        if (slowRange > 0f && distanceToTarget <= slowRange)
            moveSpeed *= slowRate;

        Vector2 desiredVelocity = direction * moveSpeed;

        // Smooth velocity changes to avoid instant reversal kicks.
        if (rb != null)
        {
            // acceleration cap based on speed (adjust multiplier if you want snappier or smoother)
            float maxDelta = Mathf.Max(1f, speed * 4f) * Time.deltaTime;
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, desiredVelocity, maxDelta);
            // DO NOT set transform.position here — let physics update it in FixedUpdate
        }
        else
        {
            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.deltaTime);
            updatePos(new Vector3(newPos.x, newPos.y, transform.position.z));
        }
    }

    private int nextPoint(int currentIndex, bool inverse = false)
    {
        if (path == null || path.Length == 0)
            return 0;
        if (path.Length == 1)
            return 0;

        int nextIdx = currentIndex;
        switch (pathType)
        {
            case PathWay.pingpon:
                if (sign > 0) nextIdx = currentIndex + 1;
                else nextIdx = currentIndex - 1;

                if (nextIdx >= path.Length)
                {
                    nextIdx = path.Length - 2;
                    sign = -1;
                }
                else if (nextIdx < 0)
                {
                    nextIdx = 1;
                    sign = 1;
                }
                break;

            case PathWay.loop:
                nextIdx = (currentIndex + 1) % path.Length;
                break;

            case PathWay.forward:
                nextIdx = currentIndex + 1;
                if (nextIdx >= path.Length)
                {
                    nextIdx = path.Length - 1;
                    move = false;
                    if (rb != null) rb.linearVelocity = Vector2.zero;
                }
                break;
        }

        return Mathf.Clamp(nextIdx, 0, path.Length - 1);
    }

    private void updatePos(Vector3 pos)
    {
        if (rb != null)
        {
            rb.position = (Vector2)pos;
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            transform.position = pos;
        }
    }

    private void addPos(Vector3 pos)
    {
        if (rb != null)
        {
            rb.position = rb.position + (Vector2)pos;
            transform.position = new Vector3(transform.position.x + pos.x, transform.position.y + pos.y, transform.position.z);
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            transform.position += pos;
        }
    }
}