using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards {
    [CreateAssetMenu(fileName = "RewardInfoCryptoCurrency", menuName = "Meta/Rewards/RewardInfoCryptoCurrency")]
    public class RewardInfoCryptoCurrency : RewardInfoEcoClicker {
        [SerializeField] private float m_value;

        public float value => this.m_value;

        public override RewardHandler CreateRewardHandler(Reward reward) {
            return new RewardHandlerCryptoCurrency(reward);
        }

        public override string GetCountToString() {
            return value.ToString();
        }
    }
}