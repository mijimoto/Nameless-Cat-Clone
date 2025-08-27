using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	public const string DEAD_COUNT_ALL_KEY = "deadCount_all";

	public const string DEAD_COUNT_LEVEL_KEY = "deadCount_level_";

	public const string DEAD_COUNT_KILLER_KEY = "deadCount_kill_";

	public const string PLAY_TIME_KEY = "playTime";

	public const string MOVE_DISTANCE_KEY = "moveDistance";

	public const string JUMP_COUNT_KEY = "jumpTime";

	public const string BLOCK_ATTACH_COUNT_KEY = "blockAttachCountKey";

	public const string BLOCK_TRADE_COUNT_KEY = "blockTradeCountKey";

	public const string COLLECT_FISH_TOTAL_KEY = "collectedBox";

	private static Dictionary<string, int> intData;

	private static Dictionary<string, float> floatData;

	public static void deadCount(string level, string killer)
	{
		if (intData == null) 
		{
			intData = new Dictionary<string, int>();
		}

		// Increment total death count
		addData(DEAD_COUNT_ALL_KEY, 1);

		// Increment level-specific death count
		string levelKey = DEAD_COUNT_LEVEL_KEY + level;
		addData(levelKey, 1);

		// Increment killer-specific death count
		if (!string.IsNullOrEmpty(killer))
		{
			string killerKey = DEAD_COUNT_KILLER_KEY + killer;
			addData(killerKey, 1);
		}
	}

	public static void addData(string key, int amount, bool saveData = false)
	{
		if (intData == null)
		{
			intData = new Dictionary<string, int>();
		}

		if (!intData.ContainsKey(key))
		{
			intData[key] = PlayerPrefs.GetInt(key, 0);
		}

		intData[key] += amount;

		if (saveData)
		{
			saveField(key);
		}
	}

	public static void addData(string key, float amount, bool saveData = false)
	{
		if (floatData == null)
		{
			floatData = new Dictionary<string, float>();
		}

		if (!floatData.ContainsKey(key))
		{
			floatData[key] = PlayerPrefs.GetFloat(key, 0f);
		}

		floatData[key] += amount;

		if (saveData)
		{
			saveField(key);
		}
	}

	public static void saveField(string key)
	{
		if (intData != null && intData.ContainsKey(key))
		{
			PlayerPrefs.SetInt(key, intData[key]);
		}
		else if (floatData != null && floatData.ContainsKey(key))
		{
			PlayerPrefs.SetFloat(key, floatData[key]);
		}
		
		PlayerPrefs.Save();
	}

	public static void saveAll()
	{
		if (intData != null)
		{
			foreach (var kvp in intData)
			{
				PlayerPrefs.SetInt(kvp.Key, kvp.Value);
			}
		}

		if (floatData != null)
		{
			foreach (var kvp in floatData)
			{
				PlayerPrefs.SetFloat(kvp.Key, kvp.Value);
			}
		}

		PlayerPrefs.Save();
	}

	public static void clearAll()
	{
		if (intData != null)
		{
			foreach (string key in intData.Keys)
			{
				PlayerPrefs.DeleteKey(key);
			}
			intData.Clear();
		}

		if (floatData != null)
		{
			foreach (string key in floatData.Keys)
			{
				PlayerPrefs.DeleteKey(key);
			}
			floatData.Clear();
		}

		PlayerPrefs.Save();
	}

	private void Start()
	{
		if (intData == null)
		{
			intData = new Dictionary<string, int>();
		}
		if (floatData == null)
		{
			floatData = new Dictionary<string, float>();
		}
	}

	private void FixedUpdate()
	{
		addData(PLAY_TIME_KEY, Time.fixedDeltaTime);
	}

	private void OnApplicationQuit()
	{
		saveAll();
	}
}