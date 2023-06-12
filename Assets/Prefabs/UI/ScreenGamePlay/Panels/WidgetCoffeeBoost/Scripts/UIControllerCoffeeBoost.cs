using SinSity.Services;
using SinSity.UI;
using IdleClicker.Gameplay;
using Orego.Util;
using Prefabs.UI.ScreenGamePlay.Panels.WidgetCoffeeBoost;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace IdleClicker.UI {
    public class UIControllerCoffeeBoost : MonoBehaviour {

        [SerializeField] private Properties properties;

        private CoffeeBoostInteractor interactor;
        private UIController uiController;
        private CoffeeBoost coffeeBoost;
        private UIWidgetCoffeeBoost widgetCoffeeBoost;
        private int timeForAD;
        private int timerValue;
        private int time;

        
        #region START

        private void Start() {
            this.Initialize();

            this.widgetCoffeeBoost.HideInstantly();
            if (this.interactor.isCoffeeBoostUnlocked)
                this.SubscribeOnEvents();
            else
                this.interactor.OnCoffeeBoostUnlockedEvent += this.OnCoffeeBoostUnlocked;
        }

        private void Initialize() {
            this.interactor = Game.GetInteractor<CoffeeBoostInteractor>();
            this.uiController = Game.GetInteractor<UIInteractor>().uiController;
            var screenGameplay = uiController.GetUIElement<UIScreenGamePlay>();
            this.widgetCoffeeBoost = screenGameplay.transform.GetComponentInChildren<UIWidgetCoffeeBoost>();
            
            var coffeeBoostInteractor = Game.GetInteractor<CoffeeBoostInteractor>();
            this.coffeeBoost = coffeeBoostInteractor.coffeeBoost;

            this.timerValue = this.properties.restPeriodRandom;
            this.timeForAD = 0;
            this.time = 0;
        }

        private void SubscribeOnEvents() {
            GameTime.OnSecondTickEvent += this.GameTimeOnSecondTick;
            this.widgetCoffeeBoost.OnClickedEvent += this.WidgetCoffeeBoostOnClicked;
        }

        #endregion
        
        
        private void OnDestroy() {
            GameTime.OnSecondTickEvent -= this.GameTimeOnSecondTick;
            this.widgetCoffeeBoost.OnClickedEvent -= this.WidgetCoffeeBoostOnClicked;
            this.interactor.OnCoffeeBoostUnlockedEvent -= this.OnCoffeeBoostUnlocked;
        }


        #region EVENTS
        
        private void OnCoffeeBoostUnlocked(object sender) {
            this.interactor.OnCoffeeBoostUnlockedEvent -= this.OnCoffeeBoostUnlocked;
            this.SubscribeOnEvents();
        }

        private void GameTimeOnSecondTick() {
            this.timeForAD += 1;

            if (!this.CanCalculateTimer())
                return;
            
            this.time += 1;
            if (this.CanShowCoffeeBoostWidget()) {
                this.time = 0;

                var actualConfig = coffeeBoost.actualConfig;
                var uiCupPrefab = actualConfig.cupPrefabForGameplayScreen;
                this.widgetCoffeeBoost.Show();
                widgetCoffeeBoost.Setup(uiCupPrefab);
            }
        }

        private bool CanCalculateTimer() {
            return !this.widgetCoffeeBoost.isActive && !this.coffeeBoost.isActive;
        }

        private bool CanShowCoffeeBoostWidget() {
            return this.time >= this.timerValue && !this.uiController.AnyScreenEnabled();
        }

        private void WidgetCoffeeBoostOnClicked(UIWidgetCoffeeBoost widget) {
            var popup = this.uiController.Show<UIPopupCoffeeBoost>();
            this.time = 0;
            
            if (this.ShouldShowADSetup()) {
                popup.SetupOfferADS(this.coffeeBoost.actualConfig);
                popup.OnDialogueResults += this.OnADSOfferResults;
            }
            else {
                popup.SetupOfferGems(this.coffeeBoost.actualConfig, this.properties.priceGems);
                popup.OnDialogueResults += OnGemsOfferResults;
            }
            
        }
        
        private bool ShouldShowADSetup() {
            if (this.timeForAD >= this.properties.adPeriod || !Bank.isEnoughtHardCurrency(this.properties.priceGems)) {
                this.timeForAD = 0;
                return true;
            }

            return false;
        }

        private void OnGemsOfferResults(UIPopupArgs e) {
            var popup = (UIPopupCoffeeBoost) e.uiElement;
            popup.OnDialogueResults -= this.OnGemsOfferResults;

            if (e.result == UIPopupResult.Apply)
            {
                if (!Bank.isEnoughtHardCurrency(this.properties.priceGems)) return;
                Bank.SpendHardCurrency(this.properties.priceGems, this);
                this.coffeeBoost.Activate();
            }
        }

        private void OnADSOfferResults(UIPopupArgs e) {
            var popup = (UIPopupCoffeeBoost) e.uiElement;
            popup.OnDialogueResults -= this.OnADSOfferResults;

            if (e.result == UIPopupResult.Apply) {
                
                void OnAdResultsReceived(UIPopupADLoading popupAdLoading, bool success, string error) {
                    popupAdLoading.OnADResultsReceived -= OnAdResultsReceived;
                    if (success)
                        this.coffeeBoost.Activate();
                }
                
                var popupAd = this.uiController.Show<UIPopupADLoading>();
                popupAd.OnADResultsReceived += OnAdResultsReceived;
                popupAd.ShowAD("coffee_boost");
            }
        }

        

        #endregion
        
        
        [System.Serializable]
        private class Properties {
            public int restPeriodMin = 10;
            public int restPeriodMax = 20;
            [Space] 
            public int priceGems;
            public int adPeriod = 60;
           
            public int restPeriodRandom => Random.Range(restPeriodMin, restPeriodMax);
        }
        
    }
}