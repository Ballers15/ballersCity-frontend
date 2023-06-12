using System.Collections.Generic;
using SinSity.Domain.Utils;
using SinSity.Meta.Rewards;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "EcoBoostUnlockConfig",
        menuName = "Game/EcoBoost/New EcoBoostUnlockConfig"
    )]
    public sealed class EcoBoostUnlockConfig : ProfileLevelUnlockWithActivityConfig
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