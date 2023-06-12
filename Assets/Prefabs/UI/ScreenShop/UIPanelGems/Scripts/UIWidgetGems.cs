using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetGems : UIWidget<UIWidgetGemsProperties>, IBankStateWithoutNotification {
        private Product product;
        private UIController uiController;
        private UIPopupIAPLoading popupIApLoading;

        private void OnEnable() {
            properties.btn.onClick.AddListener(OnClick);
            Localization.OnLanguageChanged += OnLanguageChanged;
            
            UpdateTitle();
            if (product != null)
                SetupPrice(product);
        }

        private void UpdateTitle() {
            if (product != null) SetupTitle(product);
        }

        private void OnDisable() {
            properties.btn.onClick.RemoveListener(OnClick);
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }

        public void Setup(Product product) {
            var uiInteractor = this.GetInteractor<UIInteractor>();
            this.uiController = uiInteractor.uiController;
            
            this.product = product;

            //SetupBackgroundSprite(product);
            SetupValueCount(product);
            SetupTitle(product);
            SetupIcon(product);
            SetupPrice(product);
        }

        private void SetupBackgroundSprite(Product product) {
            PaymentType paymentType = product.info.paymentType;
            if (paymentType == PaymentType.ADS)
                properties.ActivateBackPurple();
            else
                properties.ActivateBackYellow();
        }

        private void SetupValueCount(Product product) {
            ProductInfoGems infoGems = product.info as ProductInfoGems;
            properties.textCountValue.text = infoGems.gemsCount.ToString();
        }
        
        private void SetupTitle(Product product) {
            var title = Localization.GetTranslation(product.info.GetTitle());
            properties.textTitle.text = title;
        }
        
        private void SetupIcon(Product product) {
            var sprite = product.info.GetSpriteIcon();
            properties.imgIcon.sprite = sprite;
        }
        
        private void SetupPrice(Product product) {
            properties.panelPrice.SetPrice(product);
        }

        #region EVENTS

        private void OnClick() {
            /*if (this.popupIApLoading != null)
                return;
            
            if (this.product.info.isRealPayment)
                this.popupIApLoading = this.uiController.Show<UIPopupIAPLoading>();
            Shop.PurchaseProduct(product, OnProductPurchased);*/
            SFX.PlaySFX(this.properties.audioClipClick);
            var infoGems = product.info as ProductInfoGems;

            var objectEcoClicker = new UIObjectEcoClicker(transform.position);
            FXGemsGenerator.MakeFXLog(objectEcoClicker, infoGems.gemsCount);
            Bank.AddHardCurrency(infoGems.gemsCount, this);
        }

        private void OnLanguageChanged() {
            UpdateTitle();
            
            if (product != null)
                SetupPrice(product);
            
        }
        
        private void OnProductPurchased(Product purchasedProduct, bool success) {
            if (success) {
                SFX.PlaySFX(this.properties.audioClipClick);
                ProductInfoGems infoGems = purchasedProduct.info as ProductInfoGems;

                UIObjectEcoClicker objectEcoClicker = new UIObjectEcoClicker(transform.position);
                FXGemsGenerator.MakeFXLog(objectEcoClicker, infoGems.gemsCount);
                Bank.AddHardCurrency(infoGems.gemsCount, this);
            }

            if (this.popupIApLoading != null) {
                this.popupIApLoading.Hide(); 
                this.popupIApLoading = null;
            }
        }

        #endregion
    }
}