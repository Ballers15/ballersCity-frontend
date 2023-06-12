using System;
using System.Collections;
using SinSity.Analytics;
//using GoogleMobileAds.Api;
using UnityEngine;

namespace VavilichevGD.Monetization.AdMob {
	public class ADSBehaviorAdMob : ADSBehavior {

		#region DELEGATES

		public override event ADSBehaviorADHandler OnAdStartedEvent;
		public override event ADSBehaviorADHandler OnAdClickedEvent;

		#endregion
		
		private enum State {
			None,
			Busy,
			WaitForReward,
			Error
		}
		
		//private InterstitialAd interstitial;
		//private RewardBasedVideoAd rewardBasedVideo;
		private State stateInterstitial;
		private State stateRewardedVideo;
		private ADSResultsHandler callbackInterstitial;
		private ADSResultsHandler callbackRewardedVideo;
		private ADSAdmobUpdateHelper helper;
		private ADSSettingsAdMob settings;

		private const string PATH_SETTINGS = "ADSSettingsAdMob";



		protected override void Initialize() {
			settings = Resources.Load<ADSSettingsAdMob>(PATH_SETTINGS);
			
			//MobileAds.Initialize(settings.appId);

			InitRewardedVideo();
			InitInterstitial();
			CreateHelper();
		}

		public override bool IsRewardedVideoAvailable() {
			//return this.rewardBasedVideo.IsLoaded();
			return false;
		}

		public override bool IsInterstitialAvailable() {
			//return this.interstitial.IsLoaded();
			return false;
		}

		private void CreateHelper() {
			if (!helper) {
				GameObject helperGO = new GameObject("AdMob Helper");
				helper = helperGO.AddComponent<ADSAdmobUpdateHelper>();
				helper.OnUpdate += Update;
				UnityEngine.Object.DontDestroyOnLoad(helperGO);
			}
		}

		
		#region Initialize RewardedVideo

		private void InitRewardedVideo() {
			/*rewardBasedVideo = RewardBasedVideoAd.Instance;

			rewardBasedVideo.OnAdClosed += RewardBasedVideo_OnAdClosed;
			rewardBasedVideo.OnAdLeavingApplication += RewardBasedVideo_OnAdLeavingApplication;
			rewardBasedVideo.OnAdOpening += RewardBasedVideo_OnAdOpening;
			rewardBasedVideo.OnAdRewarded += RewardBasedVideo_OnAdRewarded;
			rewardBasedVideo.OnAdStarted += RewardBasedVideo_OnAdStarted;
			RequestRewardBasedVideo(settings.rewardedVideoId);*/
		}

		private void RequestRewardBasedVideo(string id) {
			/*AdRequest request = null;
			if (settings.testMode)
				request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator)
					.AddTestDevice(SystemInfo.deviceUniqueIdentifier.ToUpper()).Build();
			else
				request = new AdRequest.Builder().Build();
			rewardBasedVideo.LoadAd(request, id);*/
		}

		#endregion

		
		
		#region Initialize Interstitial

		private void InitInterstitial() {
			/*interstitial = new InterstitialAd(settings.interstitialId);

			interstitial.OnAdOpening += Interstitial_OnAdOpened;
			interstitial.OnAdClosed += Interstitial_OnAdOpenedOnAdClosed;
			interstitial.OnAdLeavingApplication += Interstitial_OnAdLeavingApplication;
			RequestInterstitial();*/
		}

		private void RequestInterstitial() {
			/*AdRequest request = null;
			if (settings.testMode)
				request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator)
					.AddTestDevice(SystemInfo.deviceUniqueIdentifier.ToUpper()).Build();
			else
				request = new AdRequest.Builder().Build();
			interstitial.LoadAd(request);*/
		}

		#endregion


		
		#region Interstitial WORK

		protected override IEnumerator ShowInterstitialRoutine(ADSResultsHandler callback) {
			/*if (callbackInterstitial != null) {
				callback?.Invoke(FAIL, ADResult.error,  "Another interstitial is working");
				yield break;
			}
			
			stateInterstitial = State.Busy;
			callbackInterstitial = callback;

#if !UNITY_EDITOR
			float timer = 0f;
			while (!interstitial.IsLoaded()) {
				yield return null;
				timer += Time.unscaledDeltaTime;
				if (timer >= settings.breakTime) {
					NotifyAboutInterstitial(FAIL, ADResult.error, "Break time reached. (Interstitial)");
					yield break;
				}
			}
#elif UNITY_EDITOR
			NotifyAboutInterstitial(true, ADResult.watched_editor);
			yield break;
#endif
			
			//interstitial.Show();*/
			yield break;
		}

		#endregion
		

		#region Interstitial EVENTS

		private void Interstitial_OnAdLeavingApplication(object sender, EventArgs e) {
			stateInterstitial = State.Error;
			UnPause();
		}

		public void Interstitial_OnAdOpened(object sender, EventArgs args) {
			Pause();
		}

		public void Interstitial_OnAdOpenedOnAdClosed(object sender, EventArgs args) {
			stateInterstitial = State.WaitForReward;
			UnPause();
		}
		
		#endregion

		
		
		#region RewardedVideo WORK
		
		protected override IEnumerator ShowRewardedVideoRoutine(ADSResultsHandler callback) {
			/*if (callbackRewardedVideo != null) {
				callback?.Invoke(FAIL, ADResult.error, "Another rewardedVideo is working");
				yield break;
			}
			stateRewardedVideo = State.Busy;
			callbackRewardedVideo = callback;
#if !UNITY_EDITOR
			float timer = 0f;
			while (!rewardBasedVideo.IsLoaded()) {
				yield return null;
				timer += Time.unscaledDeltaTime;
				if (timer >= settings.breakTime) {
					NotifyAboutRewardedVideo(FAIL, ADResult.error, "Break time reached. (RewardedVideo)");
					yield break;
				}
			}
#elif UNITY_EDITOR
			NotifyAboutRewardedVideo(true, ADResult.watched_editor);
			yield break;
#endif
			
			rewardBasedVideo.Show();*/
			yield break;
		}
		
		#endregion

		
		#region RewardedVideo EVENTS

		private void RewardBasedVideo_OnAdStarted(object sender, EventArgs e) {
			Pause();
			OnAdStartedEvent?.Invoke(this);
		}

		/*private void RewardBasedVideo_OnAdRewarded(object sender, GoogleMobileAds.Api.Reward e) {
			stateRewardedVideo = State.WaitForReward;
		}*/

		private void RewardBasedVideo_OnAdOpening(object sender, EventArgs e) {
			Pause();
		}

		private void RewardBasedVideo_OnAdLeavingApplication(object sender, EventArgs e) {
//			stateRewardedVideo = State.Error;
//			UnPause();
		}

		private void RewardBasedVideo_OnAdClosed(object sender, EventArgs e) {
			if (stateRewardedVideo != State.WaitForReward)
				stateRewardedVideo = State.Error;
			UnPause();
		}

		#endregion

		
		
		private void Update() {
			if (callbackInterstitial != null) {
				switch (stateInterstitial) {
					case State.WaitForReward:
						NotifyAboutInterstitial(SUCCESS, ADResult.watched);
						break;
					case State.Error:
						NotifyAboutInterstitial(FAIL, ADResult.error, $"State: {stateInterstitial}");
						break;
				}

			}

			if (callbackRewardedVideo != null) {
				switch (stateRewardedVideo) {
					case State.WaitForReward:
						NotifyAboutRewardedVideo(SUCCESS, ADResult.watched);
						break;
					case State.Error:
						NotifyAboutRewardedVideo(FAIL, ADResult.error, $"State: {stateRewardedVideo}");
						break;
				}
			}
		}
		
		
		private void NotifyAboutInterstitial(bool success, ADResult result,  string error = "") {
			stateInterstitial = State.None;
			RequestInterstitial();
			callbackInterstitial?.Invoke(success, result, error);
			callbackInterstitial = null;
		}
		
		
		private void NotifyAboutRewardedVideo(bool success, ADResult result, string error = "") {
			stateRewardedVideo = State.None;
			RequestRewardBasedVideo(settings.rewardedVideoId);
			callbackRewardedVideo?.Invoke(success, result, error);
			callbackRewardedVideo = null;
		}
		
		
		private void UnPause() {
			// TODO: Need to unpause game in iOS. (AdMob doesnt pause game)
			#if UNITY_IOS
			#endif
		}
		
		
		private void Pause() {
			// TODO: Need to pause game in iOS. (AdMob doesnt pause game)
			#if UNITY_IOS
			#endif
		}
    }
}
