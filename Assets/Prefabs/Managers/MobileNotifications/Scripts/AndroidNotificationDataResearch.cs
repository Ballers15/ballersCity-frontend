using System;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.LocalizationFramework;

namespace SinSity.Notifications {
    public class AndroidNotificationDataResearch : AndroidNotificationData {
        
        #region Constants

        private const string ID_NOTIFICATION = "push_research";
        private const string ID_PUSH_TITLE = "ID_PUSH_TITLE_RESEARCH";
        private const string ID_PUSH_TEXT = "ID_PUSH_RESEARCH";
        private const string ID_ICON_SMALL = "icon_small";
        private const string ID_ICON_LARGE = "icon_large_research";
        
        public const string PREFKEY = "PUSH_RESEARCH";

        #endregion

        private ResearchObjectDataInteractor researchObjectDataInteractor;


        #region Initialize

        public AndroidNotificationDataResearch(ResearchObjectDataInteractor researchObjectDataInteractor) {
            this.researchObjectDataInteractor = researchObjectDataInteractor;
            this.allowToCreate = researchObjectDataInteractor.HasActiveComplexResearch();
            
            if (this.allowToCreate)
                this.Initialize();
        }

        private void Initialize() {
            this.fireTime = GetResearchNotificationTime(this.researchObjectDataInteractor);
            this.title = Localization.GetTranslation(ID_PUSH_TITLE);
            this.text = Localization.GetTranslation(ID_PUSH_TEXT);
            this.smallIconId = ID_ICON_SMALL;
            this.largeIconId = ID_ICON_LARGE;
            this.id = ID_NOTIFICATION;
        }
        
        private DateTime GetResearchNotificationTime(ResearchObjectDataInteractor dataInteractor) {
            int maxSecondsRemaining = dataInteractor.GetComplexResearchTimeLeftSec();
            int clampedMinutes = Mathf.CeilToInt(maxSecondsRemaining / 60);

            var notificationTime = DateTime.Now.AddMinutes(clampedMinutes);
            var clampedNotificationTime = this.ClampNotificationTime(notificationTime);
            return clampedNotificationTime;
        }

        #endregion
        
        public override string GetPrefKey() {
            return PREFKEY;
        }
    }
}