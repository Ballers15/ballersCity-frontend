using System;
using System.Collections.Generic;
using System.Linq;
using SinSity.Analytics;
using SinSity.Meta;
using SinSity.Tools;
using VavilichevGD.Audio;
using VavilichevGD.Meta.FortuneWheel.UI;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetFortuneWheel : UIWidget<UIWidgetFortuneWheelProperties> {

        private Dictionary<float, Reward> rewardsByAngleMap;
        private Dictionary<float, UIWidgetFortuneWheelSector> sectorsByAngleMap;

        private FortuneWheelInteractor interactor;
        private UIWidgetButtonFortuneWheelSpin activeButton;
        private EcoClickerTimer timerFreeSpin;

        public FortuneWheel fortuneWheel => this.properties.fortuneWheel;


        #region INITIALIZE

        protected override void Awake() {
            base.Awake(); 
            this.interactor = this.GetInteractor<FortuneWheelInteractor>();
            this.interactor.RegisterFortuneWheel(this.properties.fortuneWheel);
            this.rewardsByAngleMap = new Dictionary<float, Reward>();
            this.sectorsByAngleMap = new Dictionary<float, UIWidgetFortuneWheelSector>();
            
            this.SetupAllSectors();
            this.DisableAllButtons();
        }

        private void SetupAllSectors() {
            var config = this.properties.fortuneWheel.config;
            this.timerFreeSpin = new EcoClickerTimer(config.freeSpinPeriod);

            var sectorsData = config.GetSectorsData();
            var widgets = this.gameObject.GetComponentsInChildren<UIWidgetFortuneWheelSector>();

            foreach (var wheelSectorData in sectorsData) {
                var reward = new Reward(wheelSectorData.GetRewardInfo());
                this.rewardsByAngleMap[wheelSectorData.angle] = reward;
            }

            if (sectorsData.Length != widgets.Length)
                throw new Exception("Count of sectors data does not equals to ui widgets count");

            int count = sectorsData.Length;
            var rewards = this.rewardsByAngleMap.Values.ToArray();
            var angles = this.rewardsByAngleMap.Keys.ToArray();
            for (int i = 0; i < count; i++) {
                widgets[i].Setup(rewards[i]);
                var angle = angles[i];
                this.sectorsByAngleMap[angle] = widgets[i];
            }
        }
        
        private void DisableAllButtons() {
            this.properties.btnRotateFree.HideInstantly();
            this.properties.btnRotateForAD.HideInstantly();
            this.properties.btnRotateForGems.HideInstantly();
        }

        #endregion


        #region LIFECYCLE

        protected void OnEnable() {
            if (!this.interactor.isInitialized)
                return;
            
            this.interactor.OnFortuneWheelStateChangedEvent += this.OnFortuneWheelStateChanged;
            this.properties.lightsAnimator.PlayIdle();
            this.UpdateActiveButton();
            
            if (!this.interactor.CanUseFreeSpin() && !this.timerFreeSpin.isActive)
                this.ActivateTimer();
        }
        
        private void UpdateActiveButton() {
            var newButton = this.GetActualButton();
            
            if (this.activeButton != null)
                this.activeButton.HideInstantly();
            this.activeButton = newButton;
            this.activeButton.AddListener(this.OnRotateBtnClick);
            this.activeButton.Show();
            
            if (!this.fortuneWheel.isRotating)
                this.activeButton.SetInteractable(true);
        }
        
        protected void OnDisable() {
            this.interactor.OnFortuneWheelStateChangedEvent -= this.OnFortuneWheelStateChanged;
            if (this.timerFreeSpin.isActive) {
                this.timerFreeSpin.OnTimerCompletedEvent -= this.OnFreeSpinTimerCompleted;
                this.timerFreeSpin.Stop();
            }
        }

        #endregion

        
        
        private void ActivateTimer() {
            var totalSeconds = this.interactor.GetTimeToNextFreeSpin();
            this.timerFreeSpin.timerValue = totalSeconds;
            this.timerFreeSpin.Start();
            this.timerFreeSpin.OnTimerCompletedEvent += this.OnFreeSpinTimerCompleted;
        }
        
        private UIWidgetButtonFortuneWheelSpin GetActualButton() {
            if (this.interactor.CanUseFreeSpin())
                return this.properties.btnRotateFree;

            if (this.interactor.CanUseAdSpin())
                return this.properties.btnRotateForAD;

            this.UpdateGemsPrice();
            return this.properties.btnRotateForGems;
        }

        private void UpdateGemsPrice() {
            var price = this.interactor.gemsPrice;
            this.properties.textPriceRotateGorGems.text = price.ToString();
        }


        private void LogRotationStart() {
            var paymentType = PaymentType.HardCurrency;
            var price = this.interactor.gemsPrice;
            if (this.interactor.CanUseFreeSpin()) {
                paymentType = PaymentType.Free;
            }
            else if (this.interactor.CanUseAdSpin()) {
                paymentType = PaymentType.ADS;
            }
            FortuneWheelAnalytics.LogFortuneWheelRotate(paymentType, price);
        }

        private void LogRotationComplete(Reward reward) {
            FortuneWheelAnalytics.LogFortuneWheelRotateComplete(reward);
        }

       
        #region EVENTS

        private void OnRotateBtnClick() {
            if (this.properties.fortuneWheel.isRotating)
                return;
            
            this.interactor.TryToSpin(this.OnRotateStartResult);
        }

        private void OnRotateStartResult(bool success) {
            this.activeButton.SetInteractable(!success);
            if (!success)
                return;
            
            this.properties.lightsAnimator.PlayRotate();
            SFX.PlaySFX(this.properties.sfxSpin);

            var fortuneWheel = this.properties.fortuneWheel;
            fortuneWheel.OnRewardReceivedEvent += OnFortuneWheelRewardReceived;
            fortuneWheel.OnRotateOverEvent += OnFortuneWheelRotateOver;
            this.LogRotationStart();
        }


        private void OnFortuneWheelRotateOver(FortuneWheel fortunewheel) {
            this.properties.fortuneWheel.OnRotateOverEvent -= OnFortuneWheelRotateOver;
            this.properties.lightsAnimator.PlayBlinking();
        }
        

        private void OnFortuneWheelRewardReceived(FortuneWheel fortunewheel, FortuneWheelSectorData sectorData) {
            this.properties.fortuneWheel.OnRewardReceivedEvent -= OnFortuneWheelRewardReceived;
            var angle = sectorData.angle;
            var reward = this.rewardsByAngleMap[angle];

            var uiInteractor = this.GetInteractor<UIInteractor>();
            var uiController = uiInteractor.uiController;
            var popupReward = uiController.Show<UIPopupFortuneWheelReward>();

            var sectorWidget = this.sectorsByAngleMap[angle];
            var isJackpot = sectorWidget.jackpotSector;
            if (isJackpot)
                popupReward.SetupJackpot(reward);
            else
                popupReward.SetupSimple(reward);
            popupReward.OnUIElementClosedCompletelyEvent += this.OnPopupRewardClosedCompletely;
            this.LogRotationComplete(reward);
        }

        private void OnPopupRewardClosedCompletely(UIElement uielement) {
            var popup = uielement as UIPopupFortuneWheelReward;
            popup.OnUIElementClosedCompletelyEvent -= this.OnPopupRewardClosedCompletely;
            this.interactor.Save();
            this.properties.lightsAnimator.PlayIdle();
        }

        private void OnFortuneWheelStateChanged(FortuneWheelInteractor fortuneWheelInteractor) {
            if (!this.interactor.CanUseFreeSpin() && !this.timerFreeSpin.isActive)
                this.ActivateTimer();
            
            var newButton = this.GetActualButton();
            newButton.SetInteractable(true);
            if (newButton.Equals(this.activeButton))
                return;

            this.activeButton.Hide();
            this.activeButton.RemoveListener(this.OnRotateBtnClick);
            this.activeButton.OnUIElementClosedCompletelyEvent += this.OnActiveButtonClosedEvent;
        }

        private void OnActiveButtonClosedEvent(UIElement uiElement) {
            uiElement.OnUIElementClosedCompletelyEvent -= this.OnActiveButtonClosedEvent;
            this.activeButton = this.GetActualButton();
            this.activeButton.AddListener(this.OnRotateBtnClick);
            this.activeButton.Show();
        }

        
        private void OnFreeSpinTimerCompleted() {
            this.timerFreeSpin.OnTimerCompletedEvent -= this.OnFreeSpinTimerCompleted;
            this.OnFortuneWheelStateChanged(this.interactor);
        }

        #endregion
    }
}