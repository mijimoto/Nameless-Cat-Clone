using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager _instance;

	public AudioClip onGroundSound;

	public AudioClip moveSound;

	public AudioClip jumpSound;

	public AudioClip dieSound;

	public AudioClip winSound;

	public AudioClip memoryStarSound;

	public AudioClip checkPointSound;

	public AudioClip godLeaveSound;

	public AudioClip boomSound;

	public AudioClip yellSound;

	public AudioClip collectSound;

	public AudioClip mushroomSound;

	public AudioClip attachInSound;

	public AudioClip attachOutSound;

	public AudioClip pressurePlateSound;

	public AudioClip[] shootSound;

	public AudioClip clickSound;

	private AudioSource aui;

	public bool musicOn;

	public bool soundOn;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void playAudioClip(AudioClip audioClip, float sound)
	{
	}

	public void playCharacterSound(string soundName)
	{
	}

	public static float volumeBetweenObj(Transform obj1, Transform obj2, float minDis = 3.5f, float maxDis = 4f)
	{
		return 0f;
	}

	public static float volumeBetweenObj(Vector3 pos1, Vector3 pos2, float minDis = 3.5f, float maxDis = 4f)
	{
		return 0f;
	}

	public void effectByDistance(string soundName, Transform obj1, Transform obj2, int index = -1)
	{
	}

	public void effect(string soundName, int index = -1, float volumeMultiper = 1f)
	{
	}

	public void objectSound(string soundName, int index = -1, float volumeMultiper = 1f)
	{
	}

	public void uiSound(string soundName)
	{
	}

	public void toggleMusic()
	{
	}

	public void toggleSound()
	{
	}

	public void setMusic(bool b)
	{
	}

	public void setSound(bool b)
	{
	}
}
