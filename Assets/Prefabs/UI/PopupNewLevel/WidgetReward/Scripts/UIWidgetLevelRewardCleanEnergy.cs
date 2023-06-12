using SinSity.Meta.Rewards;
using VavilichevGD.Meta.Rewards;

namespace SinSity.UI {
    public class UIWidgetLevelRewardCleanEnergy : UIWidgetLevelReward {
        protected override void MakeFX(Reward reward) {
            RewardInfoSoftCurrency rewardInfo = reward.GetRewardInfo<RewardInfoSoftCurrency>();
            UIObjectEcoClicker objectEcoClicker = new UIObjectEcoClicker(this.transform.position);
            FXCleanEnergyGenerator.MakeFXSlow(objectEcoClicker, rewardInfo.value);
        }
    }
}