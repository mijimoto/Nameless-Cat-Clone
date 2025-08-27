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
			if (removeStars == null) removeStars = new List<Star>();
			
			foreach (Star star in stars)
			{
				star.updatePos(playerPos);
				if (star.starTrans == null)
				{
					removeStars.Add(star);
				}
			}
			
			foreach (Star removeStar in removeStars)
			{
				stars.Remove(removeStar);
			}
			removeStars.Clear();
			
			if (stars.Count == 0)
			{
				destroyObj = true;
			}
		}

		public void forceUpdateAngle(Vector2 playerPos)
		{
			foreach (Star star in stars)
			{
				star.updatePos(playerPos);
			}
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

		public Star(StarManager sm, GameObject memoryStar, Vector2 direction, Vector2 speedRange, Vector2 angleRange, float delayTime)
		{
			starManager = sm;
			starTrans = Object.Instantiate(memoryStar).transform;
			childTrans = starTrans.GetChild(0);
			
			float angle = Random.Range(angleRange.x, angleRange.y) * Mathf.Deg2Rad;
			float speedMag = Random.Range(speedRange.x, speedRange.y);
			speed = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speedMag;
			
			acceleration = 0.98f;
			delayTimer = delayTime;
		}

		private void updateAngle()
		{
			if (childTrans != null)
			{
				childTrans.Rotate(0, 0, 5f * Time.fixedDeltaTime * 60f);
			}
		}

		public void updatePos(Vector2 playerPos)
		{
			if (starTrans == null) return;
			
			delayTimer -= Time.fixedDeltaTime;
			if (delayTimer > 0) return;
			
			updateAngle();
			
			Vector2 direction = (playerPos - (Vector2)starTrans.position).normalized;
			speed = Vector2.Lerp(speed, direction * 5f, Time.fixedDeltaTime * 2f);
			speed *= acceleration;
			
			starTrans.position += (Vector3)speed * Time.fixedDeltaTime;
			
			if (Vector2.Distance(starTrans.position, playerPos) < 0.5f)
			{
				Object.Destroy(starTrans.gameObject);
				starTrans = null;
			}
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
		starManager = new StarManager();
		starManager.stars = new List<Star>();
		starManager.removeStars = new List<Star>();
		timeOutTimer = 0f;
		goingToChangeScene = false;
		
		if (angleRange == Vector2.zero) angleRange = new Vector2(0, 360);
		if (speedRange == Vector2.zero) speedRange = new Vector2(2, 5);
		if (timeOutTime == 0) timeOutTime = 3f;
	}

	private void FixedUpdate()
	{
		if (starManager.starCreated)
		{
			Vector2 playerPos = Vector2.zero;
			if (PlayerPlatformerController._instance != null)
			{
				playerPos = PlayerPlatformerController._instance.transform.position;
			}
			else if (target != null)
			{
				playerPos = target.position;
			}
			
			starManager.updateStars(playerPos);
			
			if (starManager.destroyObj && !goingToChangeScene)
			{
				goingToChangeScene = true;
				StartCoroutine(changeScene());
			}
		}
		
		if (starManager.starCreated && timeOutTime > 0)
		{
			timeOutTimer += Time.fixedDeltaTime;
			if (timeOutTimer >= timeOutTime && !goingToChangeScene)
			{
				goingToChangeScene = true;
				StartCoroutine(changeScene());
			}
		}
	}

	private IEnumerator changeScene()
	{
		yield return new WaitForSeconds(1f);
		
		if (callBackObj != null)
		{
			callBackObj.SetActive(true);
		}
		
		if (goNextStage && LevelManager._instance != null)
		{
			LevelManager._instance.toNextLevel(true);
		}
	}

	public void collectMemory(int amount = -1)
	{
		collectMemory(amount, 0f, false);
	}

	public void collectMemory(int amount, float deltaTime = 0f, bool investDirection = false)
	{
		if (starManager.starCreated) return;
		
		starManager.starCreated = true;
		starManager.playSound = playSound;
		
		int starCount = amount > 0 ? amount : Random.Range(8, 15);
		
		for (int i = 0; i < starCount; i++)
		{
			GameObject starPrefab = useChildMemory && childMemory != null ? childMemory.gameObject : memoryPrefab;
			if (starPrefab == null) continue;
			
			Vector2 direction = investDirection ? Vector2.down : Vector2.up;
			float delay = deltaTime + (i * 0.05f);
			
			Star newStar = new Star(starManager, starPrefab, direction, speedRange, angleRange, delay);
			newStar.starTrans.position = transform.position;
			starManager.stars.Add(newStar);
		}
	}
}