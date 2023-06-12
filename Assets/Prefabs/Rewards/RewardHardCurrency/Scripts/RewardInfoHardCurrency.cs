using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards {
    [CreateAssetMenu(fileName = "RewardInfoHardCurrency", menuName = "Meta/Rewards/RewardHardCurrency")]
    public sealed class RewardInfoHardCurrency : RewardInfoEcoClicker {

        [SerializeField] private int m_value;

        public int value => this.m_value;

        public override RewardHandler CreateRewardHandler(Reward reward) {
            return new RewardHandlerHardCurrency(reward);
        }

        public override string GetCountToString() {
            return this.value.ToString();
        }

    }
}