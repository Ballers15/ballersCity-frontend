namespace VavilichevGD.Meta.Rewards {
    public abstract class RewardHandler {
        protected readonly Reward reward;

        protected const bool SUCCESS = true;
        protected const bool FAIL = false;

        protected RewardHandler(Reward reward) {
            this.reward = reward;
        }

        public abstract void ApplyReward(object sender, bool instantly);

        protected virtual void Fail() {
            reward.NotifyAboutRewardReceived(FAIL);
        }

        protected virtual void Success() {
            reward.NotifyAboutRewardReceived(SUCCESS);
        }
    }
}