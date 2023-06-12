using SinSity.Analytics;
using SinSity.Utils;
using UnityEngine;

namespace VavilichevGD.Monetization {
    public static class ADS {

        public static bool isActive => interactor.isActive;

        private static ADSInteractor interactor;
        
        public static void Initialize(ADSInteractor _interactor) {
            interactor = _interactor;
        }

        public static void ShowRewardedVideo(string placementId, ADSResultsHandler callback) {

            /*if (InternetChecker.internetConnected) {
                var isAvailable = interactor.IsRewardedVideoAvailable();
                LogADAvailability(ADType.rewarded, placementId, isAvailable);

                if (isAvailable) {
                    void OnAdStarted(ADSInteractor adsInteractor) {
                        interactor.OnAdStartedEvent -= OnAdStarted;
                        var result = isAvailable ? ADResult.started : ADResult.not_available;
                        LogADStarted(ADType.rewarded, placementId, result);
                    }

                    void OnAdResults(bool success, ADResult result, string error) {
                        callback?.Invoke(success, result, error);
                        LogADResult(ADType.rewarded, placementId, result);
                    }

                    void OnAdClicked(ADSInteractor adsInteractor) {
                        interactor.OnAdClickedEvent -= OnAdClicked;
                        LogADResult(ADType.rewarded, placementId, ADResult.clicked);
                    }
        
                    interactor.OnAdStartedEvent += OnAdStarted;
                    interactor.OnAdClickedEvent += OnAdClicked;
                    interactor.ShowRewardedVideo(OnAdResults);
                }
                else {
                    callback?.Invoke(false, ADResult.error, "Video is not available");
                }
                
            }
            else {
                callback?.Invoke(false, ADResult.offline, "Internet is not connected");
            }*/
            callback?.Invoke(false, ADResult.error, "Video is not available");
        }

        

        public static void ShowInterstitial(string placementId, ADSResultsHandler callback = null) {
            var isAvailable = interactor.IsInterstitialAvailable();
            LogADAvailability(ADType.interstitial, placementId, isAvailable);
            interactor.ShowInterstitial(callback);
        }

        public static void ShowBaner(string placementId) {
            var isAvailable = interactor.IsBannerAvailable();
            LogADAvailability(ADType.banner, placementId, isAvailable);
            interactor.ShowBanner();
        }

        public static void ActivateADS() {
            interactor.ActivateADS();
        }

        public static void DeactivateADS() {
            interactor.DeactivateADS();
        }

        public static bool IsInterstitialAvailable() {
            return interactor.IsInterstitialAvailable();
        }

        public static bool IsRewardedVideoAvailable() {
            return interactor.IsRewardedVideoAvailable();
        }

        public static bool IsBannerAvailable() {
            return interactor.IsBannerAvailable();
        }

        
        
        #region ANALYTICS

        private static void LogADAvailability(ADType type, string placementId, bool isAvailable) {
            /*var availability = isAvailable ? ADAvailability.success : ADAvailability.not_available;
            ADSAnalytics.LogVideoADAvailable(placementId, type, availability, InternetChecker.internetConnected);*/
        }

        private static void LogADStarted(ADType type, string placementId, ADResult result) {
            //ADSAnalytics.LogVideoADStarted(placementId, type, result, InternetChecker.internetConnected);
        }

        private static void LogADResult(ADType type, string placementId, ADResult result) {
            //ADSAnalytics.LogVideoADResult(placementId, type, result, InternetChecker.internetConnected);
        }
        
        #endregion
    }
}