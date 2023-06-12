using UnityEngine;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;

namespace SinSity.Meta.Rewards
{
    public class RewardHandlerHardCurrency : RewardHandler
    {
        public RewardHandlerHardCurrency(Reward reward) : base(reward)
        {
        }

        public override void ApplyReward(object sender, bool instantly)
        {
            RewardInfoSetupHardCurrency infoHardCurrency = reward.info as RewardInfoSetupHardCurrency;
            if (infoHardCurrency) {
                ReceiveReward(infoHardCurrency.value, sender, instantly);
                return;
            }
            
            RewardInfoHardCurrency info = reward.info as RewardInfoHardCurrency;
            ReceiveReward(info.value, sender, instantly);
        }

        private void ReceiveReward(int count, object sender, bool instantly) {
            Bank.AddHardCurrency(count, sender);
            if (instantly)
                Bank.uiBank.AddHardCurrency(sender, count);
            reward.NotifyAboutRewardReceived(SUCCESS);
        }
    }
}