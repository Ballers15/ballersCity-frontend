using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI {
    public abstract class UIPanelCurrency : UIPanel<UIPanelCurrencyProperties> {

        protected UIBank uiBank;
        protected bool isInitialized => this.uiBank != null;
        
        
        protected virtual void OnEnable() {
            if (this.isInitialized) {
                this.UpdateVisual();
                this.SubscribeOnEvents();
            }
            else {
                if (Game.isInitialized)            
                    this.LocalInitialize();
                else
                    Game.OnGameInitialized += this.OnGameInitialized;
            }
                
            if (properties.btn)
                properties.btn.onClick.AddListener(OnBtnClick);
        }

        private void LocalInitialize() {
            var bankInteractor = this.GetInteractor<BankInteractor>();
            this.uiBank = bankInteractor.uiBank;
            
            this.UpdateVisual();
            this.SubscribeOnEvents();
        }

        protected virtual void OnBtnClick() {
            Debug.Log($"{gameObject.name} clicked.");
        }

        protected virtual void OnGameInitialized(Game game) {
            this.LocalInitialize();
        }

        protected abstract void UpdateVisual();
        protected abstract void SubscribeOnEvents();
        protected abstract void UnsubscribeFromEvents();
        

        protected virtual void OnDisable() {
            Game.OnGameInitialized -= OnGameInitialized;
            if (properties.btn)
                properties.btn.onClick.RemoveListener(OnBtnClick);
            if (this.isInitialized)
                this.UnsubscribeFromEvents();
        }
    }
}