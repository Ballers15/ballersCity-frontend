using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core {
    [CreateAssetMenu(
        fileName = "GemTreeUnlockConfig",
        menuName = "Game/GemTree/New GemTreeUnlockConfig"
    )]
    public sealed class GemTreeUnlockConfig : ProfileLevelUnlockWithActivityConfig {
        [SerializeField] private RewardInfo[] rewardInfoSet;

        protected override IEnumerable<Reward> GenerateRewards() {
            var rewards = new List<Reward>();

            foreach (var rewardInfo in this.rewardInfoSet)
                rewards.Add(new Reward(rewardInfo));

            return rewards;
        }

    }
}