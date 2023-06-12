using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards {
    [CreateAssetMenu(fileName = "RewardInfoCoffeeBoost", menuName = "Meta/Rewards/RewardCoffeeBoost")]
    public class RewardInfoCoffeeBoost : RewardInfoEcoClicker {
        
        public override RewardHandler CreateRewardHandler(Reward reward) {
            return new RewardHandlerCoffeeBoost(reward);
        }

        public override string GetCountToString() {
            return "1";
        }
    }
}