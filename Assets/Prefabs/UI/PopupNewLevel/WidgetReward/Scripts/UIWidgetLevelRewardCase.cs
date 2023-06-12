using SinSity.Meta.Rewards;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;

namespace SinSity.UI {
    public class UIWidgetLevelRewardCase : UIWidgetLevelReward {
        protected override void MakeFX(Reward reward) {
            RewardInfoCase rewardInfo = reward.GetRewardInfo<RewardInfoCase>();
            Product productCase = Shop.GetProduct(rewardInfo.caseInfo.GetId());
            UIObjectEcoClicker objectEcoClicker = new UIObjectEcoClicker(this.transform.position);
            FXCasesGenerator.MakeFX(objectEcoClicker, productCase, rewardInfo.count);
        }
    }
}