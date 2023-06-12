using System.Collections.Generic;
using SinSity.Domain;
using SinSity.Domain.Utils;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "ResearchUnlockConfig",
        menuName = "Game/Research/New ResearchUnlockConfig"
    )]
    public sealed class ResearchUnlockConfig : ProfileLevelUnlockWithActivityConfig
    {
        [SerializeField] private RewardInfo[] rewardInfoSet;

        protected override IEnumerable<Reward> GenerateRewards()
        {
            var rewards = new List<Reward>();

            foreach (var rewardInfo in this.rewardInfoSet)
                rewards.Add(new Reward(rewardInfo));
            
            return rewards;
        }
    }
}