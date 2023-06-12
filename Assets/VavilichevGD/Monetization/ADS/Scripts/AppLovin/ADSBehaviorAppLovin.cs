using System;
using System.Collections;
using SinSity.Analytics;
using UnityEngine;
using VavilichevGD.Tools;
using VavilichevGD.Audio;

namespace VavilichevGD.Monetization.AppLovin {
    public class ADSBehaviorAppLovin : ADSBehavior {
        
        #region CONSTANTS

        private const string PATH_SETTINGS = "ADSSettingsAppLovin";

        #endregion

        #region DELEGATES

        public override event ADSBehaviorADHandler OnAdStartedEvent;
        public override event ADSBehaviorADHandler OnAdClickedEvent;

        #endregion
        
        private ADSSettingsAppLovin settings;
        private int interstitialRetryAttempt;
        private int rewardedVidedoRetryAttempt;

        private ADSResultsHandler callbackInterstitial;
        private ADSResultsHandler callbackRewardedVideo;



        protected override void Initialize() {
            
            this.settings = Resources.Load<ADSSettingsAppLovin>(PATH_SETTINGS);
            /*MaxSdkCallbacks.OnSdkInitializedEvent += this.OnSdkInitialized;
            

            MaxSdk.SetSdkKey(this.settings.apiKey);
            MaxSdk.InitializeSdk();*/
        }

        public override bool IsRewardedVideoAvailable() {
            //return MaxSdk.IsRewardedAdReady(this.settings.rewardedVideoAdUnitId);
            return false;
        }

        public override bool IsInterstitialAvailable() {
            //return MaxSdk.IsInterstitialReady(this.settings.interstitialAdUnitId);
            return false;
        }

        protected override IEnumerator ShowRewardedVideoRoutine(ADSResultsHandler callback) {
            /*if (this.callbackRewardedVideo != null) {
                callback?.Invoke(FAIL, ADResult.error, "Another rewarded video is working");
                yield break;
            }
            
            float timer = 0f;
            string adId = this.settings.rewardedVideoAdUnitId;

            while (!MaxSdk.IsRewardedAdReady(adId)) {
                yield return null;
                timer += Time.unscaledDeltaTime;
                if (timer >= settings.breakTime) {
                    NotifyAboutRewardedVideo(FAIL, ADResult.error, "Break time reached. (Rewarded Video)");
                    yield break;
                }
            }
			
            this.callbackRewardedVideo = callback;
			this.OnAdStartedEvent?.Invoke(this);
            
            MaxSdk.ShowRewardedAd(adId);
            Sounds.Pause();*/
            yield break;
        }

        protected override IEnumerator ShowInterstitialRoutine(ADSResultsHandler callback) {
            /*if (callbackInterstitial != null) {
                callback?.Invoke(FAIL, ADResult.error, "Another interstitial is working");
                yield break;
            }
            
            float timer = 0f;
            string adId = this.settings.interstitialAdUnitId;

            while (!MaxSdk.IsInterstitialReady(adId)) {
                yield return null;
                timer += Time.unscaledDeltaTime;
                if (timer >= settings.breakTime) {
                    NotifyAboutInterstitial(FAIL, ADResult.error, "Break time reached. (Interstitial)");
                    yield break;
                }
            }
			
            callbackInterstitial = callback;
            this.OnAdStartedEvent?.Invoke(this);

            MaxSdk.ShowInterstitial(adId);
            Sounds.Pause();*/
            yield break;
        }



        #region INIT INTERSTITIAL

        public void InitInterstitialAds() {
            // Attach callback
            /*MaxSdkCallbacks.OnInterstitialLoadedEvent += this.OnInterstitialLoaded;
            MaxSdkCallbacks.OnInterstitialLoadFailedEvent += this.OnInterstitialLoadFailed;
            MaxSdkCallbacks.OnInterstitialDisplayedEvent += this.OnInterstitialDisplayed;
            MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += this.OnInterstitialAdFailedToDisplay;

            // Load the first interstitial
            this.LoadInterstitial();*/
        }

        private void LoadInterstitial() {
            //MaxSdk.LoadInterstitial(this.settings.interstitialAdUnitId);
        }

        private IEnumerator LoadInterstitialDelay() {
            /*double retryDelay = Math.Pow(2, Math.Min(this.settings.maxAttemptCount, this.interstitialRetryAttempt));
            yield return new WaitForSecondsRealtime((float) retryDelay);
            this.LoadInterstitial();*/
            yield break;
        }

        #endregion

        
        #region INIT REWARDED VIDEO


        public void InitRewardedAds() {
            // Attach callback
            /*MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnLoadRewardedAdFailedEvent;
            MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first RewardedAd
            this.LoadRewardedAd();*/
        }

        private void LoadRewardedAd() {
            //MaxSdk.LoadRewardedAd(this.settings.rewardedVideoAdUnitId);
        }
        
        private IEnumerator LoadRewardedVideoDelay() {
            double retryDelay = Math.Pow(2, Math.Min(this.settings.maxAttemptCount, this.rewardedVidedoRetryAttempt));
            yield return new WaitForSecondsRealtime((float) retryDelay);
            this.LoadRewardedAd();
        }

        #endregion


        #region EVENTS

       /* private void OnSdkInitialized(MaxSdkBase.SdkConfiguration obj) {
            //this.InitInterstitialAds();
            this.InitRewardedAds();
        }*/
        
        
        #region Interstitial EVENTS
        
        private void OnInterstitialLoaded(string adUnitId) {
            this.interstitialRetryAttempt = 0;
        }

        private void OnInterstitialLoadFailed(string adUnitId, int errorCode) {
            this.interstitialRetryAttempt++;
            Coroutines.StartRoutine(this.LoadInterstitialDelay());
        }
        
        private void OnInterstitialDisplayed(string obj) {
            this.NotifyAboutInterstitial(SUCCESS, ADResult.watched);
            this.LoadInterstitial();
        }
        
        private void OnInterstitialAdFailedToDisplay(string adUnitId, int errorCode) {
            this.NotifyAboutInterstitial(FAIL, ADResult.error, $"Ad displaying failed with code: {errorCode}");
            this.LoadInterstitial();
        }
        
        private void NotifyAboutInterstitial(bool success, ADResult result, string error = "") {
            if (this.callbackInterstitial == null)
                return;
            
            this.LoadInterstitial();
            
            Sounds.Unpause();
            callbackInterstitial?.Invoke(success, result, error);
            callbackInterstitial = null;
        }
        
        #endregion

        
        #region RewardedVideo EVENTS

        private void OnRewardedAdLoadedEvent(string adUnitId) {
            this.rewardedVidedoRetryAttempt = 0;
        }

        private void OnLoadRewardedAdFailedEvent(string adUnitId, int errorCode) {
            this.rewardedVidedoRetryAttempt++;
            Coroutines.StartRoutine(this.LoadRewardedVideoDelay());
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode) {
            this.NotifyAboutRewardedVideo(FAIL, ADResult.error, $"Ad displaying failed with code: {errorCode}");
            this.LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId) { }

        private void OnRewardedAdClickedEvent(string adUnitId) {
            this.OnAdClickedEvent?.Invoke(this);
        }

        private void OnRewardedAdDismissedEvent(string adUnitId) {
            this.NotifyAboutRewardedVideo(FAIL, ADResult.canceled);
            this.LoadRewardedAd();
        }

       /* private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward) {
            // Rewarded ad was displayed and user should receive the reward
            this.NotifyAboutRewardedVideo(SUCCESS, ADResult.watched);
        }*/

        private void NotifyAboutRewardedVideo(bool success, ADResult result, string error = "") {
            if (this.callbackRewardedVideo == null)
                return;
            
            this.LoadRewardedAd();
            Sounds.Unpause();
            callbackRewardedVideo?.Invoke(success, result, error);
            callbackRewardedVideo = null;
        }

        #endregion

        #endregion
        
    }
}