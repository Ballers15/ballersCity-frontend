using Orego.Util;
using SinSity.Analytics;
using SinSity.Meta;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPopupFortuneWheel : UIPopupAnim<UIPopupFortuneWheel.Properties, UIPopupArgs> {

        private FortuneWheelInteractor interactor;
        private FortuneWheel fortuneWheel;

        #region LIFECYCLE

        protected override void Awake() {
            base.Awake();

            this.interactor = this.GetInteractor<FortuneWheelInteractor>();

            if (interactor.isInitialized)
                this.fortuneWheel = interactor.fortuneWheel;
            else
                interactor.OnInitializedEvent += this.OnFWInteractorInitialized;
        }
        
        private void OnEnable() {
            this.properties.buttonClose.AddListener(this.OnCloseButtonClick);
        }

        private void OnDisable() {
            this.properties.buttonClose.RemoveListener(this.OnCloseButtonClick);
        }

        protected override void OnPostShow() {
            base.OnPostShow();
            this.LogPopupShown();
        }

        #endregion

       
        private void LogPopupShown() {
            var paymentType = PaymentType.HardCurrency;
            var price = this.interactor.gemsPrice;
            if (this.interactor.CanUseFreeSpin()) {
                paymentType = PaymentType.Free;
            }
            else if (this.interactor.CanUseAdSpin()) {
                paymentType = PaymentType.ADS;
            }
            FortuneWheelAnalytics.LogPopupFortuneWheelShown(paymentType, price);
        }

        #region EVENTS
        
        private void OnFWInteractorInitialized(IInteractor iiteractor) {
            iiteractor.OnInitializedEvent -= this.OnFWInteractorInitialized;
            this.fortuneWheel = this.interactor.fortuneWheel;
        }

        private void OnCloseButtonClick() {
            SFX.PlayClosePopup();

            if (!this.fortuneWheel.isRotating)
                this.Hide();
        }

        #endregion

        [System.Serializable]
        public class Properties : UIProperties {
            public Button buttonClose;
        }
    }
}