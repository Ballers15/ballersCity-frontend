using System;
using System.Net.Http.Headers;
using SinSity.Core;
using SinSity.Monetization;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;

namespace SinSity.UI
{
    public class UIWidgetBtnNavigateShop : UIWidgetBtnNavigate {

        [SerializeField] private GameObject exclamationMarker;
        
        #region Event

        public static event Action OnPopupOpenedEvent;

        public static event Action OnPopupClosedEvent;
        
        #endregion

        private UIPopupShop popupShop;

        protected override void OnEnable()
        {
            base.OnEnable();
            UIPopupShop.OnUIPopupShopStateChanged += OnUiPopupShopStateChanged;
            ProductStateCase.OnCasesCountChanged += OnCasesCountChanged;
            ProductStateTimeBooster.OnTimeBoostersCountChanged += OnTimeBoostersCountChanged;
//            InputManager.OnBackKeyClicked += this.OnBackClicked;
        }

        private void OnUiPopupShopStateChanged(UIPopupShop popupshop, bool isActive)
        {
            if (isActive)
                SetVisualAsOpened();
            else
                SetVisualAsHidden();
        }
        
        private void OnCasesCountChanged(int casescount) {
            UpdateExclamationMarlerState();
        }

        private void OnTimeBoostersCountChanged(int timeboosterscount) {
            UpdateExclamationMarlerState();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UIPopupShop.OnUIPopupShopStateChanged -= OnUiPopupShopStateChanged;
            ProductStateCase.OnCasesCountChanged -= OnCasesCountChanged;
            ProductStateTimeBooster.OnTimeBoostersCountChanged -= OnTimeBoostersCountChanged;
//            InputManager.OnBackKeyClicked -= this.OnBackClicked;
        }

        public override bool IsPopupAlreadyOpened()
        {
            popupShop = uiInteractor.GetUIElement<UIPopupShop>();
            return popupShop && popupShop.isActive;
        }

        protected override void OpenPopup()
        {
            popupShop = uiInteractor.ShowElement<UIPopupShop>();
            OnPopupOpenedEvent?.Invoke();
            CommonAnalytics.LogShopTabOpened("btn_navigate_shop", Bank.hardCurrencyCount);
        }

        public override void ClosePopup()
        {
            if (popupShop)
            {
                popupShop.Hide();
                OnPopupClosedEvent?.Invoke();
            }
        }

        private void UpdateExclamationMarlerState() {
            bool hasAnyCases = Shop.HasAnyCases();
            bool hasAnyBoosters = Shop. HasAnyBoosters();
            
            exclamationMarker.SetActive(hasAnyBoosters || hasAnyCases);
        }

        

        private void Start() {
            Game.OnGameInitialized += OnGameInitialized;
        }

        private void OnGameInitialized(Game game) {
            Game.OnGameInitialized -= OnGameInitialized;
            UpdateExclamationMarlerState();
        }
        
//        private void OnBackClicked()
//        {
//            if (this.IsPopupAlreadyOpened())
//            {
//                this.ClosePopup();
//            }
//        }
    }
}