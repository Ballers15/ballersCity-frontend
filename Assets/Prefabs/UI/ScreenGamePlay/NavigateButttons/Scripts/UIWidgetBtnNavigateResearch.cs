using System;
using System.Linq;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Tools;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;

namespace SinSity.UI
{
    public sealed class UIWidgetBtnNavigateResearch : UIWidgetBtnNavigate
    {
        [SerializeField] private GameObject exclamationMarker;
        [SerializeField] private Button button;
        [Space] 
        [SerializeField] private Image imgBackground;
        [SerializeField] private Image imgIcon;
        [SerializeField] private Color colorLocked = Color.gray;
        [SerializeField] private UIWidgetBtnNavigateLocker widgetLocker;

        #region Event

        public static event Action OnScreenOpenedEvent;

        public static event Action OnScreenClosedEvent;

        #endregion

        private ResearchObjectTimerInteractor timerInteractor;

        private ResearchObjectRewardInteractor rewardInteractor;

        private ResearchObjectDataInteractor dataInteractor;

        private ResearchStateInteractor researchStateInteractor;

        private ProfileLevelInteractor profileInteractor;


        #region Init

        protected override void Awake()
        {
            base.Awake();
            Game.OnGameInitialized += this.OnGameInitialized;
            Game.OnGameReady += this.OnGameReady;
            Game.OnGameStart += this.OnGameStart;
        }

        private void OnGameInitialized(Game game)
        {
            Game.OnGameInitialized -= this.OnGameInitialized;
            this.researchStateInteractor = Game.GetInteractor<ResearchStateInteractor>();
            this.dataInteractor = Game.GetInteractor<ResearchObjectDataInteractor>();
            this.timerInteractor = Game.GetInteractor<ResearchObjectTimerInteractor>();
            this.rewardInteractor = Game.GetInteractor<ResearchObjectRewardInteractor>();
            this.profileInteractor = Game.GetInteractor<ProfileLevelInteractor>();
        }

        private void OnGameReady(Game obj)
        {
            Game.OnGameReady -= this.OnGameReady;
            this.timerInteractor.OnResearchObjectFinishedEvent
                += this.Finished;
            this.rewardInteractor.OnResearchObjectRewardReceivedEvent
                += this.OnResearchObjectRewardReceived;
            this.timerInteractor.OnResearchObjectLaunchedEvent += this.OnResearchObjectLaunched;
        }

        private void OnGameStart(Game obj)
        {
            Game.OnGameStart -= this.OnGameStart;
            this.UpdateExclamationMarkerState();
            if (this.researchStateInteractor.isResearchUnlocked)
            {
                //this.SetVisualAsUnlockedDefault();
                this.button.enabled = true;
            }
            else
            {
                this.researchStateInteractor.OnResearchUnlockedEvent += this.OnResearchUnlocked;
                //this.SetVisualAsLocked();
                this.button.enabled = false;
            }
        }

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
//            InputManager.OnBackKeyClicked += this.OnBackClicked;
            Bank.uiBank.OnStateChangedEvent += this.OnBankStateChanged;
        }

        public override bool IsPopupAlreadyOpened()
        {
            var screenResearch = this.uiInteractor.GetUIElement<UIScreenResearch>();
            return screenResearch && screenResearch.isActive;
        }

        protected override void OpenPopup()
        {
            ResearchAnalytics.LogResearchWindowOpened();
            this.uiInteractor.ShowElement<UIScreenResearch>();
            this.SetVisualAsOpened();
            OnScreenOpenedEvent?.Invoke();
        }

        public override void ClosePopup()
        {
            var screenResearch = this.uiInteractor.GetUIElement<UIScreenResearch>();
            if (screenResearch)
            {
                screenResearch.Hide();
                OnScreenClosedEvent?.Invoke();
                this.SetVisualAsHidden();
            }
        }

        private void UpdateExclamationMarkerState()
        {
            if (!this.researchStateInteractor.isResearchUnlocked)
            {
                this.exclamationMarker.SetActive(false);
                return;
            }

            var hasAvailableResearchObjects = this.dataInteractor
                .GetResearchObjects()
                .Any(it =>
                {
                    var state = it.state;
                    return state.isRewardReady ||
                           it.info is ResearchObjectInfoSimple &&
                           !state.isEnabled &&
                           Bank.softCurrencyCount > state.price;
                });
            this.exclamationMarker.SetActive(hasAvailableResearchObjects);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
//            InputManager.OnBackKeyClicked -= this.OnBackClicked;
            Bank.uiBank.OnStateChangedEvent -= this.OnBankStateChanged;
        }

      

        private void OnDestroy()
        {
            this.timerInteractor.OnResearchObjectFinishedEvent
                -= this.Finished;
            this.rewardInteractor.OnResearchObjectRewardReceivedEvent
                -= this.OnResearchObjectRewardReceived;
        }

        #region Events

        private void Finished(ResearchObject obj)
        {
            this.exclamationMarker.SetActive(true);
        }

        private void OnResearchObjectRewardReceived(object sender, ResearchObject researchObject)
        {
            this.UpdateExclamationMarkerState();
        }

        private void OnResearchObjectLaunched(ResearchObject researchObject)
        {
            this.UpdateExclamationMarkerState();
        }

        private void OnResearchUnlocked(object obj)
        {
            this.researchStateInteractor.OnResearchUnlockedEvent -= this.OnResearchUnlocked;
            //this.SetVisualAsUnlockedDefault();
            this.button.enabled = true;
        }

        
        private void OnBankStateChanged(object sender) {
            UpdateExclamationMarkerState();
        }
        
        #endregion

//        private void OnBackClicked()
//        {
//            if (this.IsPopupAlreadyOpened())
//            {
//                this.ClosePopup();
//            }
//        }
        
        private void SetVisualAsLocked() {
            this.imgBackground.color = this.colorLocked;
            this.imgIcon.gameObject.SetActive(false);

            int requiredLevel = profileInteractor.levelHandlerMap.First(it => it.Value is ResearchLevelHandler).Value
                .reachLevel;
            this.widgetLocker.Activate(requiredLevel);
        }

        private void SetVisualAsUnlockedDefault() {
            this.imgBackground.color = this.colorDefault;
            this.imgIcon.gameObject.SetActive(true);
            this.widgetLocker.Deactivate();
        }
    }
}