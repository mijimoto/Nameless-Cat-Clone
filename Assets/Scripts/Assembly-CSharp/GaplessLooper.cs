using UnityEngine;

public class GaplessLooper : MonoBehaviour
{
	[SerializeField]
	private AudioSource[] audioSource;

	public AudioClip clip;

	public float cutTimeInPercent;

	private float cutTime;

	private float totalLength;

	private int majorSourceIndex;

	private float volumeRatio;

	public float volumeMaximum;

	private float tailTime;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
