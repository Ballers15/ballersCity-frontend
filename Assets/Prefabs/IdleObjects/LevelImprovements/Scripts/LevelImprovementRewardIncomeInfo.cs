using System;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta {
    [Serializable]
    [CreateAssetMenu(fileName = "LevelImprovementRewardIncome", menuName = "IdleObject/LevelImprovementReward/Income")]
    public class LevelImprovementRewardIncomeInfo : RewardInfo {

        [Range(1, 100)]
        public int incomeBoost = 1;
        
        public override RewardHandler CreateRewardHandler(Reward reward) {
            LevelImprovementReward levelImprovementReward = reward as LevelImprovementReward;
            return  new LevelImprovementRewardIncomeHandler(levelImprovementReward);
        }

        public override string GetCountToString() {
            return "1";
        }
    }
}