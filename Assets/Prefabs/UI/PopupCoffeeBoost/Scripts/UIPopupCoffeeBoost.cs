using System;
using System.Security.Cryptography;
using DevToDev;
using SinSity.Services;
using IdleClicker.Gameplay;
using Orego.Util;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace IdleClicker.UI {
    public class UIPopupCoffeeBoost : UIPopupAnim<UIPopupCoffeeBoost.Properties, UIPopupArgs> {

        #region CONSTANTS

        private const string TEXT_DESCRIPTION_ID = "ID_COFFEE_POP_DESC";
        private const string TEXT_MULTIPLIER = "x{0}";

        #endregion

        private PaymentType paymentType;

        protected override void OnPostShow() {
            base.OnPostShow();
            SFX.PlayOpenPopup();
        }

        private void OnEnable() {
            this.properties.buttonClose.AddListener(this.OnCloseButtonClick);
        }

        private void OnDisable() {
            this.properties.buttonGoGems.RemoveListener(this.OnGoButtonClick);
            this.properties.buttonGoADS.RemoveListener(this.OnGoButtonClick);
            this.properties.buttonClose.RemoveListener(this.OnCloseButtonClick);
        }



        #region SETUP

        public void SetupOfferADS(CoffeeBoostConfig coffeeBoostConfig) {
            this.paymentType = PaymentType.ADS;
            
            this.SetupInfo(coffeeBoostConfig);
            SetupIcon(coffeeBoostConfig);

            this.properties.buttonGoGems.gameObject.SetActive(false);
            this.properties.buttonGoADS.gameObject.SetActive(true);
            
            this.properties.buttonGoADS.AddListener(this.OnGoButtonClick);
        }

        public void SetupOfferGems(CoffeeBoostConfig coffeeBoostConfig, int priceGems) {
            this.paymentType = PaymentType.HardCurrency;
            
            this.SetupInfo(coffeeBoostConfig);
            SetupIcon(coffeeBoostConfig);
            
            this.properties.buttonGoGems.gameObject.SetActive(true);
            this.properties.buttonGoADS.gameObject.SetActive(false);
            
            this.properties.buttonGoGems.AddListener(this.OnGoButtonClick);
            this.properties.textPriceGemsValue.text = priceGems.ToString();
        }

        private void SetupInfo(CoffeeBoostConfig coffeeBoostConfig) {
            var translatingText = Localization.GetTranslation(TEXT_DESCRIPTION_ID);
            var descriptionLine = string.Format(translatingText, coffeeBoostConfig.duration);
            this.properties.textDescription.text = descriptionLine;

            var multiplierLine = string.Format(TEXT_MULTIPLIER, coffeeBoostConfig.boostTimeScale);
            this.properties.textMultiplier.text = multiplierLine;
        }

        private void SetupIcon(CoffeeBoostConfig config) {
            CleanIconContainer();

            var prefab = config.cupPrefabForPopup;
            Instantiate(prefab, properties.cupContainer);
        }

        private void CleanIconContainer() {
            var childCount = properties.cupContainer.childCount;
            for (int i = 0; i < childCount; i++) 
                Destroy(properties.cupContainer.GetChild(i).gameObject);
        }
        
        #endregion

       

        #region EVENTS

        private void OnGoButtonClick() {
            SFX.PlayBtnClick();
            
            this.Hide();
            
            var args = new UIPopupArgs(this, UIPopupResult.Apply);
            this.NotifyAboutResults(args);

            var interactor = GetInteractor<CoffeeBoostInteractor>();
            var coffeeBoost = interactor.coffeeBoost;
            CoffeeBoostAnalytics.LogCoffeeBoostPopupResults(coffeeBoost.level, paymentType, "applied");
        }

        private void OnCloseButtonClick() {
            SFX.PlayClosePopup();
            
            this.Hide();
            var args = new UIPopupArgs(this, UIPopupResult.Close);
            this.NotifyAboutResults(args);
            
            var interactor = GetInteractor<CoffeeBoostInteractor>();
            var coffeeBoost = interactor.coffeeBoost;
            CoffeeBoostAnalytics.LogCoffeeBoostPopupResults(coffeeBoost.level, paymentType, "closed");
        }

        #endregion
        
        


        [Serializable]
        public class Properties : UIProperties {
            public Button buttonGoADS;
            public Button buttonGoGems;
            public Text textPriceGemsValue;
            public Button buttonClose;

            [Space] 
            public Text textDescription;
            public Text textMultiplier;

            [Space] 
            public Transform cupContainer;
        }
    }
}