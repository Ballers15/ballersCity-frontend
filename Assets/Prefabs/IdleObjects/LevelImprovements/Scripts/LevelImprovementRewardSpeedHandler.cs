using SinSity.Core;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta
{
    public class LevelImprovementRewardSpeedHandler : RewardHandler
    {
        private LevelImprovementReward _levelImprovementReward;

        public LevelImprovementRewardSpeedHandler(LevelImprovementReward reward) : base(reward)
        {
            _levelImprovementReward = reward;
        }

        public override void ApplyReward(object sender, bool instantly)
        {
            LevelImprovementRewardSpeedInfo info = _levelImprovementReward.info as LevelImprovementRewardSpeedInfo;
            IdleObject idleObject = _levelImprovementReward.idleObject;
            idleObject.IncreaseSpeed(info.speedBoost);
        }
    }
}