using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    public sealed class ComplexResearchRewardHandler : RewardHandler
    {
        public ComplexResearchRewardHandler(Reward reward) : base(reward)
        {
        }

        public override void ApplyReward(object sender, bool instantly)
        {
            var rewardInfo = this.reward.GetRewardInfo<ComplexResearchRewardInfo>();
            var innerRewardInfoSet = rewardInfo.innerRewardInfoSet;
            foreach (var info in innerRewardInfoSet)
            {
                var reward = new Reward(info);
                reward.Apply(sender, instantly);
            }
        }
    }
}