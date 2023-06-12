using SinSity.Services;
using Orego.Util;
using Prefabs.AirDrop.Scripts.Analytics;
using SinSity.Domain;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPopupAirDrop : UIPopupAnim<UIPopupAirDropProperties, UIPopupArgs> {

        private AirDropAnalytics analytics;
        private string type;
        private string rewardType;
        
        public override void Initialize()
        {
            base.Initialize();
            HideInstantly();

            var airDropInteractor = this.GetInteractor<AirDropInteractor>();
            this.analytics = airDropInteractor.analytics;
        }

        public void Setup(RewardInfo rewardInfo)
        {
            if (rewardInfo is RewardInfoSetupSoftCurrency rewardInfoSoftCurrency) {
                this.rewardType = AirDropAnalytics.REWARD_CLEAN_ENERGY;
                this.Setup(rewardInfoSoftCurrency.value);
            }
            else if (rewardInfo is RewardInfoSetupHardCurrency rewardInfoHardCurrency) {
                this.rewardType = AirDropAnalytics.REWARD_GEMS;
                this.Setup(rewardInfoHardCurrency.value);
            }
        }

        public void SetupAsAdAirShip()
        {
            this.properties.imgAirShip.sprite = this.properties.iconAdAirShip;
            this.type = AirDropAnalytics.TYPE_AD;
        }
        
        public void SetupAsRegularAirShip()
        {
            this.properties.imgAirShip.sprite = this.properties.iconRegularAirShip;
            this.type = AirDropAnalytics.TYPE_REGULAR;
        }

        private void Setup(int gemsValue)
        {
            this.properties.SetPackageValue(gemsValue);
        }

        private void Setup(BigNumber cleanEnergyValue)
        {
            this.properties.SetPackageValue(cleanEnergyValue);
        }

        private void OnEnable()
        {
            this.properties.btnGet.AddListener(OnGetBtnClick);
            this.properties.btnClose.AddListener(OnCloseBtnClick);
        }

        private void OnGetBtnClick()
        {
            this.properties.btnClose.RemoveListener(OnCloseBtnClick);
            this.properties.PlayClick();

            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var popupAdLoading = uiInteractor.ShowElement<UIPopupADLoading>();
            popupAdLoading.OnADResultsReceived += this.OnAdResultsReceived;
            popupAdLoading.ShowAD("airdrop");
        }

        private void OnAdResultsReceived(UIPopupADLoading popup, bool success, string error) {
            popup.OnADResultsReceived -= OnAdResultsReceived;
            

            var results = success ? UIPopupResult.Apply : UIPopupResult.Error;
            var args = new UIPopupArgs(this, results);
            this.NotifyAboutResults(args);

            Hide();
            
            this.analytics.LogAirDropResult(this.type, AirDropAnalytics.RESULT_ADWATCHED, this.rewardType);
        }

        private void OnCloseBtnClick()
        {
            this.NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Close));
            this.properties.PlayClose();
            Hide();
            
            this.analytics.LogAirDropResult(this.type, AirDropAnalytics.RESULT_CANCELED, this.rewardType);
        }

        private void OnDisable()
        {
            this.properties.btnGet.RemoveListener(OnGetBtnClick);
            this.properties.btnClose.RemoveListener(OnCloseBtnClick);
        }
    }
}