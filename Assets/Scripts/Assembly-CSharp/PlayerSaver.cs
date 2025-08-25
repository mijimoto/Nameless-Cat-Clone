using System;

public class PlayerSaver
{
	[Serializable]
	public class PlayerSaveData
	{
		public int firstOpen;

		public int completedGame;

		public LevelDataArray[] levelDatas;

		public int currentFish;

		public int totalFish;

		public int skipAd;

		public int[] ownedSkins;

		public int deadCount;

		public float playTime;

		public float moveDistance;

		public int jumpCount;

		public int attachCount;

		public int tradeCount;

		public string ToJson()
		{
			return null;
		}

		public void SaveToLocal()
		{
		}

		public void LoadFromLocal()
		{
		}
	}

	[Serializable]
	public class LevelDataArray
	{
		public int[] levelBitMapArray;

		public int[] levelDeadCount;

		public LevelDataArray(int length)
		{
		}

		public void FormBitMapData(int index, int completedLevel, int gotFish)
		{
		}

		public Tuple<int, int> GetBitMapData(int index)
		{
			return null;
		}
	}

	public static string GetSaveDataJson()
	{
		return null;
	}

	public static void SaveFromJson(string json)
	{
	}
}
