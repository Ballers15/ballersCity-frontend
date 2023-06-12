using System;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;

namespace SinSity.Notifications {
    public class AndroidNotificationDataDailyReward : AndroidNotificationData {

        #region Constants

        private const string ID_NOTIFICATION = "push_daily_reward";
        private const string ID_PUSH_TITLE = "ID_PUSH_TITLE_DAILY";
        private const string ID_PUSH_TEXT = "ID_PUSH_DAILY";
        private const string ID_ICON_SMALL = "icon_small";
        private const string ID_ICON_LARGE = "icon_large_daily";
        
        public const string PREFKEY = "PUSH_DAILY_REWARD";

        #endregion
        
        private DailyRewardInteractor dailyRewardInteractor;


        #region Initialize

        public AndroidNotificationDataDailyReward(DailyRewardInteractor dailyRewardInteractor) {
            this.dailyRewardInteractor = dailyRewardInteractor;
            this.allowToCreate = dailyRewardInteractor.lastRewardWasReceived;
            
            if (this.allowToCreate)
                this.Initialize();
        }
        
        private void Initialize() {
            this.fireTime = this.GetDailyRewardNotificationTime();
            this.title = Localization.GetTranslation(ID_PUSH_TITLE);
            this.text = Localization.GetTranslation(ID_PUSH_TEXT);
            this.smallIconId = ID_ICON_SMALL;
            this.largeIconId = ID_ICON_LARGE;
            this.id = ID_NOTIFICATION;
        }
        
        private DateTime GetDailyRewardNotificationTime() {
        
            if (!dailyRewardInteractor.lastRewardWasReceived)
                return DateTime.Now.AddHours(DailyRewardInteractor.TIME_DIFF_BETWEEN_REWARDS_HOURS);

            var lastRewardDateTime = dailyRewardInteractor.lastRewardDateTime;
            var nowDateTime = GameTime.now;
            var hoursToNextDailyReward = DailyRewardInteractor.TIME_DIFF_BETWEEN_REWARDS_HOURS - Mathf.FloorToInt((float)(nowDateTime - lastRewardDateTime).TotalHours);
            hoursToNextDailyReward = Mathf.Max(1, hoursToNextDailyReward);
            var notificationDateTime = DateTime.Now.AddHours(hoursToNextDailyReward); // Сегодня игрок зашел и получил награду, и завтра у него будет напоминалка примерно в это же время.

            var clampedNotificationDateTime = this.ClampNotificationTime(notificationDateTime);
            return clampedNotificationDateTime;
        }

        #endregion
        
        public override string GetPrefKey() {
            return PREFKEY;
        }
    }
}