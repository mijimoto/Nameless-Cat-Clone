using UnityEngine;

public class CheckAdPanel : MonoBehaviour
{
	[SerializeField]
	private GameObject panelRoot;

	[SerializeField]
	private GameObject loadingCover;

	[SerializeField]
	private GameObject failText;

	[SerializeField]
	private GameObject cancelBtn;

	private float timeoutTimer;

	private const float timeoutTime = 15f;

	private bool showing;

	public bool Showing => false;

	private void Update()
	{
	}

	public void ShowPanel(bool show)
	{
	}

	public void loadCheckPointAd()
	{
	}

	private void CheckPointAdSuccess()
	{
	}

	public void failToLoadAd()
	{
	}

	public void finishLoading()
	{
	}

	public void cancelLoadingVideo()
	{
	}
}
