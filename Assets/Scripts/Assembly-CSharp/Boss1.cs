using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boss1 : MonoBehaviour
{
	[SerializeField]
	private AudioClip beforeBattleMusic;

	public float speed;

	public float stopPos;

	private bool move;

	public GameObject tilemapGameObject;

	private Tilemap tilemap;

	private Animator animator;

	private bool closing;

	private bool stop;

	public float leaveWaitSecond;

	public GameObject breakEffectHolder;

	private static bool started;

	private static bool musicStopped;

	[SerializeField]
	public FirstPlayHider[] firstPlayHiders;

	private ParticleSystem[] breakEffects;

	private int breakEffectIndex;

	private int skipCount;

	private int skipCounter;

	public GameObject[] skipBreakPlatform;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private IEnumerator goback()
	{
		return null;
	}

	public void triggerCollider(int i)
	{
	}

	public void shakeCam()
	{
	}

	private void active(bool b)
	{
	}

	private IEnumerator activeBoss()
	{
		return null;
	}

	private void changeMove(bool b)
	{
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
	}

	private void breakEffect(Vector3 hitPosition)
	{
	}
}
