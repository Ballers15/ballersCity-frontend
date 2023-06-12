using System;

namespace SinSity.Notifications {
    public abstract class AndroidNotificationData {

        #region Constants

        private const int HOURS_MAX = 22;
        private const int HOURS_MIN_WEEKEND = 10;
        private const int HOURS_MIN_WEEKDAY = 9;

        #endregion
       

        public string id;
        public string title;
        public string text;
        public string smallIconId;
        public string largeIconId;
        public DateTime fireTime;
        public bool allowToCreate { get; protected set; }

        public abstract string GetPrefKey();
        
        protected DateTime ClampNotificationTime(DateTime notificationTime) {
            DateTime clampedNotificationTime = notificationTime;
            DateTime trasholdTodayTime = clampedNotificationTime.ChangeHours(HOURS_MAX);
            DateTime trasholdTommorowTime = ItIsWeekend(clampedNotificationTime)
                ? clampedNotificationTime.AddDays(1).ChangeHours(HOURS_MIN_WEEKEND)
                : clampedNotificationTime.AddDays(1).ChangeHours(HOURS_MIN_WEEKDAY);
        
            if (clampedNotificationTime > trasholdTodayTime && clampedNotificationTime < trasholdTommorowTime) {
                clampedNotificationTime = ItIsWeekend(clampedNotificationTime)
                    ? trasholdTommorowTime.ChangeHours(HOURS_MIN_WEEKEND)
                    : trasholdTommorowTime.ChangeHours(HOURS_MIN_WEEKDAY);
            }

            return clampedNotificationTime;
        }
        
        private bool ItIsWeekend(DateTime localTime) {
            return (int) localTime.DayOfWeek > (int) DayOfWeek.Saturday;
        }
    }
}