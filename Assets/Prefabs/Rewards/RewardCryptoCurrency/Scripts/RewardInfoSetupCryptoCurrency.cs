using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards {
    [CreateAssetMenu(fileName = "RewardInfoSetupCryptoCurrency", menuName = "Meta/Rewards/RewardInfoSetupCryptoCurrency")]
    public class RewardInfoSetupCryptoCurrency : RewardInfoEcoClicker {
        public float value { get; private set; }

        public void Setup(float value) {
            this.value = value;
        }

        public override RewardHandler CreateRewardHandler(Reward reward) {
            return new RewardHandlerCryptoCurrency(reward);
        }

        public override string GetCountToString() {
            return value.ToString();
        }

        public override string GetDescription() {
            return Localization.GetTranslation("ID_GEMS");
        }
    }
}