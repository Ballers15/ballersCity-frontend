using SinSity.Meta.Rewards;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;

namespace SinSity.UI {
    public class UIWidgetLevelRewardTimeBooster : UIWidgetLevelReward {
        protected override void MakeFX(Reward reward) {
            RewardInfoTimeBooster rewardInfo = reward.GetRewardInfo<RewardInfoTimeBooster>();
            Product productTimeBooster = Shop.GetProduct(rewardInfo.timeBoosterInfo.GetId());
            UIObjectEcoClicker objectEcoClicker = new UIObjectEcoClicker(this.transform.position);
            FXTimeBoosterGenerator.MakeFX(objectEcoClicker, productTimeBooster, rewardInfo.count);
        }
    }
}