using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager _instance;

	private RuntimePlatform platform;

	public static InputTarget selectedTarget;

	private Vector2 touchPos;

	private ArrayList lastPos;

	public static float dragSpeed;

	public static float uncontrolDragSpeed;

	public static float moveRate;

	public static bool canDrag;

	public static bool holdDown;

	private Camera mainCamera;
	private bool isDragging = false;

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
			return;
		}

		platform = Application.platform;
		mainCamera = Camera.main;
		if (mainCamera == null)
			mainCamera = FindObjectOfType<Camera>();

		lastPos = new ArrayList();
	}

	private void Update()
	{
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
		{
			HandleTouchInput();
		}
		else
		{
			HandleMouseInput();
		}
	}

	private void HandleTouchInput()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			touchPos = touch.position;

			switch (touch.phase)
			{
				case TouchPhase.Began:
					Vector2 worldPos = mainCamera.ScreenToWorldPoint(touchPos);
					Transform target = checkTouch(worldPos);
					if (target != null)
					{
						clickEvent(worldPos, target);
					}
					break;

				case TouchPhase.Moved:
					if (selectedTarget != null && canDrag)
					{
						dragEvent();
					}
					break;

				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					ClickReleaseEventTouch();
					break;
			}
		}
	}

	private void HandleMouseInput()
	{
		touchPos = Input.mousePosition;

		if (Input.GetMouseButtonDown(0))
		{
			Vector2 worldPos = mainCamera.ScreenToWorldPoint(touchPos);
			Transform target = checkTouch(worldPos);
			if (target != null)
			{
				clickEvent(worldPos, target);
			}
		}

		if (Input.GetMouseButton(0) && selectedTarget != null && canDrag)
		{
			dragEventMouse();
		}

		if (Input.GetMouseButtonUp(0))
		{
			ClickReleaseEventMouse();
		}
	}

	private void ClickReleaseEventTouch()
	{
		holdDown = false;
		isDragging = false;
		releaseEvent();
	}

	private void ClickReleaseEventMouse()
	{
		holdDown = false;
		isDragging = false;
		releaseEvent();
	}

	private void DragSelectedTarget(Vector2 inputPos)
	{
		if (selectedTarget != null && canDrag)
		{
			Vector2 worldPos = mainCamera.ScreenToWorldPoint(inputPos);
			selectedTarget.OnDrag(worldPos);

			// Store last position for drag calculation
			if (lastPos.Count > 10)
			{
				lastPos.RemoveAt(0);
			}
			lastPos.Add(worldPos);
		}
	}

	private Vector2 ConvertToScreenPos(Vector2 pos)
	{
		return mainCamera.WorldToScreenPoint(pos);
	}

	private Transform checkTouch(Vector2 worldPos)
	{
		// Cast a ray from camera to world position
		RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

		if (hit.collider != null)
		{
			InputTarget target = hit.collider.GetComponent<InputTarget>();
			if (target != null)
			{
				return hit.transform;
			}
		}

		return null;
	}

	private void clickEvent(Vector2 position, Transform target)
	{
		InputTarget inputTarget = target.GetComponent<InputTarget>();
		if (inputTarget != null)
		{
			selectedTarget = inputTarget;
			holdDown = true;
			isDragging = false;

			selectedTarget.OnClick(position);

			// Initialize last position array
			lastPos.Clear();
			lastPos.Add(position);
		}
	}

	private void releaseEvent()
	{
		if (selectedTarget != null)
		{
			selectedTarget.OnRelease();
			selectedTarget = null;
		}

		lastPos.Clear();
	}

	private void dragEvent()
	{
		if (selectedTarget != null && holdDown)
		{
			isDragging = true;
			Vector2 worldPos = mainCamera.ScreenToWorldPoint(touchPos);
			DragSelectedTarget(touchPos);
		}
	}

	private void dragEventMouse()
	{
		if (selectedTarget != null && holdDown)
		{
			isDragging = true;
			DragSelectedTarget(touchPos);
		}
	}
}