using System;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "ComplexResearchRewardInfo",
        menuName = "Game/Research/New ComplexResearchRewardInfo"
    )]
    public sealed class ComplexResearchRewardInfo : RewardInfoEcoClicker
    {
        [SerializeField]
        public RewardInfoEcoClicker[] innerRewardInfoSet;
        
        public override RewardHandler CreateRewardHandler(Reward reward)
        {
            return new ComplexResearchRewardHandler(reward);
        }

        public override string GetCountToString() {
            throw new NotSupportedException();
        }

    }
}