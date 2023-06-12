using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards
{
    [CreateAssetMenu(fileName = "RewardInfoCase", menuName = "Meta/Rewards/RewardCase")]
    public sealed class RewardInfoCase : RewardInfoEcoClicker
    {
        [SerializeField] private ProductInfoCase m_caseInfo;
        [SerializeField] private int m_count = 1;

        public int count => m_count;

        public ProductInfoCase caseInfo
        {
            get { return this.m_caseInfo; }
        }

        public override RewardHandler CreateRewardHandler(Reward reward)
        {
            return new RewardHandlerCase(reward);
        }

        public override string GetCountToString() {
            return count.ToString();
        }
    }
}