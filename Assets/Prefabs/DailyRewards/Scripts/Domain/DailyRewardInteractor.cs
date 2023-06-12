using System;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.Meta.Rewards {
    public sealed class DailyRewardInteractor : Interactor {
        #region Const

        private const string RESOURCES_PATH = "DailyRewardsPipeline";

        public const int TIME_DIFF_BETWEEN_REWARDS_HOURS = 24;

        #endregion

        #region Event

        public event Action<Reward> OnDailyBonusReceivedEvent;

        public event Action OnDailyBonusCalendarFinishedEvent;

        #endregion

        public DailyRewardsPipeline dailyRewardsPipeline { get; private set; }

        private new DailyRewardRepository repository;

        private DailyRewardsData lastDailyRewardsData;

        private Reward processingReward;

        public override bool onCreateInstantly { get; } = true;

        public int day => this.lastDailyRewardsData.dailyRewardsDayIndex + 1;

        public DateTime lastRewardDateTime => lastDailyRewardsData.dailyRewardReceivedTime;
        public bool lastRewardWasReceived => lastRewardDateTime != new DateTime();

        #region InitializeRoutine

        protected override void Initialize() {
            base.Initialize();
            var asset = Resources.Load<DailyRewardsPipeline>(RESOURCES_PATH);
            this.dailyRewardsPipeline = ScriptableObject.Instantiate(asset);
            this.repository = this.GetRepository<DailyRewardRepository>();
            this.InitializeState();
        }

        private void InitializeState() {
            var lastDailyRewardData = this.repository.GetLastDailyRewardData();

            if (!lastDailyRewardData.isCompleted) {
                Reward.OnRewardReceived += this.OnRewardReceived;
            }

            this.lastDailyRewardsData = lastDailyRewardData;
        }

        #endregion

        public bool IsCalendarFinished() {
            return this.lastDailyRewardsData.isCompleted;
        }

        public bool CanReceiveReward() {
            if (this.IsCalendarFinished()) {
                return false;
            }

            if (!GameTime.isInitialized) {
                return false;
            }

            var timeDifference = (GameTime.now - this.lastDailyRewardsData.dailyRewardReceivedTime).TotalHours;
            return timeDifference >= TIME_DIFF_BETWEEN_REWARDS_HOURS;
        }

        public void ReceiveReward() {
            if (!this.CanReceiveReward()) {
                return;
            }

            var currentDayIndex = this.lastDailyRewardsData.dailyRewardsDayIndex;
            var currentRewardInfo = this.dailyRewardsPipeline.GetReward(currentDayIndex);
            this.processingReward = new Reward(currentRewardInfo);
            this.processingReward.Apply(this, true);
        }

        private void OnRewardReceived(Reward reward, bool success) {
            if (this.processingReward != reward) {
                return;
            }

            if (!success) {
                return;
            }

            this.processingReward = null;
            this.OnDailyBonusReceivedEvent?.Invoke(reward);
            this.TrySetNextReward();
        }

        private void TrySetNextReward() {
            var currentDayIndex = this.lastDailyRewardsData.dailyRewardsDayIndex;
            if (!this.dailyRewardsPipeline.HasNextReward(currentDayIndex, out _)) {
                this.lastDailyRewardsData.isCompleted = true;
                this.repository.SetLastDailyRewardData(this.lastDailyRewardsData);
                this.OnDailyBonusCalendarFinishedEvent?.Invoke();
                return;
            }

            this.lastDailyRewardsData.dailyRewardsDayIndex++;
            this.lastDailyRewardsData.dailyRewardReceivedTimeSerialized = new DateTimeSerialized(GameTime.now);
            this.repository.SetLastDailyRewardData(this.lastDailyRewardsData);
        }
    }
}