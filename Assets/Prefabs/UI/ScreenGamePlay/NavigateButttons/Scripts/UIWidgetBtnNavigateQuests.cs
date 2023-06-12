using System;
using System.Linq;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Meta.Quests;
using SinSity.Services;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.UI
{
    public sealed class UIWidgetBtnNavigateQuests : UIWidgetBtnNavigate
    {
        [SerializeField] private GameObject exclamationMarker;
        [SerializeField] private Button button;
        [Space]
        [SerializeField] private Image imgBackground;
        [SerializeField] private Sprite spriteLocked;
        [SerializeField] private Sprite spriteUnlocked;

        #region Event

        public static event Action OnPopupOpenedEvent;

        public static event Action OnPopupClosedEvent;

        #endregion

        private UIScreenQuests screenQuests;
        private MainQuestsInteractor mainQuestsInteractor;
        private MiniQuestInteractor miniQuestInteractor;

        private bool isActiveTemp;

        protected override void OnEnable()
        {
            base.OnEnable();
            UIScreenQuests.OnUIScreenQuestsStateChanged += OnUiScreenQuestsStateChanged;
            UIWidgetMiniQuest.OnMiniQuestTakeRewardEvent += OnMiniQuestRewardTaken;

            if (Game.isInitialized)
            {
                SubscribeOnQuestsEvents();
                UpdateExclamationMarkerState();
            }
            else
            {
                Game.OnGameInitialized += OnGameInitialized;
            }
        }

        private void OnGameInitialized(Game game)
        {
            Game.OnGameInitialized -= OnGameInitialized;
            mainQuestsInteractor = Game.GetInteractor<MainQuestsInteractor>();
            miniQuestInteractor = Game.GetInteractor<MiniQuestInteractor>();

            SubscribeOnQuestsEvents();
            UpdateExclamationMarkerState();
        }

        private void SubscribeOnQuestsEvents()
        {
            this.mainQuestsInteractor.OnQuestChangedEvent += this.OnMainQuestChangedEvent;
            this.miniQuestInteractor.OnQuestChangedEvent += this.OnMiniQuestChangedEvent;
            if (this.mainQuestsInteractor.isQuestsUnlocked)
            {
                SetVisualAsUnlockedDefault();
                this.button.enabled = true;
            }
            else
            {
                this.mainQuestsInteractor.OnQuestsUnlockedEvent += this.OnQuestsUnlocked;
                SetVisualAsLocked();
                this.button.enabled = false;
            }
        }

        private void OnQuestsUnlocked(object obj)
        {
            this.mainQuestsInteractor.OnQuestsUnlockedEvent -= this.OnQuestsUnlocked;
            SetVisualAsUnlockedDefault();
            this.button.enabled = true;
        }

        private void OnMainQuestChangedEvent(Quest quest)
        {
            UpdateExclamationMarkerState();
        }

        private void OnMiniQuestChangedEvent(Quest quest)
        {
            UpdateExclamationMarkerState();
        }

        private void UpdateExclamationMarkerState()
        {
            if (!this.mainQuestsInteractor.isQuestsUnlocked)
            {
                this.exclamationMarker.SetActive(false);
                return;
            }
            
            var anyQuestComplete = this.mainQuestsInteractor.HasCompletedQuest() || 
                                   this.miniQuestInteractor.HasAnyCompletedQuest();
            this.exclamationMarker.SetActive(anyQuestComplete);
        }

        private void OnUiScreenQuestsStateChanged(UIScreenQuests screenquests, bool isActive)
        {
            if (isActive)
                SetVisualAsOpened();
            else
                SetVisualAsHidden();
        }

        private void OnMiniQuestRewardTaken()
        {
            this.UpdateExclamationMarkerState();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UIScreenQuests.OnUIScreenQuestsStateChanged -= OnUiScreenQuestsStateChanged;
            mainQuestsInteractor.OnQuestChangedEvent -= OnMainQuestChangedEvent;
            miniQuestInteractor.OnQuestChangedEvent -= OnMiniQuestChangedEvent;
        }

        public override bool IsPopupAlreadyOpened()
        {
            screenQuests = uiInteractor.GetUIElement<UIScreenQuests>();
            return screenQuests && screenQuests.isActive;
        }

        protected override void OpenPopup()
        {
            screenQuests = uiInteractor.ShowElement<UIScreenQuests>();
            var mainQuestsInteractor = Game.GetInteractor<MainQuestsInteractor>();
            var miniQuestsInteractor = Game.GetInteractor<MiniQuestInteractor>();
            OnPopupOpenedEvent?.Invoke();
            CommonAnalytics.LogQuestsTabOpened(
                mainQuestsInteractor.HasCompletedQuest() ||
                miniQuestsInteractor.GetActiveQuests().Any(it => it.isCompleted)
            );
        }

        public override void ClosePopup()
        {
            if (screenQuests)
            {
                screenQuests.Hide();
                OnPopupClosedEvent?.Invoke();
            }
        }
        
        private void SetVisualAsLocked() {
            imgBackground.sprite = spriteLocked;
        }

        private void SetVisualAsUnlockedDefault() {
            imgBackground.sprite = spriteUnlocked;
        }
    }
}