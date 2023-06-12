using System;
using SinSity.Analytics;
using SinSity.Tools;
using UnityEngine;
using UnityEngine.Events;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Meta {
    public class FortuneWheelInteractor : Interactor {

        #region CONSTANTS

        private const bool SUCCESS = true;
        private const bool FAIL = false;

        #endregion
        
        #region DELEGATES

        public delegate void FortuneWheelInteractorHandler(FortuneWheelInteractor interactor);
        public event FortuneWheelInteractorHandler OnFortuneWheelUnlockedEvent;
        public event FortuneWheelInteractorHandler OnFortuneWheelStateChangedEvent;
        public event FortuneWheelInteractorHandler OnDataDefinedEvent;

        #endregion
        

        public FortuneWheel fortuneWheel { get; private set; }
        public bool dataWasDefined;
        public bool isUnlocked => this.data.isUnlocked;
        public int gemsPrice => this.data.gemPriceCurrent;
        public override bool onCreateInstantly { get; } = true;
        
        private FortuneWheelRepository repository;
        private FortuneWheelData data;
        private FortuneWheelConfig config;


        protected override void Initialize() {
            base.Initialize();
            
            if (!GameTime.isInitialized)
                GameTime.OnGameTimeInitialized += this.OnGameTimeInitialized;
            else
                this.SetupData();
        }

        private void OnGameTimeInitialized() {
            GameTime.OnGameTimeInitialized -= this.OnGameTimeInitialized;
            this.SetupData();
        }

        private void SetupData() {
            this.repository = this.GetRepository<FortuneWheelRepository>();
            this.data = this.repository.data;
            this.config = this.repository.config;

            if (!this.data.isUnlocked)
                this.CheckUnlocked();

            this.dataWasDefined = true;
            this.OnDataDefinedEvent?.Invoke(this);
        }
        
        
        private void CheckUnlocked() {
            var firstPlayTime = this.data.firstPlayTime.GetDateTime();
            Debug.Log($"FistPlayDate: {firstPlayTime.ToString()} today is: {GameTime.now} and difference is: {(GameTime.now - firstPlayTime).Days}");
            var dayXHasCome = (GameTime.now - firstPlayTime).Days >= this.config.unlockSpinDays;
            if (dayXHasCome)
                this.Unlock();
        }


        public void RegisterFortuneWheel(FortuneWheel fortuneWheel) {
            this.fortuneWheel = fortuneWheel;
        }

        public void Unlock() {
            this.data.isUnlocked = true;
            this.repository.Save();
            FortuneWheelAnalytics.LogUnlocked();
            this.OnFortuneWheelUnlockedEvent?.Invoke(this);
        }


        #region CAN I USE A SPIN

        public bool CanUseFreeSpin() {
            if (!this.data.isUnlocked)
                return false;
            
            var timeLastFreeSpin = this.data.lastFreeSpinTimeSerialized.GetDateTime();
            var now = GameTime.now;
            var secondsDifferent = (now - timeLastFreeSpin).TotalSeconds;
            return secondsDifferent >= this.config.freeSpinPeriod;
        }

        public bool CanUseAdSpin() {
            if (!this.data.isUnlocked)
                return false;
            
            var adSpinUsedCount = this.data.adSpinUsedCount;
            var adSpinAvailableCountDefault = this.config.adSpinCount;
            return adSpinUsedCount < adSpinAvailableCountDefault;
        }

        public bool CanUseGemSpin() {
            if (!this.data.isUnlocked)
                return false;
            
            var price = this.data.gemPriceCurrent;
            return Bank.isEnoughtHardCurrency(price);
        }

        #endregion


        #region USE

        public void TryToSpin(UnityAction<bool> callback) {

            if (this.CanUseFreeSpin()) {
                this.data.gemPriceCurrent = this.config.gemPriceSpinDefault;
                this.data.adSpinUsedCount = 0;
                this.UseFreeSpin(callback);
                return;
            }

            if (this.CanUseAdSpin()) {
                this.UseADSpin(callback);
                return;
            }

            if (this.CanUseGemSpin()) {
                this.UseGemSpin(callback);
                return;
            }

            callback?.Invoke(FAIL);
        }

        private void UseFreeSpin(UnityAction<bool> callback)
        {
            this.fortuneWheel.Rotate(isFirstSpin());
            this.fortuneWheel.OnRotateOverEvent += this.FreeSpinUsed;
            callback?.Invoke(SUCCESS);
        }
        
        private void UseADSpin(UnityAction<bool> callback) {
            ADS.ShowRewardedVideo("fortune_wheel", (success, result,  error) => {
                if (success) {
                    this.fortuneWheel.Rotate(isFirstSpin());
                    this.fortuneWheel.OnRotateOverEvent += this.ADSpinUsed;
                }
                callback?.Invoke(success);
            });
        }
        
        private void UseGemSpin(UnityAction<bool> callback) {
            Bank.SpendHardCurrency(this.data.gemPriceCurrent, this);
            this.fortuneWheel.Rotate(isFirstSpin());
            this.fortuneWheel.OnRotateOverEvent += GemSpinUsed;
            callback?.Invoke(SUCCESS);
        }

        #endregion


        #region SPIN USED

        public void FreeSpinUsed(FortuneWheel fortuneWheel) {
            this.fortuneWheel.OnRotateOverEvent -= this.FreeSpinUsed;
            this.data.spinUsedCount++;
            this.UpdateLastFreeSpinTime();
            this.OnFortuneWheelStateChangedEvent?.Invoke(this);
        }

        private void UpdateLastFreeSpinTime() {
            var now = GameTime.now;
            this.data.lastFreeSpinTimeSerialized = new DateTimeSerialized(now);
        }

        public void ADSpinUsed(FortuneWheel fortuneWheel) {
            this.fortuneWheel.OnRotateOverEvent -= this.ADSpinUsed;
            this.data.adSpinUsedCount++;
            this.data.spinUsedCount++;
            this.OnFortuneWheelStateChangedEvent?.Invoke(this);
        }

        public void GemSpinUsed(FortuneWheel fortuneWheel) {
            this.fortuneWheel.OnRotateOverEvent -= this.GemSpinUsed;
            this.data.spinUsedCount++;
            this.data.gemPriceCurrent += this.config.gemPriceStep;
            this.OnFortuneWheelStateChangedEvent?.Invoke(this);
        }

        #endregion

        public EcoClickerTimer GetTimerToNextFreeSpin() {
            var lastSpinTime = this.data.lastFreeSpinTimeSerialized.GetDateTime();
            var now = GameTime.now;
            var totalSeconds = Mathf.Max(this.config.freeSpinPeriod - (int) (now - lastSpinTime).TotalSeconds, 0);
            return new EcoClickerTimer(totalSeconds);
        }

        public int GetTimeToNextFreeSpin() {
            var lastSpinTime = this.data.lastFreeSpinTimeSerialized.GetDateTime();
            var now = GameTime.now;
            var totalSeconds = Mathf.Max(this.config.freeSpinPeriod - (int) (now - lastSpinTime).TotalSeconds, 0);
            return totalSeconds;
        }
        
        
        public void Save() {
            this.repository.Save();
        }

        private bool isFirstSpin()
        {
            return data.spinUsedCount == 0;
        }

    }
}