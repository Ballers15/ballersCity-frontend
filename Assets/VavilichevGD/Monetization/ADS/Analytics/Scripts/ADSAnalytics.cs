using System.Collections.Generic;
using SinSity.Domain;
using SinSity.Services;
using VavilichevGD.Architecture;

namespace SinSity.Analytics {

    public enum ADType {
        interstitial, 
        rewarded, 
        banner
    }
    
    public enum ADAvailability {
        success, 
        not_available
    }

    public enum ADResult {
        started,
        canceled, 
        clicked, 
        watched,
        not_available,
        ad_disabled,
        error, 
        watched_editor,
        offline
    }
    
    public static class ADSAnalytics {

        #region CONSTANTS

        private const string PAR_AD_TYPE = "ad_type";
        private const string PAR_AD_PLACEMENT = "placement";
        private const string PAR_RESULT = "result";
        private const string PAR_CONNECTION = "connection";
        private const string PAR_PROFILE_LEVEL = "account_level";

        #endregion

        private static ProfileLevelInteractor profileLevelInteractor { get; set; }
        private static bool isInitialized => profileLevelInteractor != null;


        private static void InitializeIfNeeded() {
             if (isInitialized)
                 return;

             profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
        }
        

        public static void LogVideoADAvailable(string placecmentId, ADType adType, ADAvailability adAvailability,
            bool isConnected) {
            InitializeIfNeeded();
            
            var eventName = "video_ads_available";

            var parameters = new Dictionary<string, object> {
                {
                    PAR_AD_TYPE, adType.ToString()
                }, {
                    PAR_AD_PLACEMENT, placecmentId
                }, {
                    PAR_RESULT, adAvailability.ToString()
                }, {
                    PAR_CONNECTION, isConnected
                }, {
                   PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel 
                }
            };

            var m_event = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(m_event);
        }


        public static void LogVideoADStarted(string placecmentId, ADType adType, ADResult adResult,
            bool isConnected) {
            InitializeIfNeeded();

            var eventName = "video_ads_started";

            var parameters = new Dictionary<string, object> {
                { PAR_AD_TYPE, adType.ToString() }, 
                { PAR_AD_PLACEMENT, placecmentId }, 
                { PAR_RESULT, adResult.ToString() }, 
                { PAR_CONNECTION, isConnected }, 
                { PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel }
            };

            var m_event = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(m_event);
        }


        public static void LogVideoADResult(string placecmentId, ADType adType, ADResult adResult,
            bool isConnected) {
            InitializeIfNeeded();

            var eventName = "video_ads_watch";

            var parameters = new Dictionary<string, object> {
                {
                    PAR_AD_TYPE, adType.ToString()
                }, {
                    PAR_AD_PLACEMENT, placecmentId
                }, {
                    PAR_RESULT, adResult.ToString()
                }, {
                    PAR_CONNECTION, isConnected
                }, {
                    PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel 
                }
            };

            var m_event = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(m_event);
        }

    }
    
}