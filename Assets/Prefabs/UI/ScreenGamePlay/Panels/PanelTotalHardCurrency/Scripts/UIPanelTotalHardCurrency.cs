using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPanelTotalHardCurrency : UIPanelCurrency
    {
        [SerializeField]
        private AudioClip audioClipClick;
        
        protected override void UpdateVisual() {
            this.properties.textTotalCollected.text = this.uiBank.hardCurrency.ToString();
        }

        protected override void SubscribeOnEvents() {
            this.uiBank.OnHardCurrencyReceivedEvent += this.OnHardCurrencyReceived;
            this.uiBank.OnHardCurrencySpentEvent += this.OnHardCurrencySpent;
        }

        protected override void UnsubscribeFromEvents() {
            this.uiBank.OnHardCurrencyReceivedEvent -= this.OnHardCurrencyReceived;
            this.uiBank.OnHardCurrencySpentEvent -= this.OnHardCurrencySpent;
        }


        #region Events

        protected override void OnBtnClick()
        {
            var uiInteractor = GetInteractor<UIInteractor>();
            var isAlreadyOpened = uiInteractor.uiController.IsActiveUIElement<UIPopupShop>();
            if (isAlreadyOpened)
            {
                return;
            }

            uiInteractor.ShowElement<UIPopupShop>();
            SFX.PlaySFX(this.audioClipClick);
        }
        
        private void OnHardCurrencyReceived(object sender, int value) {
            this.UpdateVisual();
            this.properties.PlayBounce();
        }
        
        private void OnHardCurrencySpent(object sender, int value) {
            this.UpdateVisual();
        }
        
        #endregion
    }
}