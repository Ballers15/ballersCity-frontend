using SinSity.Monetization;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Meta.Rewards
{
    public class RewardHandlerTimeBooster : RewardHandler {
        public delegate void TimeBoosterRewardHandler(Reward reward);
        public static event TimeBoosterRewardHandler OnTimeBoosterRewarded;
        
        public RewardHandlerTimeBooster(Reward reward) : base(reward)
        {
        }

        public override void ApplyReward(object sender, bool instantly)
        {
            ProductStateTimeBooster state = GetCaseState();
            int count = (reward.info as RewardInfoTimeBooster).count;
            for (int i = 0; i < count; i++)
                state.AddBooster();
            reward.NotifyAboutRewardReceived(SUCCESS);
            OnTimeBoosterRewarded?.Invoke(reward);
        }

        private ProductStateTimeBooster GetCaseState()
        {
            RewardInfoTimeBooster rewardInfoTimeBooster = reward.info as RewardInfoTimeBooster;
            string currentTimeBoosterId = rewardInfoTimeBooster.timeBoosterInfo.GetId();

            Product[] productts = Shop.GetProducts<ProductInfoTimeBooster>();
            foreach (Product product in productts)
            {
                if (product.id == currentTimeBoosterId)
                {
                    return product.state as ProductStateTimeBooster;
                }
            }

            Logging.LogError($"Cannot find product TimeBooster with id = {currentTimeBoosterId}");
            return null;
        }
    }
}