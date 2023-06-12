using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;

namespace SinSity.Meta.Rewards {
    public class RewardHandlerCryptoCurrency : RewardHandler {
        public RewardHandlerCryptoCurrency(Reward reward) : base(reward) {
        }

        public override void ApplyReward(object sender, bool instantly) {
            var infoHardCurrency = reward.info as RewardInfoSetupCryptoCurrency;
            if (infoHardCurrency) {
                ReceiveReward(infoHardCurrency.value, sender, instantly);
                return;
            }
            
            var info = reward.info as RewardInfoCryptoCurrency;
            ReceiveReward(info.value, sender, instantly);
        }

        private void ReceiveReward(float count, object sender, bool instantly) {
            Bank.AddCryptoCurrency(count, sender);
            if (instantly)
                Bank.uiBank.AddCryptoCurrency(sender, count);
            reward.NotifyAboutRewardReceived(SUCCESS);
        }
    }
}