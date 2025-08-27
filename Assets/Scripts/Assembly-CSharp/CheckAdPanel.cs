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

	public bool Showing => showing;

	private void Update()
	{
		if (showing && timeoutTimer > 0)
		{
			timeoutTimer -= Time.deltaTime;
			if (timeoutTimer <= 0)
			{
				failToLoadAd();
			}
		}
	}

	public void ShowPanel(bool show)
	{
		showing = show;
		if (panelRoot != null)
		{
			panelRoot.SetActive(show);
		}
		
		if (show)
		{
			if (loadingCover != null) loadingCover.SetActive(true);
			if (failText != null) failText.SetActive(false);
			if (cancelBtn != null) cancelBtn.SetActive(false);
			timeoutTimer = timeoutTime;
		}
		else
		{
			timeoutTimer = 0;
		}
	}

	public void loadCheckPointAd()
	{
		// Simulate ad loading
		if (loadingCover != null) loadingCover.SetActive(true);
		if (failText != null) failText.SetActive(false);
		if (cancelBtn != null) cancelBtn.SetActive(false);
		
		timeoutTimer = timeoutTime;
		
		// Simulate successful ad load after 2 seconds
		Invoke("CheckPointAdSuccess", 2f);
	}

	private void CheckPointAdSuccess()
	{
		// Ad loaded successfully
		CheckPoint.state = CheckPoint.AdCheckPointState.AdActive;
		if (CheckPoint.adStoreCheckPoint != null)
		{
			CheckPoint.adStoreCheckPoint.activeSavePoint();
		}
		finishLoading();
		ShowPanel(false);
	}

	public void failToLoadAd()
	{
		if (loadingCover != null) loadingCover.SetActive(false);
		if (failText != null) failText.SetActive(true);
		if (cancelBtn != null) cancelBtn.SetActive(true);
		timeoutTimer = 0;
	}

	public void finishLoading()
	{
		if (loadingCover != null) loadingCover.SetActive(false);
		timeoutTimer = 0;
	}

	public void cancelLoadingVideo()
	{
		ShowPanel(false);
		timeoutTimer = 0;
	}
}
