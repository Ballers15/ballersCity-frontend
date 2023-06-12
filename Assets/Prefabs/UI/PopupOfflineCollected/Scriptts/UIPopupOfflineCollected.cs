using SinSity.Tools;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPopupOfflineCollected : UIPopupAnim<UIPopupOfflineCollectedProperties, UIPopupArgs>
    {
        #region Event

        public AutoEvent OnDoubleForAdClickEvent { get; }

        public AutoEvent OnTripleForGemsClickEvent { get; }

        public AutoEvent OnCloseClickEvent { get; }
        
        public AutoEvent OnGetSoftCurrencyClickEvent { get; }

        #endregion

        public UIPopupOfflineCollected()
        {
            this.OnDoubleForAdClickEvent = new AutoEvent();
            this.OnTripleForGemsClickEvent = new AutoEvent();
            this.OnCloseClickEvent = new AutoEvent();
            this.OnGetSoftCurrencyClickEvent = new AutoEvent();
        }

        #region OnEnable

        private void OnEnable()
        {
            this.SetupVisual(true);
            this.properties.btnDoubleForAd.AddListener(this.OnDoubleForAdClick);
            this.properties.btnTripleForGems.AddListener(this.OnTripleForGemsClick);
            this.properties.btnGet.AddListener(this.OnGetSoftCurrencyClick);
            this.properties.btnClose.AddListener(this.OnCloseClick);
        }

        private void OnCloseClick()
        {
            SFX.PlaySFX(this.properties.audioClipCloseClick);
            this.OnCloseClickEvent.Invoke();
        }

        private void OnGetSoftCurrencyClick()
        {
            SFX.PlaySFX(this.properties.audioClipGetClick); 
            this.OnGetSoftCurrencyClickEvent?.Invoke();
        }

        private void OnTripleForGemsClick()
        {
            this.OnTripleForGemsClickEvent.Invoke();
        }

        private void OnDoubleForAdClick()
        {
            SFX.PlaySFX(this.properties.audioClipMultiplySoftCurrencyClick);
            this.OnDoubleForAdClickEvent.Invoke();
        }

        #endregion

        #region OnDisable

        private void OnDisable()
        {
            this.properties.btnDoubleForAd.RemoveListeners();
            this.properties.btnTripleForGems.RemoveListeners();
            this.properties.btnClose.RemoveListeners();
            this.properties.btnGet.RemoveListeners();
        }

        #endregion

        #region Setup

        public void Setup(OfflineCollectedCurrencyArgs args)
        {
            this.SetupTime(args.offlineSeconds);
            this.SetupCollectedCurrency(args.offlineCollectedCurrency);
            this.SetupPrice(args.x3ForGemsPrice);
        }

        private void SetupTime(double secondsPassed)
        {
            string localizingText = Localization.GetTranslation("ID_TIME");
            var hours = Mathf.FloorToInt((float) (secondsPassed / 3600));
            var min = Mathf.FloorToInt((float) (secondsPassed % 3600 / 60));
            var valueText = string.Format(localizingText, hours, min);
            this.properties.textTimeValue.text = valueText;
        }

        private void SetupCollectedCurrency(BigNumber collectedCurrency)
        {
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            var collectedCleanEnergy = collectedCurrency.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            this.properties.panelCollectedCurrencyValue.SetText(collectedCleanEnergy);
        }

        private void SetupPrice(int gemsPrice)
        {
            this.properties.panelPriceTripleCurrency.SetPrice(gemsPrice);
        }

        #endregion

        public void ResetupAsIncomeMultiplyed(BigNumber offlineCollectedCurrency, int times)
        {
            BigNumber newCollectedCurrency = offlineCollectedCurrency * times;
            SetupCollectedCurrency(newCollectedCurrency);
            SetupVisual(false);
        }

        private void SetupVisual(bool asDefault)
        {
            this.properties.transformContainerIncreaseIncome.gameObject.SetActive(asDefault);
            this.properties.btnGet.gameObject.SetActive(!asDefault);
            this.properties.btnClose.gameObject.SetActive(asDefault);
        }

        public void PlayTrippleForGemsError() {
            this.properties.PlayError_BtnTripleForGems();
        }

        public void PlaySuccessSFX() {
            SFX.PlaySFX(this.properties.audioClipMultiplySoftCurrencyClick);
        }
    }
}