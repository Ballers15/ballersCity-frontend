using System;
using VavilichevGD.LocalizationFramework;

namespace SinSity.Notifications {
    public class AndroidNotificationDataComeback : AndroidNotificationData {
        
        #region Constants

        private const string ID_NOTIFICATION = "push_comeback";
        private const string ID_PUSH_TITLE = "ID_PUSH_TITLE_COMEBACK";
        private const string ID_PUSH_TEXT = "ID_PUSH_COMEBACK";
        private const string ID_ICON_SMALL = "icon_small";
        private const string ID_ICON_LARGE = "icon_large_comeback";

        public const string PREFKEY_PREFIX = "PUSH_COMEBACK_";

        #endregion

        private int remainingTimeHours;


        #region Initialize

        public AndroidNotificationDataComeback(int remainingTimeHours) {
            this.remainingTimeHours = remainingTimeHours;
            this.Initialize();
        }

        private void Initialize() {
            this.fireTime = GetResearchNotificationTime();
            this.title = Localization.GetTranslation(ID_PUSH_TITLE);
            this.text = Localization.GetTranslation(ID_PUSH_TEXT);
            this.smallIconId = ID_ICON_SMALL;
            this.largeIconId = ID_ICON_LARGE;
            this.id = $"{ID_NOTIFICATION}_{this.remainingTimeHours}";
            this.allowToCreate = true;
        }
        
        private DateTime GetResearchNotificationTime() {
            var notificationDateTime = DateTime.Now.AddHours(this.remainingTimeHours);
            var clampedNotificationDateTime = this.ClampNotificationTime(notificationDateTime);
            return clampedNotificationDateTime;
        }

        #endregion

        public override string GetPrefKey() {
            return $"{PREFKEY_PREFIX}{this.remainingTimeHours}";
        }
    }
}