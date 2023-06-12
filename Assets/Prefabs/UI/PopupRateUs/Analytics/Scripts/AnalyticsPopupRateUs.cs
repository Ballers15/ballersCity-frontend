using System.Collections.Generic;
using SinSity.Domain;
using SinSity.Services;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace Prefabs.UI.PopupRateUs.Analytics {
    public static class AnalyticsPopupRateUs {
        
        #region CONSTANTS

        private const string PAR_MODERNIZATION_LEVEL = "reset_number";
        private const string PAR_PROFILE_LEVEL = "account_level";

        #endregion
        
        
        private static ProfileLevelInteractor profileLevelInteractor { get; set; }
        private static bool isInitialized => profileLevelInteractor != null;
        private static string placementId;
        
        private static void InitializeIfNeeded() {
            if (isInitialized)
                return;

            profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
        }

        public static void LogPopupShown(string _placementId) {
            placementId = _placementId;
            var eventName = "rate_us_popup_show";
            var analyticsEvent = new AnalyticsEvent(eventName);
            CommonAnalytics.Log(analyticsEvent);
        }

        public static void LogPopupClosed(UIPopupResult result, bool isNeverRemindChecked) {
            InitializeIfNeeded();
            
            var eventName = "rate_us";
            var parNameReason = "show_reason";
            var parNameResult = "rate_result";
            var parNameNeverRemind = "never_remind";

            var resultValue = result == UIPopupResult.Apply ? 5 : result == UIPopupResult.Close ? 1 : 0;
            
            var parameters = new Dictionary<string, object> {{
                    parNameResult, resultValue
                }, {
                    parNameNeverRemind, isNeverRemindChecked.ToString()
                }, {
                   parNameReason, placementId 
                }, {
                    PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel 
                }
            };
            
            var analyticsEvent = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(analyticsEvent);
        }
        
    }
}