public static class CrashlyticsReporter
{
	public enum PlayerAction
	{
		LeftDown = 0,
		LeftUp = 1,
		RightDown = 2,
		RightUp = 3,
		Jump = 4,
		Active = 5
	}

	public enum PlayerState
	{
		Normal = 0,
		Attached = 1,
		WatchingStory = 2,
		Dead = 3
	}

	private const string CURRENT_SCENE_KEY = "Current_Scene";

	private const string LAST_ACTION_KEY = "Last_Action";

	private const string PLAYER_STATE_KEY = "Player_State";

	public static bool initialized;

	public static void SetCurrentSceneKey(string sceneName)
	{
	}

	public static void SetLastActionKey(PlayerAction playerAction)
	{
	}

	public static void SetPlayerStateKey(PlayerState playerState)
	{
	}

	public static void ClearLevelData()
	{
	}

	private static void ClearMetaData(string key)
	{
	}

	private static void SetMetaData(string key, string data)
	{
	}
}
