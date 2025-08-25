using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryStar : MonoBehaviour
{
	private class StarManager
	{
		public bool destroyObj;

		public bool starCreated;

		public bool playSound;

		public List<Star> stars;

		public List<Star> removeStars;

		public void updateStars(Vector2 playerPos)
		{
		}

		public void forceUpdateAngle(Vector2 playerPos)
		{
		}
	}

	private class Star
	{
		public StarManager starManager;

		public Transform starTrans;

		private Transform childTrans;

		public Vector2 speed;

		private float acceleration;

		private float delayTimer;

		public Star(StarManager sm, GameObject memoryStar, Vector2 driection, Vector2 speedRange, Vector2 angleRange, float delayTime)
		{
		}

		private void updateAngle()
		{
		}

		public void updatePos(Vector2 playerPos)
		{
		}
	}

	public GameObject memoryPrefab;

	public bool goNextStage;

	public string storyId;

	public GameObject callBackObj;

	public Vector2 angleRange;

	public Vector2 speedRange;

	public bool playSound;

	public Transform target;

	public Transform childMemory;

	public bool useChildMemory;

	public float timeOutTime;

	private bool goingToChangeScene;

	private StarManager starManager;

	private float timeOutTimer;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	private IEnumerator changeScene()
	{
		return null;
	}

	public void collectMemory(int amount = -1)
	{
	}

	public void collectMemory(int amount, float deltaTime = 0f, bool investDirection = false)
	{
	}
}
