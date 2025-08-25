using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobRequest : MonoBehaviour
{
	private const string LOG_CATEGORY = "AdMob";

	private const string appIdAndroid = "ca-app-pub-6732596046627259~6739637166";

	private const string checkPointVideoAndroid = "ca-app-pub-6732596046627259/3741718866";

	private const string interstitialAndroid = "ca-app-pub-6732596046627259/4037877237";

	private const string appIdIos = "ca-app-pub-6732596046627259~6507530385";

	private const string checkPointVideoIos = "ca-app-pub-6732596046627259/2153707625";

	private const string interstitialIos = "ca-app-pub-6732596046627259/3358978086";

	public static AdMobRequest _instance;

	[SerializeField]
	private bool showLog;

	private InterstitialAd interstitial;

	private RewardedAd rewardBasedVideo;

	public static bool skipAd;

	private bool ableToCall;

	private bool showVideo;

	private bool initialized;

	private void Start()
	{
	}

	private void Init()
	{
	}

	private void OnInitialized(InitializationStatus status)
	{
	}

	private AdRequest CreateAdRequest()
	{
		return null;
	}

	private void RequestInterstitial()
	{
	}

	public void HandleOnAdLoaded_Interstitial(object sender, EventArgs args)
	{
	}

	public void HandleOnAdFailedToLoad_Interstitial(object sender, AdFailedToLoadEventArgs args)
	{
	}

	public void HandleOnAdOpened_Interstitial(object sender, EventArgs args)
	{
	}

	public void HandleOnAdClosed_Interstitial(object sender, EventArgs args)
	{
	}

	public void ShowInterstitial()
	{
	}

	public void destroyInterstitial()
	{
	}

	public void ShowRewardBasedVideo()
	{
	}

	public void CancelLoadingVideo()
	{
	}

	private void RequestRewardedVideo()
	{
	}

	private void HandleOnAdLoaded(object sender, EventArgs args)
	{
	}

	private void HandleRewardBasedVideoFailedToLoad(object sender, EventArgs args)
	{
	}

	private void loadVideoFail()
	{
	}

	private void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
	}

	private void HandleOnAdClosed(object sender, EventArgs args)
	{
	}

	private void VideoRewarded()
	{
	}

	private void VideoClose()
	{
	}

	private bool NetworkConnected()
	{
		return false;
	}
}
