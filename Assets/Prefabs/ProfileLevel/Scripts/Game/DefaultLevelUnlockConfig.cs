using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "DefaultLevelUnlockConfig",
        menuName = "Game/ProfileLevel/New DefaultLevelUnlockConfig"
    )]
    public class DefaultLevelUnlockConfig : ProfileLevelUnlockConfig
    {
        [SerializeField]
        protected RewardInfo[] rewardInfoSet;

        protected override IEnumerable<Reward> GenerateRewards()
        {
            var rewards = new List<Reward>();
            foreach (var rewardInfo in this.rewardInfoSet)
            {
                rewards.Add(new Reward(rewardInfo));
            }

            return rewards;
        }
    }
}