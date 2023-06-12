using System.Collections.Generic;
using System.Globalization;
using DevToDev;
//using Firebase.Analytics;
//using Mycom.Tracker.Unity;
using SinSity.Domain;
using SinSity.Services;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Tools
{
    public static class GemTreeAnalytics
    {

        #region CONSTANTS

        private const string PAR_PROFILE_LEVEL = "account_level";

        #endregion
        
        private static ProfileLevelInteractor profileLevelInteractor { get; set; }
        private static bool isInitialized => profileLevelInteractor != null;
        
        public static void InitializeIfNeeded() {
            if (isInitialized)
                return;

            profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
        }
        
        private static int collectInWindow;

        public static void LogTreeWindowOpened()
        {
            const string EVENT_KEY = "tree_window_opened";
            //AppMetrica.Instance.ReportEvent(EVENT_KEY);
            //FirebaseAnalytics.LogEvent(EVENT_KEY);
            DevToDev.Analytics.CustomEvent(EVENT_KEY);
        }

        public static void LogTreeUpgradeBought(int progress, int levelIndex)
        {
            const string EVENT_KEY = "tree_upgrade_bought";
            const string COUNT_KEY = "count";
            const string LEVEL_UP_KEY = "level_up";
            levelIndex++;
            collectInWindow++;
            /*AppMetrica.Instance.ReportEvent(EVENT_KEY, new Dictionary<string, object>
            {
                {
                    COUNT_KEY, progress
                },
                {
                    LEVEL_UP_KEY, levelIndex
                }
            });*/
            /*FirebaseAnalytics.LogEvent(EVENT_KEY,
                new Parameter
                (
                    COUNT_KEY, progress
                ),
                new Parameter
                (
                    LEVEL_UP_KEY, levelIndex
                )
            );*/
            DevToDev.Analytics.CustomEvent(EVENT_KEY, new CustomEventParams()
                .NewParam(COUNT_KEY, progress)
                .NewParam(LEVEL_UP_KEY, levelIndex)
            );
            
            /*MyTracker.TrackEvent(EVENT_KEY, new Dictionary<string, string>
            {
                {
                    COUNT_KEY, progress.ToString()
                },
                {
                    LEVEL_UP_KEY, levelIndex.ToString()
                }
            });*/
        }

        public static void LogTreeWindowClosed()
        {
            const string EVENT_KEY = "tree_window_closed";
            const string REWARD_COLLECTED_KEY = "reward_collected";
            const string LIFETIME_KEY = "lifetime";
            var lifetime = GameTime.timeSinceGameInstalledHours;
            /*AppMetrica.Instance.ReportEvent(EVENT_KEY, new Dictionary<string, object>
            {
                {
                    REWARD_COLLECTED_KEY, collectInWindow
                },
                {
                    LIFETIME_KEY, lifetime
                }
            });*/
            /*FirebaseAnalytics.LogEvent(EVENT_KEY,
                new Parameter
                (
                    REWARD_COLLECTED_KEY, collectInWindow
                ),
                new Parameter(
                    LIFETIME_KEY, lifetime
                )
            );*/
            DevToDev.Analytics.CustomEvent(EVENT_KEY, new CustomEventParams()
                .NewParam(REWARD_COLLECTED_KEY, collectInWindow)
                .NewParam(LIFETIME_KEY, lifetime)
            );
            /*MyTracker.TrackEvent(EVENT_KEY, new Dictionary<string, string>
            {
                {
                    REWARD_COLLECTED_KEY, collectInWindow.ToString()
                },
                {
                    LIFETIME_KEY, lifetime.ToString(CultureInfo.CurrentCulture)
                }
            });*/

            collectInWindow = 0;
        }

        public static void LogLevelUpEvent(int newLevelNumber) {
            InitializeIfNeeded();

            var eveneName = "life_tree";
            var parNameLevel = "level_up";
            
            var parameters = new Dictionary<string, object> {
                { parNameLevel, newLevelNumber },
                { PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel }
            };

            var _event = new AnalyticsEvent(eveneName, parameters);
            CommonAnalytics.Log(_event);
        }
    }
}