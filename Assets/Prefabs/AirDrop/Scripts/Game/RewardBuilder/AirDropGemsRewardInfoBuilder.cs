using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "AirDropGemsRewardInfoBuilder",
        menuName = "Game/AirDrop/New AirDropGemsRewardInfoBuilder"
    )]
    public sealed class AirDropGemsRewardInfoBuilder : ScriptableRewardInfoBuilder
    {
        [SerializeField]
        private RewardInfoSetupHardCurrency rewardInfoHardCurrencyAsset;

        [SerializeField]
        private int gemsCount;
        
        public override RewardInfo Build()
        {
            var rewardInfo = Instantiate(this.rewardInfoHardCurrencyAsset);
            rewardInfo.Setup(this.gemsCount);
            rewardInfo.id = "reward_gems_" + this.gemsCount;
            return rewardInfo;
        }
    }
}