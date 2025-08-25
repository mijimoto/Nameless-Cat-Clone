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
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
	}

	public void updateCam()
	{
	}

	public void CamShake(float level = 0.1f, Transform source = null)
	{
	}

	public void setFixCam(FixCam fc, bool active = true)
	{
	}

	public void rotateCam(float z, bool instanceUpdate = false)
	{
	}

	public bool camReady()
	{
		return false;
	}

	public void addBg(MoveByPlayer bg)
	{
	}

	public Vector2 getBorder()
	{
		return default(Vector2);
	}

	public void moveCam(Vector2 offset)
	{
	}

	public void changeFollowTarget(Transform transform)
	{
	}
}
