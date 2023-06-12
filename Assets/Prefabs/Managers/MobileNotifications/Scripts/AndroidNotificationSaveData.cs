using System;

namespace SinSity.Notifications {
    [Serializable]
    public class AndroidNotificationSaveData {
        
        public int idForStatus;
        public string idForAnalytics;

        public static AndroidNotificationSaveData GetDefault() {
            var data = new AndroidNotificationSaveData();
            data.idForStatus = 0;
            data.idForAnalytics = "";
            return data;
        }
    }
}