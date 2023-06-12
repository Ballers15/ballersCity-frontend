using System.Collections.Generic;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "AirDropUnlockConfig",
        menuName = "Game/AirDrop/New AirDropUnlockConfig"
    )]
    public sealed class AirDropUnlockConfig : ProfileLevelUnlockWithActivityConfig
    {
        [SerializeField] private RewardInfo rewardInfo;

        protected override IEnumerable<Reward> GenerateRewards()
        {
            var rewards = new List<Reward>
            {
                new Reward(this.rewardInfo),
            };
            return rewards;
        }
        
    }
}