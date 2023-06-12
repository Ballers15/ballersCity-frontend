using IdleClicker.Gameplay;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards
{
    public class RewardHandlerCoffeeBoost : RewardHandler
    {
        public RewardHandlerCoffeeBoost(Reward reward) : base(reward) { }

        public override void ApplyReward(object sender, bool instantly) {
            var coffeeBoostInteractor = Game.GetInteractor<CoffeeBoostInteractor>();
            coffeeBoostInteractor.coffeeBoost.Activate();
        }
    }
}