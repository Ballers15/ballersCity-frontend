using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orego.Util;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;

namespace SinSity.Domain
{
    public sealed class ResearchObjectTimerInteractor : Interactor,
        SecondTickInteractor.IListener,
        ISaveListenerInteractor, 
        IRewindTimeAsyncListenerInteractor
    {
        #region Const

        private const int SECOND = 1;

        private const int ZERO = 0;

        #endregion

        #region Event

        public event Action<ResearchObject> OnResearchObjectLaunchedEvent;

        public event Action<ResearchObject> OnResearchObjectFinishedEvent;

        public event Action<ResearchObject> OnResearchObjectRemainingSecondsChangedEvent;

        #endregion

        private new ResearchDataRepository repository;

        private ResearchObjectDataInteractor dataInteractor;

        private HashSet<ResearchObject> processingResearchObjects;

        public override bool onCreateInstantly { get; } = true;

        #region Initialize

        protected override void Initialize() {
            base.Initialize();
            this.repository = this.GetRepository<ResearchDataRepository>();
        }

        public override void OnInitialized()
        {
            this.dataInteractor = this.GetInteractor<ResearchObjectDataInteractor>();
        }

        public override void OnReady() {
            this.processingResearchObjects = this.dataInteractor
                .GetProcessingResearchObjects()
                .ToSet();
        }


        #endregion

        #region LaunchResearch

        public bool CanLaunchResearch(ResearchObject researchObject)
        {
            var state = researchObject.state;
            return !this.processingResearchObjects.Contains(researchObject) &&
                   !state.isEnabled &&
                   !state.isRewardReady &&
                   Bank.softCurrencyCount >= state.price;
        }

        public void LaunchResearchObject(ResearchObject researchObject)
        {
            if (!this.CanLaunchResearch(researchObject))
            {
                throw new Exception("Can not launch research!");
            }

            var state = researchObject.state;
            Bank.SpendSoftCurrency(state.price, this);
            state.isEnabled = true;
            state.remainingTimeSec = researchObject.info.durationSeconds;
            this.processingResearchObjects.Add(researchObject);
            var researchData = researchObject.ToData();
            this.repository.UpdateData(researchData);
            this.dataInteractor.NotifyAboutDataChanged(researchObject);
            this.OnResearchObjectLaunchedEvent?.Invoke(researchObject);
        }

        public void StopResearchObject(ResearchObject researchObject) {
            if (CanNotStopResearch(researchObject)) {
                throw new Exception("Can not stop research!");
            }
            
            var state = researchObject.state;
            state.isEnabled = false;
            processingResearchObjects.Remove(researchObject);
            var researchData = researchObject.ToData();
            repository.UpdateData(researchData);
            dataInteractor.NotifyAboutDataChanged(researchObject);
        }

        public bool CanNotStopResearch(ResearchObject researchObject) {
            var state = researchObject.state;
            return !processingResearchObjects.Contains(researchObject) || !state.isEnabled;
        }

        #endregion

        #region OnSecondTick

        public void OnSecondTick()
        {
            if (this.processingResearchObjects.IsEmpty())
            {
                return;
            }

            var processingResearchObjects = this.processingResearchObjects.ToList();
            foreach (var researchObject in processingResearchObjects)
            {
                this.ProcessResearchObjectTime(researchObject);
            }
        }

        private void ProcessResearchObjectTime(ResearchObject researchObject)
        {
            var state = researchObject.state;
            state.remainingTimeSec -= SECOND;
            this.dataInteractor.NotifyAboutDataChanged(researchObject);
            this.OnResearchObjectRemainingSecondsChangedEvent?.Invoke(researchObject);
            if (state.remainingTimeSec > ZERO)
            {
                return;
            }

            state.isRewardReady = true;
            state.isEnabled = false;
            state.remainingTimeSec = ZERO;
            this.processingResearchObjects.Remove(researchObject);
            var researchData = researchObject.ToData();
            this.repository.UpdateData(researchData);
            this.OnResearchObjectFinishedEvent?.Invoke(researchObject);
        }

        #endregion

        #region OnSave

        public void OnSave()
        {
            var researchDataSet = this.processingResearchObjects
                .ToList()
                .Select(it => it.ToData());
            this.repository.UpdateDataSet(researchDataSet);
        }

        #endregion

        #region OnRewindTime

        public IEnumerator OnRewindTimeAsync(RewindTimeIntent intent)
        {
            if (this.processingResearchObjects.IsEmpty())
            {
                yield break;
            }

            // Exclude time booster effect.
//            if (intent is RewindTimeIntentTimeBooster)
//            {
//                yield break;
//            }

            var passedOfflineSeconds = intent.passedSeconds;
            var processingResearchObjects = this.processingResearchObjects.ToList();
            var researchDataSet = new List<ResearchData>();
            foreach (var researchObject in processingResearchObjects)
            {
                var state = researchObject.state;
                state.remainingTimeSec -= passedOfflineSeconds;
                if (state.remainingTimeSec <= ZERO)
                {
                    state.isRewardReady = true;
                    state.isEnabled = false;
                    state.remainingTimeSec = ZERO;
                    this.processingResearchObjects.Remove(researchObject);
                    this.OnResearchObjectFinishedEvent?.Invoke(researchObject);
                }

                var researchData = researchObject.ToData();
                researchDataSet.Add(researchData);
            }

            this.repository.UpdateDataSet(researchDataSet);
        }

        #endregion
    }
}