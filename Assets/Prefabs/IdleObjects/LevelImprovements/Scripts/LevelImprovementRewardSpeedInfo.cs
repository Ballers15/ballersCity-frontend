using System;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta {
    [Serializable]
    [CreateAssetMenu(fileName = "LevelImprovementRewardSpeed", menuName = "IdleObject/LevelImprovementReward/Speed")]
    public class LevelImprovementRewardSpeedInfo : RewardInfo {

        [Range(1f, 3f)]
        public float speedBoost = 1f;
        
        public override RewardHandler CreateRewardHandler(Reward reward) {
            LevelImprovementReward levelImprovementReward = reward as LevelImprovementReward;
            return new LevelImprovementRewardSpeedHandler(levelImprovementReward);
        }

        public override string GetCountToString() {
            return "1";
        }
    }
}