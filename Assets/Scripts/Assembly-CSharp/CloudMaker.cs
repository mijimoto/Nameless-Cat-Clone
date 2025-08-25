using System.Collections.Generic;
using UnityEngine;

public class CloudMaker : MonoBehaviour
{
	private class Cloud
	{
		public static List<Cloud> clouds;

		public static List<Cloud> removeClouds;

		public static List<Cloud> unusedClouds;

		private Transform cloud;

		private GameObject cloudGameObject;

		private float speed;

		private SpriteRenderer spriteRenderer;

		public Cloud(GameObject c, float s, Sprite sp, Transform parent)
		{
		}

		public static Cloud createCloud(GameObject cloudPrefab, Vector3 spawnPos, float s, Sprite sp, Transform parent)
		{
			return null;
		}

		public static void updateCloud()
		{
		}

		public void updatePos()
		{
		}
	}

	public static CloudMaker _instance;

	public GameObject cloudPrefab;

	public Sprite[] sprites;

	public Vector2 speedRange;

	public Vector2 spawnRange;

	private float spawnTimer;

	public bool spawnCloud;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static void cleanAllCloud()
	{
	}

	private void OnDestroy()
	{
	}
}
