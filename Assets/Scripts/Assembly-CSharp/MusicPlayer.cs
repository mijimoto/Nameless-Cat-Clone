using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public static MusicPlayer _instance;

	public AudioClip titleMusic;

	public AudioClip[] battleMusic;

	public AudioClip[] preBattleMusic;

	public AudioClip[] gameMusic;

	private AudioSource aui;

	private float baseVolume;

	private float extraVolumeModifier;

	private int targetMusic;

	private AudioClip targetMusicClip;

	private float currentSmoothVolume;

	private float smoothTargetVolume;

	private float smoothSide;

	private bool musicUp;

	private AudioClip nextMusic;

	public bool HasMusic => false;

	public bool HasNextMusic => false;

	private void Awake()
	{
	}

	private void Update()
	{
	}

	public void changeMusic(int musicIndex, int chapter = 1, bool play = true)
	{
	}

	public void setNextMusic(int musicIndex, int chapter = 1)
	{
	}

	public void playMusic(AudioClip music)
	{
	}

	private AudioClip getMusic(int musicIndex, int chapter)
	{
		return null;
	}

	public void pauseMusic(bool b)
	{
	}

	public void updateVolme()
	{
	}

	public void SetExtraVolumeModifier(float extraVolumeModifier)
	{
	}

	public void smoothChangeMusic(int musicIndex = -1)
	{
	}

	public void smoothChangeMusic(AudioClip musicClip)
	{
	}

	public void startFadeIn(float targetVolume = 1f)
	{
	}

	public void startFadeOut(float targetVolume = 0f)
	{
	}

	public void ResetSmooth()
	{
	}
}
