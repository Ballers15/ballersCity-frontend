using SinSity.Meta.Rewards;
using SinSity.Services;
using Orego.Util;
using SinSity.Core;
using SinSity.Monetization;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public class UIWidgetCase : UIWidgetAnim<UIWidgetCaseProperties>
    {
        private Product product;
        private ProductStateCase state;
        private static readonly int TRIGGER_NOT_ENOUGH_GEMS = Animator.StringToHash("not_enough_gems");


        private void OnEnable()
        {
            properties.btn.onClick.AddListener(OnClick);
            UpdateInfoLight(product);
            
            UIPopupRewardSummary.OnOpenNextCaseBtnClicked += OnOpenNextCaseBtnClicked;
            Localization.OnLanguageChanged += OnLanguageChanged;
            UpdateTitle();
        }

       

        private void OnClick()
        {
            if (state.countCurrent <= 0) {
                /*if (product.info.isADSPayment)
                    Shop.PurchaseProduct(product, OnProductPurhased);
                else
                    ShowPurchaseAgreementPopup();*/
                state.AddCase();
                OpenCase();
            }
            else {
                OpenCase();
            }
        }

        private void ShowPurchaseAgreementPopup() {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            UIPopupPurchaseAgreement popup = uiInteractor.ShowElement<UIPopupPurchaseAgreement>();
            string title = Localization.GetTranslation(product.info.GetTitle());
            popup.Setup(title);
            popup.OnDialogueResults += args => {
                if (args.result == UIPopupResult.Apply)
                    Shop.PurchaseProduct(product, OnProductPurhased);
            };
        }

        private void OpenCase() {
            this.state.SpendCase();
            var productInfoCase = this.product.GetInfo<ProductInfoCase>();
            var rewardInfoSet = productInfoCase.GetRewardInfoSet();
            ProductStateCase.ApplyRewards(rewardInfoSet);
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var popupCaseOpening = uiInteractor.ShowElement<UIPopupCaseOpening>();
            popupCaseOpening.Setup(productInfoCase, rewardInfoSet);
            this.Resetup();
            SFX.PlaySFX(this.properties.audioClipOpenCase);
        }

        private void OnProductPurhased(Product purchasedProduct, bool success)
        {
            if (success)
            {
                OpenCase();
            }
            else
            {
                SFX.PlaySFX(this.properties.audioClipNotEnoughGems);
                if (!purchasedProduct.info.isADSPayment)
                    SetTrigger(TRIGGER_NOT_ENOUGH_GEMS);
            }
        }
        
        private void OnOpenNextCaseBtnClicked() {
            this.Resetup();
        }
        
        private void OnLanguageChanged() {
            UpdateTitle();
            
            if (product != null)
                SetupPrice(product);
        }
        
        private void UpdateTitle() {
            if (product != null)
                SetupTitle(product);
        }

        private void OnDisable()
        {
            properties.btn.onClick.RemoveListener(OnClick);

            UIPopupRewardSummary.OnOpenNextCaseBtnClicked -= OnOpenNextCaseBtnClicked;
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }


        public void Setup(Product product)
        {
            this.product = product;
            state = product.state as ProductStateCase;

            SetupIcon(product);
            SetupTitle(product);
            UpdateInfoLight(product);
        }
        
        private void SetupIcon(Product product)
        {
            properties.imgIcon.sprite = product.info.GetSpriteIcon();
        }

        private void SetupTitle(Product product)
        {
            properties.textCaseTitle.text = Localization.GetTranslation(product.info.GetTitle());
        }

        private void UpdateInfoLight(Product product)
        {
            if (product == null)
                return;
            
            //SetupCountValue(product);
            //SetupPrice(product);
        }

        private void SetupBackground(ProductStateCase stateCase)
        {
            if (stateCase.countCurrent > 0)
            {
                properties.ActivateYellowBack();
                return;
            }

            switch (product.info.paymentType)
            {
                case PaymentType.ADS:
                    properties.ActivatePurpleBack();
                    break;
                default:
                    properties.ActivateYellowBack();
                    break;
            }
        }

        private void SetupCountValue(Product product)
        {
            ProductStateCase state = (ProductStateCase) product.state;
            bool panelIsActive = state.countCurrent > 0;
            properties.goContainerCount.SetActive(panelIsActive);

            if (panelIsActive)
                properties.textCount.text = state.countCurrent.ToString();
        }

        private void SetupPrice(Product product)
        {
            ProductStateCase state = (ProductStateCase) product.state;
            var enableGet = state.countCurrent > 0;
            properties.panelPrice.SetPrice(product, enableGet);
        }


        public void Resetup()
        {
            UpdateInfoLight(product);
        }
    }
}