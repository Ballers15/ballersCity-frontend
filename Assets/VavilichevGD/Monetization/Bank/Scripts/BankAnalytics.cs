using System.Collections.Generic;
using SinSity.Domain;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;

namespace VavilichevGD.Monetization {
    public static class BankAnalytics {
        #region CONSTANTS

        private const string PAR_PROFILE_LEVEL = "account_level";
        private const string PAR_CATEGORY = "category";
        private const string PAR_ITEM_ID = "item_id";
        private const string PAR_VALUE = "value";

        #endregion
        
        private static ProfileLevelInteractor profileLevelInteractor { get; set; }
        private static bool isInitialized => profileLevelInteractor != null;
        
        public static void Initialize() {
            if (isInitialized)
                return;

            profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
            
            Bank.OnHardCurrencyReceivedInstantlyEvent += OnHardCurrencyReceived;
            Bank.OnHardCurrencySpentInstantlyEvent += OnHardCurrencySpent;
        }

        private static void OnHardCurrencyReceived(int receivedhardcurrency, object sender) {
            var category = sender is UIWidgetGems ? "buy" : "income";
            var reason = sender.ToString();
            LogHardCurrencyEvent(receivedhardcurrency, category, reason);
        }
        
        private static void OnHardCurrencySpent(int spenthardcurrency, object sender) {
            var category = "spend";
            var reason = sender.ToString();
            LogHardCurrencyEvent(-spenthardcurrency, category, reason);
        }

        private static void LogHardCurrencyEvent(int hardCurrencyCount, string category, string reason) {
            
            var eventName = "hard_currency";

            var parameters = new Dictionary<string, object> {
                {
                    PAR_CATEGORY, category
                }, {
                    PAR_ITEM_ID, reason
                }, {
                    PAR_VALUE, hardCurrencyCount
                }, {
                    PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel 
                }
            };
            
            var m_event = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(m_event);
        }
        
        
    }
}