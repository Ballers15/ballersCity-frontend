using SinSity.Core;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta
{
    public sealed class LevelImprovementReward : Reward
    {
        public IdleObject idleObject { get; private set; }

        public LevelImprovementReward(RewardInfo info) : base(info)
        {
        }

        public void Apply(IdleObject idleObject)
        {
            this.idleObject = idleObject;
            this.Apply(this, false);
        }
    }
}