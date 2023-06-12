using SinSity.Domain;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards
{
    public sealed class RewardHandlerCard : RewardHandler
    {
        public RewardHandlerCard(Reward reward) : base(reward)
        {
        }

        public override void ApplyReward(object sender, bool instantly)
        {
            var rewardInfo = reward.info as RewardInfoCard;
            var cardInteractor = Game.GetInteractor<CardsInteractor>();
            var card = cardInteractor.GetCard(rewardInfo.cardId);
            card.IncreaseAmount(rewardInfo.count);
            this.reward.NotifyAboutRewardReceived(true);
        }
    }
}