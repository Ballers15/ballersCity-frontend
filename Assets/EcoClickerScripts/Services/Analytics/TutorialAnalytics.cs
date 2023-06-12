using System;
using System.Collections.Generic;
using DevToDev;
//using Firebase.Analytics;
//using Mycom.Tracker.Unity;
using VavilichevGD.Tools;

namespace SinSity.Services
{
    public static class TutorialAnalytics
    {
        #region Const

        private const string START_TIME_KEY = "tutorial_start_time";

        private const string LIFETIME_15S_KEY = "lifetime_15s";

        private const string LIFETIME_1MIN_KEY = "lifetime_15min";

        #endregion

        private static DateTimeSerialized startTime;

        public static void Start()
        {
            startTime = Storage.GetCustom(START_TIME_KEY, new DateTimeSerialized(DateTime.Now));
        }

        public static void LogCollectTimesFromFirstBuildingInSecondStage(int times)
        {
            const string EVENT_KEY = "tutor_first_object_income_collect";
            const string PARAM_KEY = "building_1_income_count";
            /*AppMetrica.Instance.ReportEvent(EVENT_KEY, new Dictionary<string, object>
            {
                {
                    PARAM_KEY, times
                }
            });*/
            /*FirebaseAnalytics.LogEvent(EVENT_KEY,
                new Parameter
                (
                    PARAM_KEY, times
                )
            );*/
            /*MyTracker.TrackEvent(EVENT_KEY, new Dictionary<string, string>
            {
                {
                    PARAM_KEY, times.ToString()
                }
            });*/
        }

        public static void LogTutorialEventInSec(string eventKey, int tutorialStep)
        {
            LogEventInSec(eventKey);
            DevToDev.Analytics.Tutorial(tutorialStep);
        }

        public static void LogEventInSec(string eventKey)
        {
            var now = DateTime.Now;
            var diffBetweenStart = now - startTime.GetDateTime();
            var diffSeconds = diffBetweenStart.Seconds;
            var roundedSeconds = diffSeconds - diffSeconds % 15;
            /*AppMetrica.Instance.ReportEvent(eventKey, new Dictionary<string, object>
            {
                {
                    LIFETIME_15S_KEY, roundedSeconds
                }
            });*/
            /*FirebaseAnalytics.LogEvent(eventKey,
                new Parameter
                (
                    LIFETIME_15S_KEY, roundedSeconds
                )
            );*/
            DevToDev.Analytics.CustomEvent(eventKey, new CustomEventParams()
                .NewParam(LIFETIME_15S_KEY, roundedSeconds)
            );
            /*MyTracker.TrackEvent(eventKey, new Dictionary<string, string>
            {
                {
                    LIFETIME_15S_KEY, roundedSeconds.ToString()
                }
            });*/
        }

        public static void LogEventInMin(string eventKey)
        {
            var nowDate = DateTime.Now;
            var startDate = startTime.GetDateTime();
            var diffBetweenStart = nowDate - startDate;
            var minutes = diffBetweenStart.Minutes;
            /*AppMetrica.Instance.ReportEvent(eventKey, new Dictionary<string, object>
            {
                {
                    LIFETIME_1MIN_KEY, minutes
                }
            });*/
            /*FirebaseAnalytics.LogEvent(eventKey,
                new Parameter
                (
                    LIFETIME_1MIN_KEY, minutes
                )
            );*/
            DevToDev.Analytics.CustomEvent(eventKey, new CustomEventParams()
                .NewParam(LIFETIME_1MIN_KEY, minutes)
            );
            /*MyTracker.TrackEvent(eventKey, new Dictionary<string, string>
            {
                {
                    LIFETIME_1MIN_KEY, minutes.ToString()
                }
            });*/
        }

        public static void LogReceiveFirstMainQuestReward()
        {
            const string EVENT_KEY = "hint_mission_window_reward_pickup_main_quest";
            if (Storage.HasObject(EVENT_KEY))
            {
                return;
            }

            Storage.SetCustom(EVENT_KEY, true);
            LogEventInMin(EVENT_KEY);
        }

        public static void LogReceiveFirstMiniQuestReward()
        {
            const string EVENT_KEY = "hint_mission_window_reward_pickup_mini_quest";
            if (Storage.HasObject(EVENT_KEY))
            {
                return;
            }

            Storage.SetCustom(EVENT_KEY, true);
            LogEventInMin(EVENT_KEY);
        }
    }
}