using System.Collections.Generic;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Services;
using VavilichevGD.Architecture;

namespace SinSity.Tools {
    public static class ResearchAnalytics {
        
        #region Const

        private const string PAR_PROFILE_LEVEL = "account_level";

        #endregion

        private static ProfileLevelInteractor profileLevelInteractor { get; set; }
        private static bool isInitialized => profileLevelInteractor != null;

        public static void InitializeIfNeeded() {
            if (isInitialized)
                return;

            profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
        }

        public static void LogResearchWindowOpened() {
            const string EVENT_KEY = "research_window_opened";
            var _event = new AnalyticsEvent(EVENT_KEY);
            CommonAnalytics.Log(_event);
        }

        public static void LogResearchStartedNoAds(ResearchObject researchObject) {
            LogResearchEvent("start", researchObject);
        }


        public static void LogResearchStartedAds(ResearchObject researchObject, bool isAdWatched) {
            if (isAdWatched)
                LogResearchEvent("start", researchObject);
        }

        public static void LogResearchRewardCollected(ResearchObject researchObject) {
            LogResearchEvent("completed", researchObject);
        }


        private static void LogResearchEvent(string action, ResearchObject research) {
            InitializeIfNeeded();

            var eventName = "research";
            var parNameAction = "action";
            var parNameId = "research_id";
            var parNameDuration = "time";
            var parNamePriceType = "currency";

            var priceType = research.info is ResearchObjectInfoComplex ? "ads" : "soft";

            var parameters = new Dictionary<string, object> {
                {parNameAction, action},
                {parNameId, research.info.id},
                {parNameDuration, research.info.durationSeconds},
                {parNamePriceType, priceType},
                {PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel}
            };

            var _event = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(_event);
        }
    }
}