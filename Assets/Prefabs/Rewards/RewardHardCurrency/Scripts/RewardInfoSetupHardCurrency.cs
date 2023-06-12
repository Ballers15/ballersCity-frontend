using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards
{
    [CreateAssetMenu(fileName = "RewardSetupInfoHardCurrency", menuName = "Meta/Rewards/RewardSetupHardCurrency")]
    public sealed class RewardInfoSetupHardCurrency : RewardInfoEcoClicker
    {
        public int value { get; private set; }

        public void Setup(int value)
        {
            this.value = value;
        }

        public override RewardHandler CreateRewardHandler(Reward reward)
        {
            return new RewardHandlerHardCurrency(reward);
        }

        public override string GetCountToString() {
            return this.value.ToString();
        }

        public override string GetDescription() {
            return Localization.GetTranslation("ID_GEMS");
        }
    }
}