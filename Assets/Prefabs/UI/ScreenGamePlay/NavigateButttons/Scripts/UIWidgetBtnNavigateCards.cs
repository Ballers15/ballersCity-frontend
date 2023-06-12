using System;
using System.Linq;
using SinSity.Domain;
using SinSity.Meta.Quests;
using SinSity.Services;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetBtnNavigateCards  : UIWidgetBtnNavigate {
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

        #region Init

        protected override void Awake() {
            base.Awake();
            Game.OnGameInitialized += this.OnGameInitialized;
            Game.OnGameReady += this.OnGameReady;
            Game.OnGameStart += this.OnGameStart;
        }

        private void OnGameInitialized(Game game) {
            Game.OnGameInitialized -= this.OnGameInitialized;
        }

        private void OnGameReady(Game obj) {
            Game.OnGameReady -= this.OnGameReady;
        }

        private void OnGameStart(Game obj) {
            Game.OnGameStart -= this.OnGameStart;
            this.UpdateExclamationMarkerState();
            this.button.enabled = true;
        }

        #endregion

        protected override void OnEnable() {
            base.OnEnable();
        }

        public override bool IsPopupAlreadyOpened() {
            var popup = this.uiInteractor.GetUIElement<UIPopupCardsCollection>();
            return popup && popup.isActive;
        }

        protected override void OpenPopup() {
            var popup = uiInteractor.ShowElement<UIPopupCardsCollection>();
            SetVisualAsOpened();
            popup.OnUIElementClosedCompletelyEvent += OnPopupClosed;
            OnScreenOpenedEvent?.Invoke();
        }

        private void OnPopupClosed(UIElement uielement) {
            uielement.OnUIElementClosedCompletelyEvent -= OnPopupClosed;
            SetVisualAsHidden();
        }

        public override void ClosePopup() {
            var popup = this.uiInteractor.GetUIElement<UIPopupCardsCollection>();
            if (popup.isActive) {
                popup.Hide();
                OnScreenClosedEvent?.Invoke();
                SetVisualAsHidden();
            }
        }

        private void UpdateExclamationMarkerState() {
            exclamationMarker.SetActive(false);
        }

        protected override void OnDisable() {
            base.OnDisable();
        }
        
        private void OnDestroy() {

        }
    }
}