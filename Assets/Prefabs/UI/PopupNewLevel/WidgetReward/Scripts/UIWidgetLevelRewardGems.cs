using SinSity.Meta.Rewards;
using VavilichevGD.Meta.Rewards;

namespace SinSity.UI
{
    public class UIWidgetLevelRewardGems : UIWidgetLevelReward
    {
        protected override void MakeFX(Reward reward)
        {
            var rewardInfo = reward.GetRewardInfo<RewardInfo>();
            var objectEcoClicker = new UIObjectEcoClicker(this.transform.position);
            if (rewardInfo is RewardInfoSetupHardCurrency rewardInfoSetupHardCurrency)
            {
                FXGemsGenerator.MakeFX(objectEcoClicker, rewardInfoSetupHardCurrency.value);
            }
            else if (rewardInfo is RewardInfoHardCurrency rewardInfoHardCurrency)
            {
                FXGemsGenerator.MakeFX(objectEcoClicker, rewardInfoHardCurrency.value);
            }
        }
    }
}