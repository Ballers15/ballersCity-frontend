using System;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIWidgetPersCardContentReadyForPurchase
        : UIWidget<UIWidgetPersCardContentReadyForPurchaseProps> {
//TODO: OLD CARDS SYSTEM DELETED
        /*private int nextLevelNumber;
        private CardObject cardObject;

        public void SetActive(bool isActive) {
            this.gameObject.SetActive(isActive);
        }

        public void Setup(CardObject cardObject) {
            this.cardObject = cardObject;

            var hasNextLevel = cardObject.HasNextLevel();
            var textNextIncomeSpeedMultiplicator = this.properties.textNextIncomeSpeedMultiplicator;
            var textDescription = this.properties.textDescription;
            textNextIncomeSpeedMultiplicator.gameObject.SetActive(hasNextLevel);
            textDescription.gameObject.SetActive(hasNextLevel);
            
            this.SetupPrice();
            
            if (!hasNextLevel)
                return;
            

            var nextLevelIndex = cardObject.currentLevelIndex + 1;
            var nextMultiplier = Math.Pow(CardObjectInfo.SPEED_BOOST_MULTIPLIER, nextLevelIndex);
            textNextIncomeSpeedMultiplicator.text = $"x{nextMultiplier}";
            nextLevelNumber = nextLevelIndex + 1;

            UpdateLocalization();
        }

        private void SetupPrice() {
            if (this.cardObject == null)
                return;
            
            var panelPrice = this.properties.widgetPrice;
            var hasNextLevel = cardObject.HasNextLevel();
            panelPrice.gameObject.SetActive(hasNextLevel);
            
            if (!hasNextLevel)
                return;
            
            var nextLevel = cardObject.GetNextLevel();
            var priceGems = nextLevel.m_gemsPrice;
            this.properties.widgetPrice.SetPrice(priceGems.ToString());
            
            this.SetupPriceVisual();
        }

        private void SetupPriceVisual() {
            if (!this.properties.widgetPrice.isActive)
                return;

            var nextLevel = cardObject.GetNextLevel();
            var priceGems = nextLevel.m_gemsPrice;
            var isEnoughMoney = Bank.isEnoughtHardCurrency(priceGems);

            if (isEnoughMoney)
                this.properties.widgetPrice.SetVisualAsEnoughMoney();
            else
                this.properties.widgetPrice.SetVisualAsNotEnoughMoney();
        }

        private void UpdateLocalization() {
            if (!Localization.isInitialized)
                return;
            
            string localizingString = Localization.GetTranslation("ID_UPGRADE_CARD_DESC");
            properties.textDescription.text = string.Format(localizingString, nextLevelNumber);
        }

        private void OnEnable() {
            Localization.OnLanguageChanged += OnLanguageChanged;

            if (Game.isInitialized) {
                this.Subscribe();
                this.SetupPriceVisual();
                UpdateLocalization();
            }
        }
        

        private void Subscribe() {
            Bank.uiBank.OnStateChangedEvent += this.OnBankStateChanged;
        }

        #region EVENTS

        private void OnBankStateChanged(object sender) {
            this.SetupPriceVisual();
        }

        private void OnLanguageChanged() {
            UpdateLocalization();
        }

        #endregion

        
        private void OnDisable() {
            if (Game.isInitialized)
                Bank.uiBank.OnStateChangedEvent -= this.OnBankStateChanged;
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }*/
    }
}