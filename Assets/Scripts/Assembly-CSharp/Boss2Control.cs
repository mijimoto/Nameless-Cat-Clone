using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boss2Control : MonoBehaviour
{
	public static Boss2Control _instance;

	public Boss2 boss;

	public Animator bossShadow;

	public Animator totemFall;

	public GameObject cannonBtn;

	public GameObject startBtn;

	public float camYAdjust;

	public FixCam fixcam;

	public GameObject[] door;

	public Tilemap tilemap;

	public GameObject rockEffect;

	public GameObject fishCan;

	private static bool started;

	private Coroutine startBossCor;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void active(bool b)
	{
	}

	private IEnumerator activeBoss()
	{
		return null;
	}

	private void OnDestroy()
	{
	}
}
