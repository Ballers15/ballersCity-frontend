using SinSity.Services;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIAirDropController : UIElement
    {
        private AirShipController airShipController;

        private AirShipBehaviour currentAirShipBehaviour;
        private UIInteractor uiInteractor;

        protected override void OnGameInitialized()
        {
            this.uiInteractor = this.GetInteractor<UIInteractor>();
            var airDropInteractor = this.GetInteractor<AirDropInteractor>();
            this.airShipController = airDropInteractor.airShipController;
            this.airShipController.OnAirShipClickedEvent += this.OnAirShipClicked;
        }

        private void OnAirShipClicked(AirShipBehaviour airShipBehaviour)
        {    
            if (uiInteractor.uiController.IsActiveUIElement<UIPopupAirDrop>())
                return;
            var popup = uiInteractor.GetUIElement<UIPopupAirDrop>();
            
            if (airShipBehaviour is AirShipBehaviorAD)
                popup.SetupAsAdAirShip();
            else
                popup.SetupAsRegularAirShip();
            this.SetupPopup(airShipBehaviour,popup);
        }
        
        private void OnAdAirShipClicked(AirShipBehaviour airShipBehaviour)
        {    
            if (uiInteractor.uiController.IsActiveUIElement<UIPopupAirDrop>())
                return;
            var popup = uiInteractor.GetUIElement<UIPopupAirDrop>();
            popup.SetupAsAdAirShip();
            this.SetupPopup(airShipBehaviour,popup);
        }

        
        private void SetupPopup(AirShipBehaviour airShipBehaviour,UIPopupAirDrop popup)
        {
            this.currentAirShipBehaviour = airShipBehaviour;
            if (uiInteractor.uiController.IsActiveUIElement<UIPopupAirDrop>())
                return;

            var rewardInfo = airShipBehaviour.rewardInfo;
            if (airShipBehaviour.needToWatchAds) {
                popup.Setup(rewardInfo);
                popup.Show();
                popup.OnDialogueResults += OnDialogueResults;
                this.currentAirShipBehaviour.Disable();
                this.HandlePopupResults(airShipBehaviour, popup);
            }
            else
            {
                this.ReceiveReward(rewardInfo);
            }
        }

        private void HandlePopupResults(AirShipBehaviour airShipBehaviour, UIPopupAirDrop popup) {
            void OnPopupClosed(UIElement uiElement) {
                uiElement.OnUIElementClosedCompletelyEvent -= OnPopupClosed;
                if (airShipBehaviour is AirShipBehaviorAD airShipBehaviorAd) 
                    airShipBehaviorAd.FlyAway();
            }

            popup.OnUIElementClosedCompletelyEvent += OnPopupClosed;
        }

        private void OnDialogueResults(UIPopupArgs e)
        {
            var popup = e.uiElement as UIPopupAirDrop;
            popup.OnDialogueResults -= this.OnDialogueResults;
            
            var uiPopupResult = e.result;
            var rewardInfo = this.currentAirShipBehaviour.rewardInfo;
            if (uiPopupResult == UIPopupResult.Apply)
            {
                this.ReceiveReward(rewardInfo);
            }

            if (uiPopupResult == UIPopupResult.Close)
            {
                this.airShipController.parcelSupplier.SkipParcel();
            }
            
            this.currentAirShipBehaviour.Enable();
        }

        private void OnDestroy()
        {
            this.airShipController.OnAirShipClickedEvent -= this.OnAirShipClicked;
        }

        private void ReceiveReward(RewardInfo rewardInfo)
        {
            this.currentAirShipBehaviour.DropParcel();
            this.currentAirShipBehaviour.OnDroppingBoxExploded += (droppingBox =>
            {
                this.airShipController.ReceiveAirDropReward(this.currentAirShipBehaviour);
                this.MakeFX(rewardInfo, droppingBox.boxShapePosition);
            });
        }


        private void MakeFX(RewardInfo rewardInfo, Vector3 position) {
            WorldObjectEcoClicker objectEcoClicker = new WorldObjectEcoClicker(position);

            if (rewardInfo is RewardInfoSetupSoftCurrency) {
                RewardInfoSetupSoftCurrency rewardInfoSetupSoftCurrency = rewardInfo as RewardInfoSetupSoftCurrency;
                
                FXCleanEnergyGenerator.MakeFXSlow(objectEcoClicker, rewardInfoSetupSoftCurrency.value);
                var dictionary = BigNumberLocalizator.GetSimpleDictionary();
                var numberLocalized = rewardInfoSetupSoftCurrency.value.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
                FXTextGenerator.MakeFX(objectEcoClicker, numberLocalized);
            }
            else if (rewardInfo is RewardInfoSetupHardCurrency) {
                RewardInfoSetupHardCurrency rewardInfoSetupHardCurrency = rewardInfo as RewardInfoSetupHardCurrency;

                FXGemsGenerator.MakeFX(objectEcoClicker, rewardInfoSetupHardCurrency.value);
                FXTextGenerator.MakeFX(objectEcoClicker, rewardInfoSetupHardCurrency.value.ToString());
            }
        }
    }
}