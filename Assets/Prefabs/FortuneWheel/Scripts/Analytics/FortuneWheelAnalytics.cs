using System.Collections.Generic;
using SinSity.Domain;
using SinSity.Services;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Analytics {
    public static class FortuneWheelAnalytics {

        #region CONSTANTS

        private const string PARAMETER_PAYMENT_TYPE = "payment_type";
        private const string PARAMETER_PRICE = "price";
        private const string PAR_PROFILE_LEVEL = "account_level";

        #endregion
        
        private static ProfileLevelInteractor profileLevelInteractor { get; set; }
        private static bool isInitialized => profileLevelInteractor != null;

        private static string s_spinType;
        private static int s_spinPrice;
        
        
        private static void InitializeIfNeeded() {
            if (isInitialized)
                return;

            profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
        }


        public static void LogUnlocked() {
            var lifeTime = GameTime.timeSinceGameInstalledHours;
            var eventName = "fortune_unlocked";
            var parameterName = "lifetime";
            
            var parameters = new Dictionary<string, object> {
                {parameterName, lifeTime}
            };
            
            var m_event = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(m_event);
        }

        public static void LogPopupFortuneWheelShown(PaymentType paymentType, int gemsPrice = 0) {
            var eventName = "fortune_popup_show";
            var price = paymentType == PaymentType.HardCurrency ? gemsPrice : 0;
            
            var parameters = new Dictionary<string, object> {
                {PARAMETER_PAYMENT_TYPE, paymentType.ToString()},
                {PARAMETER_PRICE, price.ToString()}
            };

            var m_event = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(m_event);
        }

        
        public static void LogFortuneWheelRotate(PaymentType paymentType, int gemsPrice = 0) {
            s_spinType = paymentType.ToString();
            s_spinPrice = paymentType == PaymentType.HardCurrency ? gemsPrice : 0;
        }

        
        public static void LogFortuneWheelRotateComplete(Reward reward) {
            LogFortuneWheelSpin(s_spinType, s_spinPrice, reward.id, reward.info.GetCountToString());
        }

        
        public static void LogPopupFortuneWheelRewardClosed() {
            var eventName = "fortune_reward_popup_close";
            var m_event = new AnalyticsEvent(eventName);
            CommonAnalytics.Log(m_event);
        }

        private static void LogFortuneWheelSpin(string spinType, int spinPrice, string rewardId, string rewardCount) {
            InitializeIfNeeded();
            
            var eventName = "fortune_wheel";
            var parNameSpinType = "spin_type";
            var parNameSpinPrice = "spin_price";
            var parNameRewardId = "reward_type";
            var parNameRewardCount = "reward_value";
            
            var parameters = new Dictionary<string, object> {
                { parNameSpinType, spinType },
                { parNameSpinPrice, spinPrice },
                { parNameRewardId, rewardId },
                { parNameRewardCount, rewardCount },
                { PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel }
            };
            
            var _event = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(_event);
        }
    }
}