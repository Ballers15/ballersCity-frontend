using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta.Rewards;
using Localization = VavilichevGD.LocalizationFramework.Localization;

namespace SinSity.Meta.Rewards
{
    [CreateAssetMenu(
        fileName = "RewardInfoCard",
        menuName = "Meta/Rewards/RewardInfoCard"
    )]
    public sealed class RewardInfoCard : RewardInfoEcoClicker
    {
        [SerializeField] 
        public CardInfo _cardInfo;

        [SerializeField]
        public int count;

        public string cardId => _cardInfo.id;

        public override RewardHandler CreateRewardHandler(Reward reward) {
            return new RewardHandlerCard(reward);
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