using System.Collections;
using SinSity.Services;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using VavilichevGD.LocalizationFramework;
using Orego.Util;
using SinSity.Domain;
using SinSity.Meta.Rewards;
using SinSity.Monetization;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIWidgetTimeBooster : UIWidgetAnim<UIWidgetTimeBoosterProperties> {

        private Coroutine routine;

        private Product product;
        private static readonly int TRIGGER_NOT_ENOUGH_GEMS = Animator.StringToHash("not_enough_gems");

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            properties.btn.onClick.AddListener(OnClick);
            RewardHandlerTimeBooster.OnTimeBoosterRewarded += OnTimeBoosterRewarded;
            Localization.OnLanguageChanged += OnLanguageChanged;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            if (product != null)
                SetupTitle(product);
        }

        private void OnClick()
        {
            Debug.Log("TIME BOOSTER CLICK!");
            if (this.routine != null)
                return;

            var productState = this.product.GetState<ProductStateTimeBooster>();
            /*if (productState.countCurrent > 0)
                this.ActivateBooster(product);
            else
                Shop.PurchaseProduct(product, OnProductPurchased);*/
            productState.AddBooster();
            ActivateBooster(product);
            //ShowPurchaseAgreementPopup();
        }

        private void ShowPurchaseAgreementPopup()
        {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            UIPopupPurchaseAgreement popup = uiInteractor.ShowElement<UIPopupPurchaseAgreement>();

            ProductInfoTimeBooster infoBooster = product.info as ProductInfoTimeBooster;
            string localizedTitle = Localization.GetTranslation(infoBooster.GetTitle());
            string title = string.Format(localizedTitle, infoBooster.timeHours);
            popup.Setup(title);

            popup.OnDialogueResults += args =>
            {
                if (args.result == UIPopupResult.Apply)
                    Shop.PurchaseProduct(product, OnProductPurchased);
            };
        }

        private void ActivateBooster(Product activatingProduct)
        {
            Debug.Log("ACTIVATE TIME BOOSTER!");
            this.routine = Coroutines.StartRoutine(this.ActivateTimeBoosterRoutine());
            //this.UpdateInfoLight(this.product);
            this.ActivateFX();
            SFX.PlaySFX(this.properties.audioClipActivateTimeBooster);
        }

        private void OnProductPurchased(Product purchasedProduct, bool success)
        {
            if (!success)
            {
                this.animator.SetTrigger(TRIGGER_NOT_ENOUGH_GEMS);
                SFX.PlaySFX(this.properties.audioClipNotEnoughGems);
                return;
            }

            this.ActivateBooster(purchasedProduct);
        }

        private void ActivateFX()
        {
            UIPopupShop popupShop = gameObject.GetComponentInParent<UIPopupShop>();
            popupShop.ActivateTimeBoosterFX();
        }

        private IEnumerator ActivateTimeBoosterRoutine() {
//            Debug.Log("START TIME BOOSTER ROUTINE!");
            var activationInteractor = this.GetInteractor<TimeBoosterActivationInteractor>();
            yield return activationInteractor.ActivateTimeBooster(this.product);
            this.routine = null;
//            Debug.Log("COMPLETE TIME BOOSTER ROUTINE!");
        }

        private void OnTimeBoosterRewarded(Reward reward)
        {
            this.Resetup();
        }

        private void OnLanguageChanged()
        {
            UpdateTitle();
        }

        private void OnDisable()
        {
            properties.btn.onClick.RemoveListener(OnClick);
            RewardHandlerTimeBooster.OnTimeBoosterRewarded -= OnTimeBoosterRewarded;
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }


        public void Setup(Product product)
        {
            this.product = product;

            SetupIcon(product);
            SetupTitle(product);
            //SetupPrice(product);
            //UpdateInfoLight(product);
        }

        private void SetupIcon(Product product)
        {
            properties.imgIcon.sprite = product.info.GetSpriteIcon();
        }

        private void SetupTitle(Product product)
        {
            ProductInfoTimeBooster infoBooster = product.info as ProductInfoTimeBooster;
            string localizedTitle = Localization.GetTranslation(infoBooster.GetTitle());
            properties.textTitle.text = string.Format(localizedTitle, infoBooster.timeHours);
        }

        private void SetupPrice(Product product)
        {
            ProductStateTimeBooster state = product.state as ProductStateTimeBooster;
            bool enableGet = state.countCurrent > 0;
            properties.panelPrice.SetPrice(product, enableGet);
        }

        private void UpdateInfoLight(Product product)
        {
            SetupCurrentCountValue(product);
            SetupPrice(product);
        }

        private void SetupCurrentCountValue(Product product)
        {
            ProductStateTimeBooster state = product.state as ProductStateTimeBooster;
            int count = state.countCurrent;
            bool panelIsActive = count > 0;
            properties.panelTimeBoosterValue.SetActive(panelIsActive);
            if (panelIsActive)
                properties.textCountCurrentValue.text = count.ToString();
        }


        public void Resetup()
        {
            //UpdateInfoLight(product);
        }
    }
}