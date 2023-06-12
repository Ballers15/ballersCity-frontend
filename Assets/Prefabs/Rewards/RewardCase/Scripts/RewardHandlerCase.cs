using SinSity.Core;
using SinSity.Monetization;
using UnityEngine;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Meta.Rewards
{
    public class RewardHandlerCase : RewardHandler
    {
        public RewardHandlerCase(Reward reward) : base(reward)
        {
        }

        public override void ApplyReward(object sender, bool instantly)
        {
            ProductStateCase state = GetCaseState();
            RewardInfoCase rewardInfoCase = reward.info as RewardInfoCase;
            int count = rewardInfoCase.count;
            for (int i = 0; i < count; i++)
                state.AddCase();
            
            reward.NotifyAboutRewardReceived(SUCCESS);
        }

        private ProductStateCase GetCaseState()
        {
            var infoCase = reward.info as RewardInfoCase;
            var currentCaseId = infoCase.caseInfo.GetId();
            var productts = Shop.GetProducts<ProductInfoCase>();
            foreach (var product in productts)
            {
                if (product.id == currentCaseId)
                {
                    return product.state as ProductStateCase;
                }
            }

            Logging.LogError($"Cannot find product Case with id = {currentCaseId}");
            return null;
        }
    }
}