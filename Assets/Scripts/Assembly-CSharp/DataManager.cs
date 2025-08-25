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
	}

	public static void addData(string key, int amount, bool saveData = false)
	{
	}

	public static void addData(string key, float amount, bool saveData = false)
	{
	}

	public static void saveField(string key)
	{
	}

	public static void saveAll()
	{
	}

	public static void clearAll()
	{
	}

	private void Start()
	{
	}

	private void FixedUpdate()
	{
	}

	private void OnApplicationQuit()
	{
	}
}
