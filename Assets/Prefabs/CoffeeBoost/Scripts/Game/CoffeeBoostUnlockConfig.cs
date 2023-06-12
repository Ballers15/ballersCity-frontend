using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core {
    [CreateAssetMenu(
        fileName = "CoffeeBoostUnlockConfig",
        menuName = "Game/CoffeeBoost/New CoffeeBoostUnlockConfig"
    )]
    public class CoffeeBoostUnlockConfig : ProfileLevelUnlockWithActivityConfig {

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