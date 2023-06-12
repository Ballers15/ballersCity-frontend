using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta {
    [CreateAssetMenu(fileName = "LevelImprovementRewardAutoPlay", menuName = "IdleObject/LevelImprovementReward/Autoplay")]
    public class LevelImprovementRewardAutoplayInfo : RewardInfo {
        public override RewardHandler CreateRewardHandler(Reward reward) {
            LevelImprovementReward levelImprovementReward = reward as LevelImprovementReward;
            return new LevelImprovementRewardAutoplayHandler(levelImprovementReward);
        }

        public override string GetCountToString() {
            return "1";
        }
    }
}