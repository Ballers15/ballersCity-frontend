using SinSity.Domain.Utils;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "AirDropCleanEnergyRewardInfoBuilder",
        menuName = "Game/AirDrop/New AirDropCleanEnergyRewardInfoBuilder"
    )]
    public sealed class AirDropCleanEnergyRewardInfoBuilder : ScriptableRewardInfoBuilder
    {
        [SerializeField]
        private BigNumber minReward;
        
        [SerializeField]
        private RewardInfoSetupSoftCurrency rewardInfoSoftCurrencyAsset;

        [SerializeField]
        public int secondCoefficent;

        public override RewardInfo Build()
        {
            var idleObjectsInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            var rewardInfo = Instantiate(this.rewardInfoSoftCurrencyAsset);
            var cleanEnergyReward = idleObjectsInteractor.GetFullIncomeByIdleObjectsForTime(this.secondCoefficent);
            if (cleanEnergyReward < this.minReward)
            {
                cleanEnergyReward = new BigNumber(this.minReward);
            }
            
            rewardInfo.Setup(cleanEnergyReward);
            rewardInfo.id = "reward_clean_energy_" + this.secondCoefficent;
            return rewardInfo;
        }
    }
}