using SinSity.Domain;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards
{
    public sealed class RewardHandlerSetupCard : RewardHandler
    {
        public RewardHandlerSetupCard(Reward reward) : base(reward)
        {
        }

        public override void ApplyReward(object sender, bool instantly) {
            var rewardInfo = reward.info as RewardInfoSetupCard;
            var cardInteractor = Game.GetInteractor<CardsInteractor>();
            var card = cardInteractor.GetCard(rewardInfo.cardId);
            card.IncreaseAmount(rewardInfo.count);
            reward.NotifyAboutRewardReceived(true);
        }
    }
}