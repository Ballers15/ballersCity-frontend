using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SinSity.Tools;
using SinSity.UI;
using Orego.Util;
using SinSity.Core;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;

namespace SinSity.Domain {
    public sealed class GemTreeTimerInteractor : Interactor,
        SecondTickInteractor.IListener,
        ISaveListenerInteractor,
        IRewindTimeAsyncListenerInteractor,
        IBankStateWithoutNotification {
        #region Const

        private const int SECONDS_IN_MINUTE = 60;

        private const int SECOND = 1;

        private const int ZERO = 0;

        #endregion

        #region Event

        public event Action<object, GemBranchObject, GemBranchReward> OnBranchObjectRewardReceivedEvent;

        public event Action<object, GemBranchObject> OnBranchObjectStartedEvent;

        public event Action<object, GemBranchObject> OnBranchObjectRemainingSecondsChangedEvent;

        public event Action<object, GemBranchObject> OnBranchObjectReadyEvent;

        #endregion

        private new GemTreeRepository repository;

        private GemTreeBranchDataInteractor branchDataInteractor;

        private GemTreeStateInteractor treeStateInteractor;

        private readonly HashSet<GemBranchObject> processingBranchObjects;

        public override bool onCreateInstantly { get; } = true;

        #region Initialize

        public GemTreeTimerInteractor() {
            this.processingBranchObjects = new HashSet<GemBranchObject>();
        }

        protected override void Initialize() {
            base.Initialize();
            this.repository = this.GetRepository<GemTreeRepository>();
        }


        public override void OnInitialized() {
            this.branchDataInteractor = this.GetInteractor<GemTreeBranchDataInteractor>();
            this.treeStateInteractor = this.GetInteractor<GemTreeStateInteractor>();
        }

        public override void OnReady() {
            this.treeStateInteractor.OnBranchObjectOpenedEvent += this.OnBranchObjectOpened;
            var processingBranchObjects = this.branchDataInteractor
                .GetBranchObjects()
                .Where(it => {
                    var state = it.state;
                    return state.isOpened && !state.isReady;
                });

            this.processingBranchObjects.AddRange(processingBranchObjects);
        }

        #endregion

        #region CollectGem

        public bool CanCollectGem(GemBranchObject branchObject) {
            var state = branchObject.state;
            return state.isOpened && state.isReady;
        }

        public void CollectGem(object sender, GemBranchObject branchObject) {
            this.ReceiveReward(sender, branchObject);
            this.StartBranch(sender, branchObject);
        }

        private void ReceiveReward(object sender, GemBranchObject branchObject) {
            if (!this.CanCollectGem(branchObject))
                throw new Exception("Can not collect gem");

            var state = branchObject.state;
            state.isReady = false;
            state.remainingTimeSec = ZERO;
            var branchData = branchObject.ToData();
            this.repository.UpdateBranchData(branchData);
            var branchObjectInfo = branchObject.info;
            var reward = branchObjectInfo.GenerateGemRewardCount();
            Bank.AddHardCurrency(reward.gemsReward, this);
            this.OnBranchObjectRewardReceivedEvent?.Invoke(sender, branchObject, reward);
        }

        private void StartBranch(object sender, GemBranchObject branchObject) {
            var randomMinutes = branchObject.info.GetRandomMinutes();
            var randomWaitSeconds = SECONDS_IN_MINUTE * randomMinutes;
            branchObject.state.remainingTimeSec = randomWaitSeconds; //TEMP VALUE:
            this.processingBranchObjects.Add(branchObject);
            var researchData = branchObject.ToData();
            this.repository.UpdateBranchData(researchData);
            this.branchDataInteractor.NotifyAboutBranchObjectChanged(sender, branchObject);
            this.OnBranchObjectStartedEvent?.Invoke(sender, branchObject);
        }

        #endregion

        #region OnSecondTick

        public void OnSecondTick() {
            if (this.processingBranchObjects.IsEmpty()) {
                return;
            }

            var processingBranchObjects = this.processingBranchObjects.ToList();
            foreach (var branchObject in processingBranchObjects) {
                this.ProcessBranchObjectTime(branchObject);
            }
        }

        private void ProcessBranchObjectTime(GemBranchObject branchObject) {
            var state = branchObject.state;
            state.remainingTimeSec -= SECOND;
            this.branchDataInteractor.NotifyAboutBranchObjectChanged(this, branchObject);
            this.OnBranchObjectRemainingSecondsChangedEvent?.Invoke(this, branchObject);
            if (state.remainingTimeSec > ZERO) {
                return;
            }

            this.processingBranchObjects.Remove(branchObject);
            this.SetReadyBranch(this, branchObject);
        }

        #endregion

        private void SetReadyBranch(object sender, GemBranchObject branchObject) {
            var state = branchObject.state;
            state.isReady = true;
            state.remainingTimeSec = ZERO;
            var branchData = branchObject.ToData();
            this.repository.UpdateBranchData(branchData);

            if (sender is GemTreeTimerInteractor)
                this.OnBranchObjectReadyEvent?.Invoke(this, branchObject);
        }

        #region OnSave

        public void OnSave() {
            var processingBranchObjects = this.processingBranchObjects
                .ToList()
                .Select(it => it.ToData());
            this.repository.UpdateBranchDataSet(processingBranchObjects);
        }

        #endregion

        #region OnRewindTime

        public IEnumerator OnRewindTimeAsync(RewindTimeIntent intent) {
            if (this.processingBranchObjects.IsEmpty()) {
                yield break;
            }

            if (intent is RewindTimeIntentAsMultiplier) {
                yield break;
            }

            var passedOfflineSeconds = intent.passedSeconds;
            var processingBranchObjects = this.processingBranchObjects.ToList();
            var branchDataSet = new List<GemBranchData>();
            foreach (var branchObject in processingBranchObjects) {
                var state = branchObject.state;
                state.remainingTimeSec -= passedOfflineSeconds;
                if (state.remainingTimeSec <= ZERO) {
                    state.isReady = true;
                    state.remainingTimeSec = ZERO;
                    this.processingBranchObjects.Remove(branchObject);
                    this.OnBranchObjectReadyEvent?.Invoke(this, branchObject);
                }

                var branchData = branchObject.ToData();
                branchDataSet.Add(branchData);
            }

            this.repository.UpdateBranchDataSet(branchDataSet);
        }

        #endregion

        #region InteractorEvents

        private void OnBranchObjectOpened(object sender, GemBranchObject branchObject) {
            this.SetReadyBranch(sender, branchObject);
        }

        #endregion
    }
}