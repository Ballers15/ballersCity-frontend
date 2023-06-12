using SinSity.Domain;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards {
    public class RewardHandlerRandomCard : RewardHandler {
        public RewardHandlerRandomCard(Reward reward) : base(reward) {
        }

        public override void ApplyReward(object sender, bool instantly) {
            var rewardInfo = reward.info as RewardInfoRandomCard;
            var cardInteractor = Game.GetInteractor<CardsInteractor>();
            var randomCardInfo = rewardInfo.GetRandomCardInfo();
            var card = cardInteractor.GetCard(randomCardInfo.id);
            card.IncreaseAmount(1);
            reward.NotifyAboutRewardReceived(true);
        }
    }
}