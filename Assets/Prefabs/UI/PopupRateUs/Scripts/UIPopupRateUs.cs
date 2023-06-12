using System;
using Orego.Util;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPopupRateUs : UIPopupAnim<UIPopupRateUs.Properties, UIPopupArgs> {

        [HideInInspector] public bool neverShow = false;

        protected override void OnPostShow() {
            base.OnPostShow();
            SFX.PlayOpenPopup();
        }

        #region MONO

        private void OnEnable() {
            this.properties.buttonRate.button.AddListener(this.OnRateButtonClick);
            this.properties.buttonClose.AddListener(this.OnCloseButtonClick);
            this.properties.widgetStars.OnValueChangedEvent += this.OnStarToggled;
        }
        
        private void OnDisable() {
            this.properties.buttonRate.button.RemoveListener(this.OnRateButtonClick);
            this.properties.buttonClose.RemoveListener(this.OnCloseButtonClick);
            this.properties.widgetStars.OnValueChangedEvent -= this.OnStarToggled;
            this.ResetVisual();
        }

        #endregion
       

        private void ResetVisual() {
            this.properties.widgetStars.ResetStars();
            this.properties.buttonRate.ResetVisual();
        }


        #region CALLBACKS

        private void OnRateButtonClick() {
            if (!this.properties.widgetStars.HasToggledStar())
            {
                this.properties.textSelectStars.gameObject.Show();
                return;
            }

            var starsToggled = this.properties.widgetStars.GetToggledStarsCount();
            var args = new UIPopupArgs(this, UIPopupResult.Close);
            
            if (starsToggled >= this.properties.starsToRate)
            {
                args.result = UIPopupResult.Apply;
            }
            else
            {
                this.neverShow = true;
            }
            
            SFX.PlayBtnClick();
            this.Hide();

            this.NotifyAboutResults(args);
        }

        private void OnCloseButtonClick() {
            SFX.PlayClosePopup();
            this.Hide();

            var args = new UIPopupArgs(this, UIPopupResult.Close);
            this.NotifyAboutResults(args);
        }
        
        private void OnToggleValueChanged(bool toggleValue) {
            SFX.PlayBtnClick();
        }
        
        private void OnStarToggled() {
            this.properties.widgetStars.OnValueChangedEvent -= this.OnStarToggled;
            this.properties.buttonRate.SetVisualActive();
        }

        #endregion


        [Serializable]
        public class Properties : UIProperties {
            public int starsToRate;
            public ButtonRateUs buttonRate;
            public Button buttonClose;
            public Text textSelectStars;
            public WidgetRateUsStars widgetStars;
        }
    }
}