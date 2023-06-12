using System.Collections;
using SinSity.Analytics;
using UnityEngine;
using UnityEngine.Advertisements;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization.Unity {
	public class ADSBehaviorUnity : ADSBehavior {


		#region DELEGATES

		public override event ADSBehaviorADHandler OnAdStartedEvent;
		public override event ADSBehaviorADHandler OnAdClickedEvent;

		#endregion

	    
	    private ADSSettingsUnity settings;
	    
	    private const string PATH_SETTINGS = "ADSSettingsUnity";
	    private const string REWARDED_VIDEO = "rewardedVideo";
	    private const string INTERSTITIAL = "video";
	    private const string BANNER = "banner";

        private bool rewardedVideoIsWorking;
        private bool interstitialVideoIsWorking;


        protected override void Initialize() {
	        settings = Resources.Load<ADSSettingsUnity>(PATH_SETTINGS);
	        
	        // Advertisement.debugMode = settings.testMode;
	        // if (!string.IsNullOrEmpty(settings.appId))
		       //  Advertisement.Initialize(settings.appId, settings.testMode);
	        // else
		       //  throw new System.Exception($"ADS BEHAVIOR UNITY: APP_ID is null or empty: {settings.appId}");
        }

        public override bool IsRewardedVideoAvailable() {
	        return true;
	        // return Advertisement.IsReady(REWARDED_VIDEO);
        }

        public override bool IsInterstitialAvailable() {
	        return true;
	        // return Advertisement.IsReady(INTERSTITIAL);
        }


        #region Rewarded Video

        private void NotifyAboutResultsRewardedVideo(ADSResultsHandler callback, bool success, ADResult result, string error = "") {
	        rewardedVideoIsWorking = false;
	        callback?.Invoke(success, result, error);
        }

        protected override IEnumerator ShowRewardedVideoRoutine(ADSResultsHandler callback) {
	        if (rewardedVideoIsWorking) {
		        NotifyAboutResultsRewardedVideo(callback, FAIL, ADResult.error, "AD process is working now (rewardedVideo)");
		        yield break;
	        }
	        
	        rewardedVideoIsWorking = true;
#if !UNITY_EDITOR
			// float timer = 0f;
			// while (!Advertisement.IsReady(REWARDED_VIDEO)) {
			// 	yield return null;
			// 	timer += Time.unscaledDeltaTime;
			// 	if (timer >= settings.breakTime) {
			// 		NotifyAboutResultsRewardedVideo(callback, FAIL, ADResult.error, $"Not loaded during a BREAK_TIME ({settings.breakTime} sec (rewardedVideo)");
			// 		yield break;
			// 	}
			// }
#endif

	        // ShowOptions options = new ShowOptions {
		       //  resultCallback = result => { HandleRewardedVideoResult(result, callback); }
	        // };
	        // Advertisement.Show(REWARDED_VIDEO, options);
	        this.OnAdStartedEvent?.Invoke(this);
        }

        private void HandleRewardedVideoResult(/*ShowResult result, */ADSResultsHandler callback) {
	        // switch (result) {
		       //  case ShowResult.Finished:
			      //   NotifyAboutResultsRewardedVideo(callback, SUCCESS, ADResult.watched);
			      //   break;
		       //  case ShowResult.Failed:
			      //   NotifyAboutResultsRewardedVideo(callback, FAIL, ADResult.error, "The AD failed (rewardedVideo)");
			      //   break;
	        // }
        }
        
        #endregion


        #region Interstitial

        private void NotifyAboutResultsInterstitialVideo(ADSResultsHandler callback, bool success, ADResult result, string error = "") {
	        interstitialVideoIsWorking = false;
	        callback?.Invoke(success, result, error);
        }

        protected override IEnumerator ShowInterstitialRoutine(ADSResultsHandler callback) {
	        if (interstitialVideoIsWorking) {
		        NotifyAboutResultsInterstitialVideo(callback, FAIL, ADResult.error, "AD process is working now (interstitialVideo)");
		        yield break;
	        }

	        interstitialVideoIsWorking = true;
	        // if (!Advertisement.IsReady(INTERSTITIAL)) {
		        NotifyAboutResultsInterstitialVideo(callback, FAIL, ADResult.error, "AD is not loaded yet (interstitialVideo)");
		        yield break;
	        // }
	        
		    // ShowOptions options = new ShowOptions { resultCallback = (result) => HandleSkippableVideoResult(callback, result) };
		    // Advertisement.Show(options);
		    this.OnAdStartedEvent?.Invoke(this);
        }
        
		private void HandleSkippableVideoResult(ADSResultsHandler callback/*, ShowResult result*/) {
			// switch (result) {
			// 	case ShowResult.Finished:
			// 		NotifyAboutResultsInterstitialVideo(callback, SUCCESS, ADResult.watched);
			// 		break;
			// 	case ShowResult.Skipped:
			// 		NotifyAboutResultsInterstitialVideo(callback, FAIL, ADResult.canceled, "AD skipped (interstitialVideo)");
			// 		break;
			// 	case ShowResult.Failed:
			// 		NotifyAboutResultsInterstitialVideo(callback, FAIL, ADResult.error, "The ad failed to be shown. (interstitialVideo)");
			// 		break;
			// }
		}
		
		#endregion

		
		public override void ShowBanner() {
			// Advertisement.Show(BANNER);
			Logging.Log("ADS BEHAVIOR UNITY: Banner ADS starts showing");
		}
    }
}