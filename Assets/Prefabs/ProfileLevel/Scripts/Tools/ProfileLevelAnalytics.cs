using System.Collections.Generic;
using System.Globalization;
using DevToDev;
using SinSity.Domain;
//using Firebase.Analytics;
//using Mycom.Tracker.Unity;
using SinSity.Services;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Tools {
    public static class ProfileLevelAnalytics {

        #region CONSTANTS

        private const string PAR_PROFILE_LEVEL = "account_level";
        private const string PAR_PROFILE_SOURCE = "sourceId";

        #endregion

        public static void LogPlayerLevelUp(int levelNumber) {
            
            var eventName = "level_up";
            var parNameLevel = "level_up";

            var parameters = new Dictionary<string, object> {
                { parNameLevel, levelNumber.ToString() }, 
                { PAR_PROFILE_LEVEL, levelNumber.ToString() }, 
            };
            
            var analyticsEvent = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(analyticsEvent);
        }

        public static void LogPlayerExpAdded(int levelNumber, string sourceId) {
            var eventName = "player_exp_added";

            var parameters = new Dictionary<string, object> {{
                    PAR_PROFILE_LEVEL, levelNumber.ToString()
                }, {
                    PAR_PROFILE_SOURCE, sourceId
                }
            };
            
            var analyticsEvent = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(analyticsEvent);
        }
    }
}