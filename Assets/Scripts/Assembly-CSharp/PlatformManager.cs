using UnityEngine;

public class PlatformManager : MonoBehaviour
{
	public static PlatformManager _instance;

	[SerializeField]
	private SteamManager steamManager;

	private void Awake()
	{
	}
}
