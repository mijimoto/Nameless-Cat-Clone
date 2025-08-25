using System;
using UnityEngine;

public class StaticsController : MonoBehaviour
{
	[SerializeField]
	private StaticsItemController playTimeItem;

	[SerializeField]
	private StaticsItemController deadTimeItem;

	[SerializeField]
	private StaticsItemController jumpCountItem;

	[SerializeField]
	private StaticsItemController moveDistanceItem;

	[SerializeField]
	private StaticsItemController tradeCountItem;

	[SerializeField]
	private StaticsItemController attachCountItem;

	private TimeSpan playTime;

	private string timeTextDay;

	private string timeTextHour;

	private string timeTextMin;

	private string timeTextSec;

	private string distanceText;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private string playTimeText()
	{
		return null;
	}

	public string GetPlayTimeText(TimeSpan playTime)
	{
		return null;
	}
}
