using UnityEngine;

public class DataSetter : MonoBehaviour
{
	public static void unlockLevel()
	{
		int currentLevel = PlayerPrefs.GetInt("unlockedLevel", 1);
		PlayerPrefs.SetInt("unlockedLevel", currentLevel + 1);
		PlayerPrefs.Save();
	}

	public static void clearLevel()
	{
		PlayerPrefs.DeleteKey("unlockedLevel");
		
		for (int i = 1; i <= 100; i++)
		{
			PlayerPrefs.DeleteKey("level_" + i + "_completed");
			PlayerPrefs.DeleteKey("level_" + i + "_stars");
		}
		
		PlayerPrefs.Save();
	}

	public static void clearCollection()
	{
		PlayerPrefs.DeleteKey("totalCollected");
		PlayerPrefs.DeleteKey("collectionItems");
		
		for (int i = 1; i <= 50; i++)
		{
			PlayerPrefs.DeleteKey("collection_" + i);
		}
		
		PlayerPrefs.Save();
	}

	public static void ClearFishCan()
	{
		PlayerPrefs.DeleteKey(DataManager.COLLECT_FISH_TOTAL_KEY);
		PlayerPrefs.DeleteKey("fishCanCount");
		PlayerPrefs.Save();
	}

	public static void clearData()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
		DataManager.clearAll();
	}
}