using System.Collections;
using UnityEngine;

public class Collection : MonoBehaviour
{
	private const float TOTAL_ANIMATION_SEC = 1f;

	[SerializeField]
	private AudioClip collectionSound;

	[SerializeField]
	private Sprite collectionPhoto;

	[SerializeField]
	private int amountOfStar;

	[SerializeField]
	private bool soundControll;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private float minDistance;

	[SerializeField]
	private float maxDistance;

	private float baseVolume;

	[SerializeField]
	private GameObject memoryStar;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private ParticleSystem particleSystemNormal;

	[SerializeField]
	private ParticleSystem particleSystemDissolve;

	[SerializeField]
	private Animator dissolveAnimator;

	[SerializeField]
	private float dissolveAmount;

	private float lastDissolveAmount;

	private Vector3 origialPos;

	private Material material;

	private Transform player;

	private bool collected;

	private string chapter;

	private SoundManager soundManager;

	private MusicPlayer musicPlayer;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public static bool hasCollected(string chapter)
	{
		return false;
	}

	public static bool hasCollected(int chapter)
	{
		return false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
	}

	private IEnumerator collectionAnimation()
	{
		return null;
	}

	public void storyEnd()
	{
	}

	public void animationEnd()
	{
	}
}
