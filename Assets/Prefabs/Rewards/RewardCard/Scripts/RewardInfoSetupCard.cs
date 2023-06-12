//using Facebook.Unity;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards
{
    [CreateAssetMenu(
        fileName = "RewardInfoSetupPersonageCard",
        menuName = "Meta/Rewards/RewardInfoSetupPersonageCard"
    )]
    public sealed class RewardInfoSetupCard : RewardInfoEcoClicker {
        public string cardId { get; private set; }
        public int count { get; private set; }

        public void Setup(string cardId, int count) {
            //TODO: OLD CARDS SYSTEM DELETED
            this.cardId = cardId;
            this.count = count;
            //var cardInfo = Game.GetInteractor<CardsInteractor>().GetCard(cardId).info;
            //this.m_description = cardInfo.;
            //this.m_spriteIcon = cardObjectInfo.spriteIconThick;
        }

        public override RewardHandler CreateRewardHandler(Reward reward) {
            return new RewardHandlerSetupCard(reward);
        }

        public override string GetCountToString() {
            return this.count.ToString();
        }

        public override string GetTitle() {
            return "";
        }

        public override string GetDescription() {
            return Localization.GetTranslation("ID_PERS_CARD_TITLE");
        }
    }
}