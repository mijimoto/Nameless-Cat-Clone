using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
	private const string LOG_CATEGORY = "Firebase";

	private const string USER_PROPERTY_PAID = "PaidUser";

	private const string USER_PROPERTY_PAID_FREE = "Free";

	private const string USER_PROPERTY_PAID_IS_PAID = "Is paid";

	private const string COMPLETE_LEVEL_EVENT = "CompleteLevel";

	private const string COMPLETE_LEVEL_PARAMETER_LEVEL = "Level";

	private const string COMPLETE_LEVEL_PARAMETER_DEAD = "DeadCount";

	private const string COMPLETE_LEVEL_PARAMETE_GET_FISH = "GetFish";

	private const string COMPLETE_LEVEL_PARAMETE_FINISH_TIME = "FinishTime";

	private const string COMPLETE_LEVEL_PARAMETE_AD_WATCHED = "AdWatached";

	private const string COMPLETE_LEVEL_PARAMETE_AD_CHECKPOINT = "AdCheckPoint";

	private const string DIE_EVENT = "Die";

	private const string DIE_PARAMETER_LEVEL = "Level";

	private const string DIE_PARAMETER_KILLED_BY = "KilledBy";

	private const string WATCH_STORY_EVENT = "WatchStory";

	private const string WATCH_STORY_PARAMETE_STORY_ID = "StoryId";

	private const string CHECK_POINT_AD_EVENT = "CheckPointAd";

	private const string CHECK_POINT_AD_PARAMETER_LEVEL = "Level";

	private const string FINISH_GAME_EVENT = "FinishGame";

	private const string FINISH_GAME_PARAMETER_DEAD = "DeadCount";

	private const string FINISH_GAME_PARAMETER_PLAYED_TIME = "PlayedTime";

	private const string WATCHED_ENDING_EVENT = "WatchedEnding";

	private const string CLEAR_DATA_EVENT = "ClearData";

	public static FirebaseManager instance_;

	[SerializeField]
	private bool showLog;

	private bool initialized;

	public static bool Initialized => false;

	public static bool ShowLog => false;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void UpdatePaidUserProperty(bool isPaid)
	{
	}

	private void LogEvent(string name)
	{
	}

	private void LogEvent(string name, string parameterId, string parameterValue)
	{
	}

	private void LogConsole(string name, string parameterId, object parameterValue)
	{
	}

	public void CompleteLevel(string level, int deadCount, Item.FishState fishState, float finishTime, int adWatched, CheckPoint.AdCheckPointState adCheckPoint)
	{
	}

	public void Die(string level, string killedBy)
	{
	}

	public void FinishGame(int deadCount, float playedTime)
	{
	}

	public void WatchStory(string storyId)
	{
	}

	public void WatchCheckPointAd(string level)
	{
	}

	public void WatchedEnding()
	{
	}

	public void ClearData()
	{
	}
}
