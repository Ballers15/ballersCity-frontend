using System;
using System.Collections.Generic;
//using AppsFlyerSDK;
using Orego.Util;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Services {
    public static class CommonAnalytics {

        #region CONSTANTS

        private const string PAR_PROFILE_LEVEL = "account_level";

        #endregion
        
        private static AnalyticsBehavior _appmetrica;

        private static AnalyticsBehavior appMetrica {
            get {
                if (_appmetrica == null) {
                    _appmetrica = new AppMetricaAnalyticsBehavior();
                }

                return _appmetrica;
            }
        }

        private static AnalyticsBehavior _firebase;

        private static AnalyticsBehavior firebase {
            get {
                if (_firebase == null) {
                    _firebase = new FireBaseAnalyticsBehavior();
                }

                return _firebase;
            }
        }

        private static AnalyticsBehavior _myTracker;
       
        private static AnalyticsBehavior myTracker {
            get {
                if (_myTracker == null) {
                    _myTracker = new MyTrackerAnalyticsBehaviour();
                }

                return _myTracker;
            }
        }
        
        private static ProfileLevelInteractor profileLevelInteractor { get; set; }
        private static IdleObjectsInteractor idleObjectsInteractor { get; set; }
        private static bool isInitialized => profileLevelInteractor != null;
        
        private static void InitializeIfNeeded() {
            if (isInitialized)
                return;

            profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
            idleObjectsInteractor = Game.GetInteractor<IdleObjectsInteractor>();
        }
        
        #region GameStarted

        public static void LogGameStarted() {
            InitializeIfNeeded();
            
            var localDeviceTimeHours = DateTime.Now.Hour;
            var offlineTimeHours =
                (int) GameTime.timeSinceLastSessionEndedToCurrentSessionStartedSeconds / 3600;
            var lifetimeHours = (int) GameTime.timeSinceGameInstalledHours;
            Log(new AnalyticsEvent("game_started", new Dictionary<string, object> {
                {
                    AnalyticsKeys.LOCAL_TIME_HOURS, localDeviceTimeHours
                }, {
                    AnalyticsKeys.OFFLINE_TIME_HOURS, offlineTimeHours
                }, {
                    AnalyticsKeys.LIFETIME_HOURS, lifetimeHours
                }
            } ));
        }

        #endregion


        public static void LogArrowsClicked(int fromIndex, int toIndex) {
            const string EVENT_KEY = "arrows_clicked";
            var transition = $"{fromIndex + 1}->{toIndex + 1}";
#if DEBUG
            Debug.Log("ARROWCLICK: " + transition);
#endif
            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.SPECIFIC_ARROW, transition
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

       
        public static void LogObjectBuilt(string objectId) {
            InitializeIfNeeded();
            var idleObject = idleObjectsInteractor.GetIdleObject(objectId);
            var eventName = "build_object";
            var parNameObjectId = "object_id";
            
            var parameters = new Dictionary<string, object> {{
                    parNameObjectId, idleObject.id
                }, {
                    PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel
                }
            };
            
            var _event = new AnalyticsEvent(eventName, parameters);
            Log(_event);
        }

        public static void LogGemsReceived(int amount, int currentGemsCount) {
            const string EVENT_KEY = "gems_received";

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.CURRENT_GEMS_COUNT, currentGemsCount
                }, {
                    AnalyticsKeys.AMOUNT, amount
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogGemsSpent(int amount, int currentGemsCount) {
            const string EVENT_KEY = "gems_spent";

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.CURRENT_GEMS_COUNT, currentGemsCount
                }, {
                    AnalyticsKeys.AMOUNT, amount
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogIdleObjectLevelRisen(string objectId, int level) {
            var eventName = "upgrade_object";
            var parNameObjectId = "object_id";
            var parNameLevel = "object_level";
            
            var idleObject = idleObjectsInteractor.GetIdleObject(objectId);

            var parameters = new Dictionary<string, object> {
                {
                    parNameObjectId, idleObject.id
                }, {
                    parNameLevel, level
                }, {
                    PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel
                }
            };
            
            var _event = new AnalyticsEvent(eventName, parameters);
            Log(_event);
        }

        public static void LogXtraManagementTabOpened() {
            const string EVENT_KEY = "xtra_management_tab_opened";
            appMetrica.LogEvent(EVENT_KEY);
            firebase.LogEvent(EVENT_KEY);
            myTracker.LogEvent(EVENT_KEY);
        }

        public static void LogXtraManagementTabStarted() {
            const string EVENT_KEY = "xtra_management_tab_started";
            appMetrica.LogEvent(EVENT_KEY);
            firebase.LogEvent(EVENT_KEY);
            myTracker.LogEvent(EVENT_KEY);
        }

        public static void LogBluePrintTabOpened() {
            const string EVENT_KEY = "blue_print_tab_opened";
            appMetrica.LogEvent(EVENT_KEY);
            firebase.LogEvent(EVENT_KEY);
            myTracker.LogEvent(EVENT_KEY);
        }

        public static void LogQuestsTabOpened(bool isAnyQuestCompleted) {
            const string EVENT_KEY = "quest_tab_opened";
            var anyQuestCompleted = isAnyQuestCompleted ? 1 : 0;

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.ANY_QUEST_COMPLETED, anyQuestCompleted
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogShopTabOpened(string buttonId, int currentGemsCount) {
            const string EVENT_KEY = "shop_tab_opened";

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.SOURCE_BUTTON_ID, buttonId
                }, {
                    AnalyticsKeys.CURRENT_GEMS_COUNT, currentGemsCount
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogCaseReceived(string caseId, bool wasPurchased, int currentCaseCount) {
            const string EVENT_KEY = "case_received";
            var purchased = wasPurchased ? 1 : 0;

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.CASE_ID, caseId
                }, {
                    AnalyticsKeys.WAS_PURCHASED, purchased
                }, {
                    AnalyticsKeys.CURRENT_CASE_COUNT, currentCaseCount
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogCaseOpened(string caseId, IEnumerable<RewardInfoEcoClicker> rewardsInfoSet) {
            var eventName = "open_case";
            var parNameId = "case_type";
            var parNameHardCount = "reward_hard";
            var parNameCardId = "worker_id";
            var parNameCardCount = "worker_value";
            var parNameTBoosterId = "booster_type";
            var parNameTBoosterCount = "booster_value";

            var hardCount = "0";
            var cardId = "";
            var cardsCount = "0";
            var boosterId = "";
            var boostersCount = "0";

            foreach (RewardInfoEcoClicker rewardInfo in rewardsInfoSet) {
                if (rewardInfo is RewardInfoHardCurrency || rewardInfo is RewardInfoSetupHardCurrency) {
                    hardCount = rewardInfo.GetCountToString();
                    continue;
                }

                if (rewardInfo is RewardInfoCard || rewardInfo is RewardInfoSetupCard) {
                    var cardsInteractor = Game.GetInteractor<CardsInteractor>();
                    if (rewardInfo is RewardInfoCard ripc) {
                        var card = cardsInteractor.GetCard(ripc.cardId);
                        cardId = card.id;
                    }
                    else if (rewardInfo is RewardInfoSetupCard rispc) {
                        var card = cardsInteractor.GetCard(rispc.cardId);
                        cardId = card.id;
                    }

                    cardsCount = rewardInfo.GetCountToString();
                    continue;
                }

                if (rewardInfo is RewardInfoTimeBooster ritb) {
                    boostersCount = rewardInfo.GetCountToString();
                    boosterId = ritb.id;
                }
            }
            
            var parameters = new Dictionary<string, object> {
                { parNameId, caseId },
                { parNameHardCount, hardCount },
                { parNameCardId, cardId },
                { parNameCardCount, cardsCount },
                { parNameTBoosterId, boosterId },
                { parNameTBoosterCount, boostersCount },
                { PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel }
            };
            
            
            var _event = new AnalyticsEvent(eventName, parameters);
            Log(_event);
        }

        #region TIME BOOSTERS

        public static void LogTimeBoosterReceived(string timeBoosterId, bool wasPurchased,
            int currentTimeBoosterCount) {
            const string EVENT_KEY = "time_booster_received";
            var purchased = wasPurchased ? 1 : 0;

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.TIME_BOOSTER_ID, timeBoosterId
                }, {
                    AnalyticsKeys.WAS_PURCHASED, purchased
                }, {
                    AnalyticsKeys.CURRENT_TIME_BOOSTER_COUNT, currentTimeBoosterCount
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogTimeBoosterUsed(string timeBoosterId) {
            var eventName = "boosters";
            var parNameBoosterId = "booster_type";
            
            var parameters = new Dictionary<string, object> {
                { parNameBoosterId, timeBoosterId },
                { PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel }
            };

            var _event = new AnalyticsEvent(eventName, parameters);
            Log(_event);
        }

        #endregion

       

        public static void LogProductPurchased(
            string productId,
            string paymentType,
            int currentGemsCount,
            bool isSuccess
        ) {
            const string EVENT_KEY = "product_purchased";
            var success = isSuccess ? 1 : 0;

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.PRODUCT_ID, productId
                }, {
                    AnalyticsKeys.PAYMENT_TYPE, paymentType
                }, {
                    AnalyticsKeys.CURRENT_GEMS_COUNT, currentGemsCount
                }, {
                    AnalyticsKeys.SUCCESS, success
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }
        
#if UNITY_ANDROID      
        public static void LogPurchase(PurchaseEventArgs args) {
            var currencyName = args.purchasedProduct.metadata.isoCurrencyCode;
            var revenue = args.purchasedProduct.metadata.localizedPrice;
            var productId = args.purchasedProduct.definition.id;
            var transactionId = args.purchasedProduct.transactionID;
            
            var pairs = new Dictionary<string, string> ();
            pairs.Add(AFInAppEvents.CURRENCY, currencyName);
            pairs.Add(AFInAppEvents.REVENUE, revenue.ToString());
            pairs.Add(AFInAppEvents.QUANTITY, "1");
            pairs.Add("SKU", productId);
            pairs.Add("TRANSACTION_ID", transactionId);
            AppsFlyer.sendEvent("af_purchase", pairs);
        }
#endif
        
        public static void LogGamePause(double sessionTime, bool isPaused) {
            const string EVENT_KEY = "game_pause";
            var paused = isPaused ? 1 : 0;

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.SESSION_TIME, sessionTime
                }, {
                    AnalyticsKeys.PAUSED, paused
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogRewardReceived(string rewardId, bool success) {
            const string EVENT_KEY = "reward_received";
            var isSuccess = success
                ? 1
                : 0;

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.REWARD_ID, rewardId
                }, {
                    AnalyticsKeys.SUCCESS, isSuccess
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        #region QUESTS

        public static void LogMainQuestStarted(string questId, bool fromBeginning) {
            const string EVENT_KEY = "main_quest_started";

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.QUEST_ID, questId
                }, {
                    AnalyticsKeys.FROM_BEGINNING, fromBeginning
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogMainQuestEnd(string questId) {
            const string EVENT_KEY = "main_quest_end";

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.QUEST_ID, questId
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogMainQuestRewardReceived(Quest quest) {
            LogFinishQuestEvent(quest);
        }

        public static void LogDailyQuestStarted(string questId) {
            const string EVENT_KEY = "daily_quest_started";

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.QUEST_ID, questId
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogDailyQuestEnd(string questId) {
            const string EVENT_KEY = "daily_quest_end";

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.QUEST_ID, questId
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogDailyQuestRewardReceived(Quest quest) {
            LogFinishQuestEvent(quest);
        }

        private static void LogFinishQuestEvent(Quest quest) {
            var questId = quest.id;
            var rewardInfo = ((QuestInfoEcoClicker) quest.info).rewardInfo;
            var rewardId = rewardInfo.id;
            var count = rewardInfo.GetCountToString();

            var eventName = "quests";
            var parNameQuestId = "quest_id";
            var parNameRewardId = "reward_type";
            var parNameRewardValue = "reward_value";
           
            var parameters = new Dictionary<string, object> {
                {parNameQuestId, questId}, 
                {parNameRewardId, rewardId},
                {parNameRewardValue, count},
                {PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel}
            };
            
            var _event = new AnalyticsEvent(eventName, parameters);
            Log(_event);
        }

        #endregion

        

        public static void LogPersCardReceived(string cardId, int level, int countTotal,
            int currentGemsCount) {
            const string EVENT_KEY = "pers_card_received";

            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.CARD_ID, cardId
                }, {
                    AnalyticsKeys.CARD_LEVEL, level
                }, {
                    AnalyticsKeys.COUNT_TOTAL, countTotal
                }, {
                    AnalyticsKeys.CURRENT_GEMS_COUNT, currentGemsCount
                }
            };

            appMetrica.LogEvent(EVENT_KEY, parameters);
            firebase.LogEvent(EVENT_KEY, parameters);
            myTracker.LogEvent(EVENT_KEY, parameters);
        }

        public static void LogPersLevelRisen(string cardId, int level) {
            var interactor = Game.GetInteractor<CardsInteractor>();
            var card = interactor.GetCard(cardId);
            var objectId = card.id;
            var idleObject = idleObjectsInteractor.GetIdleObject(objectId);
            var idleObjectNumber = idleObject.info.number;
            var fullCardId = $"{idleObjectNumber}_{cardId}";
            
            var eventName = "upgrade_worker";
            var parNameWorkerId = "worker_id";
            var parNameLevel = "worker_level";
            
            var parameters = new Dictionary<string, object> {
                {parNameWorkerId, fullCardId},
                {parNameLevel, level},
                {PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel}
            };
            
            var _event = new AnalyticsEvent(eventName, parameters);
            Log(_event);
        }

        public static void LogNotificationHandled(string notificationId, string statusId) {
            var parameters = new Dictionary<string, object> {
                {
                    AnalyticsKeys.NOTIFICATION_ID, notificationId
                }, {
                    AnalyticsKeys.NOTIFICATION_STATUS_ID, statusId
                }
            };

            appMetrica.LogEvent(AnalyticsKeys.NOTIFICATION_HANDLED, parameters);
            firebase.LogEvent(AnalyticsKeys.NOTIFICATION_HANDLED, parameters);
            myTracker.LogEvent(AnalyticsKeys.NOTIFICATION_HANDLED, parameters);
        }

        #region PopupPersonageCard

        public static void LogPopupPersonageCardOpened(bool readyForUpgrade, bool enoughGems) {
            const string EVENT_NAME = "pers_card_popup_opened";
            const string PARAMETER_READY_FOR_UPGRADE = "ready_for_upgrade";
            const string PAREMETER_ENOUGH_GEMS = "enough_gems";

            int intReadyForUpgrade = readyForUpgrade.ToInt();
            int intEnoughGems = enoughGems.ToInt();

            var parameters = new Dictionary<string, object> {
                {
                    PARAMETER_READY_FOR_UPGRADE, intReadyForUpgrade
                }, {
                    PAREMETER_ENOUGH_GEMS, intEnoughGems
                }
            };

            appMetrica.LogEvent(EVENT_NAME, parameters);
            firebase.LogEvent(EVENT_NAME, parameters);
        }

        public static void LogPopupPersonageCardClosed(bool enoughGems, bool upgraded) {
            const string EVENT_NAME = "pers_card_popup_closed";
            const string PAREMETER_ENOUGH_GEMS = "enough_gems";
            const string PARAMETER_UPGRADED = "upgraded";

            int intUpgraded = upgraded.ToInt();
            int intEnoughGems = enoughGems.ToInt();

            var parameters = new Dictionary<string, object> {
                {
                    PARAMETER_UPGRADED, intUpgraded
                }, {
                    PAREMETER_ENOUGH_GEMS, intEnoughGems
                }
            };

            appMetrica.LogEvent(EVENT_NAME, parameters);
            firebase.LogEvent(EVENT_NAME, parameters);
            myTracker.LogEvent(EVENT_NAME, parameters);
        }

        #endregion

        public static void LogFacebookSubscriptionClicked() {
            const string EVENT_KEY = "fb_subscription_clicked";
            appMetrica.LogEvent(EVENT_KEY);
            firebase.LogEvent(EVENT_KEY);
            myTracker.LogEvent(EVENT_KEY);
        }

        public static void LogInstagramSubscriptionClicked() {
            const string EVENT_KEY = "insta_subscription_clicked";
            appMetrica.LogEvent(EVENT_KEY);
            firebase.LogEvent(EVENT_KEY);
            myTracker.LogEvent(EVENT_KEY);
        }

        public static void Log(AnalyticsEvent analyticsEvent) {
            if (analyticsEvent.isEmpty) {
                appMetrica.LogEvent(analyticsEvent.eventName);
                firebase.LogEvent(analyticsEvent.eventName);
                myTracker.LogEvent(analyticsEvent.eventName);
            }
            else {
                appMetrica.LogEvent(analyticsEvent.eventName, analyticsEvent.parameters);
                firebase.LogEvent(analyticsEvent.eventName, analyticsEvent.parameters);
                myTracker.LogEvent(analyticsEvent.eventName, analyticsEvent.parameters);
            }
        }
    }
}