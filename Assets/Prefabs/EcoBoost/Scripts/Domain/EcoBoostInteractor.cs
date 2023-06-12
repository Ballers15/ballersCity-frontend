using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using SinSity.Repo;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain {
    public sealed class EcoBoostInteractor : Interactor,
        IIdleObjectExternalMultipliersHolderInteractor,
        ISaveListenerInteractor,
        IModernizationAsyncListenerInteractor,
        SecondTickInteractor.IListener,
        IRewindTimeAsyncListenerInteractor {
        #region Const

        private const float ZERO = 0.0f;

        private const float SECOND = 1.0f;

        private const string ECO_BOOST_CONFIG_PATH = "EcoBoostConfig";

        private const string ECO_BOOST_MULTIPLIER_ID = "eco_boost_multiplier_id";

        #endregion

        #region Event

        public event Action<long> OnEcoBoostRemainingSecondsChangedEvent;

        public event Action OnEcoBoostEnabledEvent;

        public event Action OnEcoBoostDisabledEvent;

        public event Action OnEcoBoostLaunchedEvent;

        public event Action<object> OnEcoBoostUnlockedEvent;

        #endregion

        public EcoBoostConfig config { get; private set; }

        public float remainingTimeSeconds { get; private set; }

        public bool isEcoboostUnlocked { get; private set; }

        public bool isEcoBoostWorking { get; private set; }

        private new EcoBoostRepository repository;
        
        public override bool onCreateInstantly { get; } = true;

        private IdleObjectsInteractor idleObjectsInteractor;

        private bool isEnabled;

        #region Initialize

        protected override void Initialize() {
            base.Initialize();
            this.config = Resources.Load<EcoBoostConfig>(ECO_BOOST_CONFIG_PATH);
            this.repository = this.GetRepository<EcoBoostRepository>();
            this.Setup();
        }

        private void Setup() {
            var ecoBoostStatistics = this.repository.Get();
            this.isEnabled = ecoBoostStatistics.isEnabled;
            this.remainingTimeSeconds = ecoBoostStatistics.remainingTimeSec;
            this.isEcoboostUnlocked = ecoBoostStatistics.isUnlocked;
#if DEBUG
            Debug.Log("IS ECOBOOST UNLOCKED! " + this.isEcoboostUnlocked);
#endif
        }

        #endregion

        #region OnInitialized

        public override void OnInitialized() {
            base.OnInitialized();
            this.idleObjectsInteractor = this.GetInteractor<IdleObjectsInteractor>();
        }


        #endregion

        #region OnReady

        public Dictionary<string, Coefficient> GetStaticMultiplierMapBy(IdleObject idleObject) {
            return new Dictionary<string, Coefficient>();
        }

        public Dictionary<string, TimeCoefficient> GetTimeMultiplierMapBy(IdleObject idleObject) {
            if (!this.isEnabled) {
                return new Dictionary<string, TimeCoefficient>();
            }

            var remainingTimeSeconds = (long) this.remainingTimeSeconds;
            var ecoBoostCoefficient = this.config.idleObjectMultiplier;
            var timeCoefficient = new TimeCoefficient(ecoBoostCoefficient, remainingTimeSeconds);
            return new Dictionary<string, TimeCoefficient> {
                {ECO_BOOST_MULTIPLIER_ID, timeCoefficient}
            };
        }

        #endregion

        public IEnumerator OnRewindTimeAsync(RewindTimeIntent intent) {
            if (!this.isEnabled) {
                yield break;
            }

            if (intent is RewindTimeIntentAsMultiplier) {
                yield break;
            }

            var remainingSeconds = this.remainingTimeSeconds - intent.passedSeconds;
            if (remainingSeconds <= ZERO) {
                this.StopEcoBoost();
            }
            else {
                this.remainingTimeSeconds = remainingSeconds;
                this.isEcoBoostWorking = true;
                this.OnEcoBoostEnabledEvent?.Invoke();
            }
        }

        public IEnumerator OnStartModernizationAsync() {
            this.StopEcoBoost();
            yield break;
        }
        
        public void OnSave() {
            var statistics = new EcoBoostStatistics {
                isUnlocked = this.isEcoboostUnlocked,
                isEnabled = this.isEnabled,
                remainingTimeSec = (int) this.remainingTimeSeconds
            };
            this.repository.Update(statistics);
        }

        public void OnSecondTick() {
            if (!this.isEcoBoostWorking) {
                return;
            }

            this.remainingTimeSeconds -= SECOND;
            var seconds = (long) this.remainingTimeSeconds;
            this.OnEcoBoostRemainingSecondsChangedEvent?.Invoke(seconds);
            if (this.remainingTimeSeconds <= 0) {
                this.StopEcoBoost();
            }
        }

        #region StopEcoBoost

        private void StopEcoBoost() {
            Debug.Log("ECO BOOST STOP!");
            this.remainingTimeSeconds = 0.0f;
            this.isEnabled = false;
            this.isEcoBoostWorking = false;
            this.RemoveIncomeMultiplierForIdleObjects();
            this.OnSave();
            this.OnEcoBoostDisabledEvent?.Invoke();
        }

        private void RemoveIncomeMultiplierForIdleObjects() {
            foreach (var idleObject in this.idleObjectsInteractor.GetIdleObjects()) {
                var idleObjectState = idleObject.state;
                var incomeTimeMultiplicator = idleObjectState.incomeTimeMultiplicator;
                incomeTimeMultiplicator.RemoveCoefficient(ECO_BOOST_MULTIPLIER_ID);
                idleObject.NotifyAboutStateChanged();
            }
        }

        #endregion

        #region LaunchEcoBoost

        public void LaunchEcoBoost() {
            if (!this.isEcoboostUnlocked) {
                throw new Exception("Eco boost is locked!");
            }

            var newRemainingTime = this.remainingTimeSeconds + this.config.durationSeconds;
            var limitDurationTime = this.config.limitDurationTime;
            this.remainingTimeSeconds = Math.Min(newRemainingTime, limitDurationTime);
            this.isEnabled = true;
            this.isEcoBoostWorking = true;
            this.OnSave();
            this.ProvideIncomeMultiplierForIdleObjects();
            this.OnEcoBoostEnabledEvent?.Invoke();
            this.OnEcoBoostLaunchedEvent?.Invoke();
            CommonAnalytics.LogXtraManagementTabStarted();
        }

        private void ProvideIncomeMultiplierForIdleObjects() {
            foreach (var idleObject in this.idleObjectsInteractor.GetIdleObjects()) {
                var idleObjectState = idleObject.state;
                var incomeConstMultiplicator = idleObjectState.incomeTimeMultiplicator;
                var ecoBoostCoefficient = this.config.idleObjectMultiplier;
                var timeCoefficient = new TimeCoefficient(ecoBoostCoefficient, (int) this.remainingTimeSeconds);
                incomeConstMultiplicator.SetCoefficient(ECO_BOOST_MULTIPLIER_ID, timeCoefficient);
                idleObject.NotifyAboutStateChanged();
            }
        }

        #endregion

        public void UnlockEcoBoost(object sender) {
            this.isEcoboostUnlocked = true;
            this.OnSave();
            this.OnEcoBoostUnlockedEvent?.Invoke(sender);
        }
    }
}