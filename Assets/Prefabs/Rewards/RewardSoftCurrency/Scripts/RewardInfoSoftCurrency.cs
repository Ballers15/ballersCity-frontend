using SinSity.Tools;
using UnityEngine;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;

namespace SinSity.Meta.Rewards
{
    [CreateAssetMenu(fileName = "RewardInfoSoftCurrency", menuName = "Meta/Rewards/RewardInfoSoftCurrency")]
    public sealed class RewardInfoSoftCurrency : RewardInfoEcoClicker
    {
        [SerializeField]
        private BigNumber m_value;

        public BigNumber value
        {
            get { return this.m_value; }
        }

        public override RewardHandler CreateRewardHandler(Reward reward)
        {
            return new RewardHandlerSoftCurrency(reward);
        }

        public override string GetCountToString() {
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            var numberLocalized = value.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            return numberLocalized;
        }

    }
}