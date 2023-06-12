using System;
using Prefabs.UI.PopupRateUs.Analytics;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Meta;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using VavilichevGD.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace SinSity.UI {
    public class UIControllerPopupRateUs : MonoBehaviour {

        #region CONSTANTS

        private const string KEY_NEVER_REMIND = "NEVER_REMIND_RATE_US";
        private const string KEY_RATE_US_POPUP_DATA = "RATE_US_POPUP_DATA";
        private const int PROFILE_LEVEL_MIN = 5;
        private const int IDLE_OBJECT_NUMBER_MIN = 4;
        private const string URL_GOOGLE_PLAY = "market://details?id=com.MadDiamond.EcoClicker";
        private const string URL_APP_STORE = "itms-apps:apps.apple.com/us/app/id1511960509";

        #endregion

        [Tooltip("In seconds")]
        [SerializeField] private float firstShowPeriod = 300;
        [SerializeField] private int periodBetweenShowing = 3600;
        [SerializeField] private int showTimes = 3;

        private ProfileLevelInteractor profileLevelInteractor;
        private GemTreeStateInteractor gemTreeStateInteractor;
        private IdleObjectsInteractor idleObjectsInteractor;
        private UIScreenGemTree screenGemTree;
        private UIPopupBuild popupBuild;
        private UIPopupNewLevel popupNewLevel;
        private UIController uiController;
        private DateTime timeFromSessionStart;
        private UIPopupRateUsData data; 

        private bool gemTreeLevelRisen;


        #region LIFECYCLE

        private void Awake() {
            this.profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
            this.gemTreeStateInteractor = Game.GetInteractor<GemTreeStateInteractor>();
            this.idleObjectsInteractor = Game.GetInteractor<IdleObjectsInteractor>();

            this.timeFromSessionStart = DateTime.Now;
        }

        private void Start() {
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            this.uiController = uiInteractor.uiController;
            this.uiController.OnAllCachedElementsCreatedEvent += this.OnUiControllerAllCachedElementsCreated;
            this.data = Storage.GetCustom(KEY_RATE_US_POPUP_DATA, this.data) ?? new UIPopupRateUsData();
        }

       
        private void OnDestroy() {
            this.popupNewLevel.OnUIElementClosedCompletelyEvent -= OnPopupNewLevelClosedCompletelyEvent;
            this.screenGemTree.OnUIElementClosedCompletelyEvent -= OnScreenGemTreeClosedCompletely;
            this.popupBuild.OnUIElementClosedCompletelyEvent -= OnPopupBuildObjectClosedCompletelyEvent;

            this.gemTreeStateInteractor.OnCurrentLevelChangedEvent -= OnGemtreeLevelChanged;
        }

        #endregion
        

        
        private void ShowPopup(string placementId) {
            
            if (!this.TimeHasCome())
                return;

            var neverRemind = Storage.GetBool(KEY_NEVER_REMIND);
            if (neverRemind)
                return;
            
            this.data.showedTimes++;
            
            var popup = this.uiController.Show<UIPopupRateUs>();
            popup.OnDialogueResults += this.OnRateUsDialogueResults;
            AnalyticsPopupRateUs.LogPopupShown(placementId);
        }

        private bool TimeHasCome() {
            var now = DateTime.Now;
            var nextTimeShowing = this.data.timeNextShowing.GetDateTime();
            var diffBetweenSessions = nextTimeShowing - now;

            if (diffBetweenSessions.TotalSeconds > 0) return false;
            
            var diffFromSessionStart = now - this.timeFromSessionStart;
            var timeHasCome = diffFromSessionStart.TotalSeconds >= this.firstShowPeriod;
            
            if (timeHasCome)
                this.timeFromSessionStart = now;
            
            return timeHasCome;
        }

        
        
        #region EVENTS
        
        private void OnUiControllerAllCachedElementsCreated(UIController uicontroller) {
            this.uiController.OnAllCachedElementsCreatedEvent -= this.OnUiControllerAllCachedElementsCreated;
            
            this.popupNewLevel = this.uiController.GetUIElement<UIPopupNewLevel>();
            this.popupNewLevel.OnUIElementClosedCompletelyEvent += OnPopupNewLevelClosedCompletelyEvent;

            this.screenGemTree = this.uiController.GetUIElement<UIScreenGemTree>();
            this.screenGemTree.OnUIElementClosedCompletelyEvent += OnScreenGemTreeClosedCompletely;

            this.popupBuild = this.uiController.GetUIElement<UIPopupBuild>();
            this.popupBuild.OnUIElementClosedCompletelyEvent += OnPopupBuildObjectClosedCompletelyEvent;
            
            this.gemTreeStateInteractor.OnCurrentLevelChangedEvent += OnGemtreeLevelChanged;
        }

        private void OnPopupBuildObjectClosedCompletelyEvent(UIElement uielement) {
            if (this.popupBuild.lastBuildedObject != null && this.popupBuild.lastBuildedObject.info.number >= IDLE_OBJECT_NUMBER_MIN)
                this.ShowPopup($"object_builded_number_above_{IDLE_OBJECT_NUMBER_MIN}");
        }

        private void OnPopupNewLevelClosedCompletelyEvent(UIElement uielement) {
            var profileLevel = this.profileLevelInteractor.currentLevel;
            if (profileLevel >= PROFILE_LEVEL_MIN)
                this.ShowPopup($"new_player_level_above_{PROFILE_LEVEL_MIN}");
        }
        
        private void OnGemtreeLevelChanged(object sender, int nextLevel) {
            this.gemTreeLevelRisen = true;
        }
        
        private void OnScreenGemTreeClosedCompletely(UIElement uielement) {
            if (this.gemTreeLevelRisen) {
                this.gemTreeLevelRisen = false;
                this.ShowPopup("gem_tree_level_risen");
            }
        }

        private void OnRateUsDialogueResults(UIPopupArgs e) {
            var popup = (UIPopupRateUs) e.uiElement;
            popup.OnDialogueResults -= this.OnRateUsDialogueResults;
            
            if (e.result == UIPopupResult.Close) {
                if (popup.neverShow)
                    Storage.SetBool(KEY_NEVER_REMIND, true);
                if (this.data.showedTimes >= this.showTimes)
                {
                    Storage.SetBool(KEY_NEVER_REMIND, true);
                }
                else
                {
                    this.data.timeNextShowing = new DateTimeSerialized(this.timeFromSessionStart.AddSeconds(this.periodBetweenShowing));
                    Storage.SetCustom(KEY_RATE_US_POPUP_DATA, this.data);
                }
                
                AnalyticsPopupRateUs.LogPopupClosed(UIPopupResult.Close, popup.neverShow);
            }
            else if (e.result == UIPopupResult.Apply) {
                string url = "";
#if UNITY_ANDROID
                url = URL_GOOGLE_PLAY;                
#elif UNITY_IOS
                url = URL_APP_STORE;
#endif
                
                Application.OpenURL (url);
                Storage.SetBool(KEY_NEVER_REMIND, true);
                AnalyticsPopupRateUs.LogPopupClosed(UIPopupResult.Apply, popup.neverShow);
            }
        }

        #endregion
    }
}