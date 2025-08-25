using UnityEngine;
using UnityEngine.EventSystems;

public class AdjustDrapper : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler
{
	public RectTransform buttonRectTrans;

	public string adjustId;

	public Vector2 posRange;

	private Vector2 startPos;

	private Vector2 buttonStartPos;

	private Vector2 origialXPos;

	public void OnBeginDrag(PointerEventData eventData)
	{
	}

	public void OnDrag(PointerEventData eventData)
	{
	}

	public void Init()
	{
	}

	public void Start()
	{
	}

	public void End()
	{
	}

	public void Reset()
	{
	}
}
