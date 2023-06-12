using SinSity.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.UI
{
    public sealed class UIWidgetPanelResearchPriceButton : UIWidgetPanelResearchButton
    {
        
        [SerializeField] private Text textPrice;
        [SerializeField] private Button button;
        [Space]        
        [SerializeField] private Image imgBackground;
        [SerializeField] private Sprite spriteBackgroundDisable;
        [SerializeField] private Sprite spriteBackgroundEnable;
        [Space] 
        [SerializeField] private Image imgHighlight;
        [SerializeField] private Sprite spriteHighlightEnable;
        [SerializeField] private Sprite spriteHighlightDisable;
        
        private BigNumber currentPrice;

        public void SetupPrice(BigNumber price)
        {
            this.currentPrice = price;
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            this.textPrice.text = price.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            this.RefreshPrice();
        }

        private void RefreshPrice() {
            var isSoftCurrencyEnough = Bank.softCurrencyCount > this.currentPrice;
            if (isSoftCurrencyEnough)
                SetVisualAsEnabled();
            else
                SetVisualAsDisabled();
        }

        private void SetVisualAsEnabled() {
            this.imgBackground.sprite = this.spriteBackgroundEnable;
            this.imgHighlight.sprite = this.spriteHighlightEnable;
        }

        private void SetVisualAsDisabled() {
            this.imgBackground.sprite = this.spriteBackgroundDisable;
            this.imgHighlight.sprite = this.spriteHighlightDisable;
        }

        public override bool AllowToUse() {
            return Bank.isEnoughtSoftCurrency(this.currentPrice);
        }

        private void OnEnable()
        {
            Bank.uiBank.OnStateChangedEvent += this.OnBankStateChanged;
        }

        

        private void OnDisable() {
            Bank.uiBank.OnStateChangedEvent -= this.OnBankStateChanged;
        }

        #region Events

        private void OnBankStateChanged(object sender) {
            this.RefreshPrice();
        }

        #endregion
    }
}