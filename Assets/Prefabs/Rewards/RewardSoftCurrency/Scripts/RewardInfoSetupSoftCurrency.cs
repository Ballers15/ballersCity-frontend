using SinSity.Tools;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;

namespace SinSity.Meta.Rewards
{
    [CreateAssetMenu(fileName = "RewardInfoSetupSoftCurrency", menuName = "Meta/Rewards/RewardInfoSetupSoftCurrency")]
    public class RewardInfoSetupSoftCurrency : RewardInfoEcoClicker
    {
        public BigNumber value { get; private set; }
        
        public void Setup(BigNumber value)
        {
            this.value = value;
        }
        
        public override RewardHandler CreateRewardHandler(Reward reward)
        {
            return new RewardHandlerSoftCurrency(reward);
        }

        public override string GetCountToString() {
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            var numberLocalized = this.value.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            return numberLocalized;
        }

        public override string GetDescription() {
            return Localization.GetTranslation("ID_CLEAN_ENERGY");
        }
    }
}