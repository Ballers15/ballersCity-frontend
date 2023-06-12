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
    public sealed class UIWidgetBtnNavigateGemTree : UIWidgetBtnNavigate {
        #region Event

        public static event Action OnPopupOpenedEvent;

        public static event Action OnPopupClosedEvent;

        #endregion
        
        [SerializeField] private Image imgBackground;
        [SerializeField] private Color colorLocked = Color.gray;
        [SerializeField] private Image imgIcon;
        [SerializeField] private UIWidgetBtnNavigateLocker widgetLocker;
        [SerializeField] private GameObject goExclamationMarker;

        private GemTreeTimerInteractor gemTreeTimerInteractor;
        private GemTreeBranchDataInteractor gemTreeBranchDataInteractor;
        private GemTreeStateInteractor gemTreeStateInteractor;
        
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
            this.gemTreeTimerInteractor = Game.GetInteractor<GemTreeTimerInteractor>();
            this.gemTreeBranchDataInteractor = Game.GetInteractor<GemTreeBranchDataInteractor>();
            this.gemTreeStateInteractor = Game.GetInteractor<GemTreeStateInteractor>();
        }

        private void OnGameReady(Game obj)
        {
            Game.OnGameReady -= this.OnGameReady;
            
            if (!gemTreeStateInteractor.isTreeUnlocked) {
                this.gemTreeStateInteractor.OnTreeUnlockedEvent += OnTreeUnlocked;
                this.SetVisualAsLocked();
            }
            else {
                this.gemTreeTimerInteractor.OnBranchObjectReadyEvent += OnBranchObjectReady;
                this.gemTreeTimerInteractor.OnBranchObjectRewardReceivedEvent += OnBranchObjectRewardReceived;
                this.gemTreeStateInteractor.OnCurrentProgressChangedEvent += OnGemTreeProgressChanged;
                this.SetVisualAsUnlockedDefault();
            }          
        }

       

        private void OnGameStart(Game obj)
        {
            Game.OnGameStart -= this.OnGameStart;
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
            var screenGemTree = this.uiInteractor.GetUIElement<UIScreenGemTree>();
            return screenGemTree && screenGemTree.isActive;
        }

        protected override void OpenPopup()
        {
            if (!gemTreeStateInteractor.isTreeUnlocked)
            {
                return;
            }

            this.uiInteractor.ShowElement<UIScreenGemTree>();
            this.SetVisualAsOpened();
            GemTreeAnalytics.LogTreeWindowOpened();
            OnPopupOpenedEvent?.Invoke();
        }

        public override void ClosePopup()
        {
            var screenGemTree = this.uiInteractor.GetUIElement<UIScreenGemTree>();
            if (!screenGemTree || (screenGemTree && !screenGemTree.isActive))
            {
                return;
            }

            screenGemTree.Hide();
            this.SetVisualAsHidden();
            GemTreeAnalytics.LogTreeWindowClosed();
            OnPopupClosedEvent?.Invoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
//            InputManager.OnBackKeyClicked -= this.OnBackClicked;
            Bank.uiBank.OnStateChangedEvent -= this.OnBankStateChanged;
        }

//        private void OnBackClicked()
//        {
//            if (this.IsPopupAlreadyOpened())
//            {
//                this.ClosePopup();
//            }
//        }

        
        
        #region Visual

        private void SetVisualAsLocked() {
            this.goExclamationMarker.SetActive(false);
            this.btn.interactable = false;
            this.imgBackground.color = this.colorLocked;
            this.imgIcon.gameObject.SetActive(false);

            var profileInteractor = Game.GetInteractor<ProfileLevelInteractor>();
            int requiredLevel = profileInteractor.levelHandlerMap.First(it => it.Value is GemTreeLevelHandler).Value
                .reachLevel;
            this.widgetLocker.Activate(requiredLevel);
        }

        private void SetVisualAsUnlockedDefault() {
            this.btn.interactable = true;
            this.imgBackground.color = this.colorDefault;
            this.imgIcon.gameObject.SetActive(true);
            this.widgetLocker.Deactivate();
            
            this.UpdateExclamationState();
        }

        private void UpdateExclamationState() {
            bool hasAnyFruit = this.gemTreeBranchDataInteractor.HasTreeAnyReadyFruit();
            bool canUpgrade = this.gemTreeStateInteractor.CanUpgradeTree();
            bool isUnlocked = this.gemTreeStateInteractor.isTreeUnlocked;
            goExclamationMarker.SetActive(isUnlocked && (hasAnyFruit || canUpgrade));
        }

        #endregion

        
        #region Events
        
        private void OnGemTreeProgressChanged(object arg1, int arg2) {
            this.UpdateExclamationState();
        }

        private void OnBankStateChanged(object sender) {
            this.UpdateExclamationState();
        }

        private void OnTreeUnlocked(object obj) {
            this.gemTreeTimerInteractor.OnBranchObjectReadyEvent += OnBranchObjectReady;
            this.gemTreeTimerInteractor.OnBranchObjectRewardReceivedEvent += OnBranchObjectRewardReceived;
            this.SetVisualAsUnlockedDefault();
        }

        private void OnBranchObjectRewardReceived(object sender, GemBranchObject gemBranchObject, GemBranchReward gemBranchReward) {
            this.UpdateExclamationState();
        }

        private void OnBranchObjectReady(object sender, GemBranchObject gemBranchObject) {
            this.UpdateExclamationState();
        }

        #endregion

        
        
        private void OnDestroy() {
            this.gemTreeTimerInteractor.OnBranchObjectReadyEvent -= OnBranchObjectReady;
            this.gemTreeTimerInteractor.OnBranchObjectRewardReceivedEvent -= OnBranchObjectRewardReceived;
            this.gemTreeStateInteractor.OnTreeUnlockedEvent -= OnTreeUnlocked;
            this.gemTreeStateInteractor.OnCurrentProgressChangedEvent -= OnGemTreeProgressChanged;
        }
    }
}