using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager _instance;
    private Transform player;
    public Vector3 initialPosition;
    private List<MoveByPlayer> bgs;
    public bool moveDelay;
    public bool isFollow;
    private float shakeLevel;
    private Vector3 NextPos;
    private FixCam fixCam;
    private float currentCamAngle;
    private float targetCamAngle;
    public static Vector2 border;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        bgs = new List<MoveByPlayer>();
        initialPosition = transform.position;
        NextPos = initialPosition;
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            isFollow = true;
        }
    }

    private void Update()
    {
        if (shakeLevel > 0)
        {
            shakeLevel -= Time.deltaTime * 2f;
            if (shakeLevel < 0) shakeLevel = 0;
        }

        // Smooth camera angle rotation
        if (Mathf.Abs(targetCamAngle - currentCamAngle) > 0.1f)
        {
            currentCamAngle = Mathf.LerpAngle(currentCamAngle, targetCamAngle, Time.deltaTime * 5f);
            transform.rotation = Quaternion.Euler(0, 0, currentCamAngle);
        }
    }

    private void FixedUpdate()
    {
           // Ensure player reference exists
    if (player == null)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            isFollow = true;
        }
    }

    updateCam();
    }

    public void updateCam()
    {
        Vector3 targetPos = NextPos;

        if (isFollow && player != null)
        {
            if (fixCam != null && fixCam.activing)
            {
                targetPos = fixCam.camPos();
            }
            else
            {
                targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
            }
        }

        // Apply camera shake
        if (shakeLevel > 0)
        {
            Vector3 shake = new Vector3(
                Random.Range(-shakeLevel, shakeLevel),
                Random.Range(-shakeLevel, shakeLevel),
                0
            );
            targetPos += shake;
        }

        // Apply border constraints
        if (border != Vector2.zero)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, -border.x, border.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -border.y, border.y);
        }

        if (moveDelay)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * 3f);
        }
        else
        {
            transform.position = targetPos;
        }

        NextPos = targetPos;
    }

    public void CamShake(float level = 0.1f, Transform source = null)
    {
        shakeLevel = level;
        if (source != null)
        {
            float distance = Vector3.Distance(transform.position, source.position);
            shakeLevel *= Mathf.Clamp01(1f / (distance + 1f));
        }
    }

    public void setFixCam(FixCam fc, bool active = true)
    {
        if (active)
        {
            fixCam = fc;
            if (fc != null)
            {
                fc.activing = true;
            }
        }
        else
        {
            if (fixCam != null)
            {
                fixCam.activing = false;
            }
            fixCam = null;
        }
    }

    public void rotateCam(float z, bool instanceUpdate = false)
    {
        targetCamAngle = z;
        if (instanceUpdate)
        {
            currentCamAngle = z;
            transform.rotation = Quaternion.Euler(0, 0, z);
        }
    }

    public bool camReady()
    {
        return Vector3.Distance(transform.position, NextPos) < 0.1f;
    }

    public void addBg(MoveByPlayer bg)
    {
        if (bg != null && !bgs.Contains(bg))
        {
            bgs.Add(bg);
        }
    }

    public Vector2 getBorder()
    {
        return border;
    }

    public void moveCam(Vector2 offset)
    {
        NextPos += new Vector3(offset.x, offset.y, 0);
    }

    public void changeFollowTarget(Transform transform)
    {
        player = transform;
        isFollow = (player != null);
    }
}