using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;

namespace SinSity.Meta.Rewards
{
    public class RewardHandlerSoftCurrency : RewardHandler
    {
        public RewardHandlerSoftCurrency(Reward reward) : base(reward)
        {
        }

        public override void ApplyReward(object sender, bool instantly)
        {
            RewardInfoSetupSoftCurrency infoSoftCurrency = reward.info as RewardInfoSetupSoftCurrency;
            if (instantly)
                Bank.uiBank.AddSoftCurrency(sender, infoSoftCurrency.value);
            Bank.AddSoftCurrency(infoSoftCurrency.value, sender);
            reward.NotifyAboutRewardReceived(SUCCESS);
        }
    }
}