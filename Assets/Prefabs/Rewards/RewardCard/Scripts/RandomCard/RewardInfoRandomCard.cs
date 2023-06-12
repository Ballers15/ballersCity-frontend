using System.Linq;
using Ecorobotics;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards {
    [CreateAssetMenu(
        fileName = "RewardInfoRandomCard",
        menuName = "Meta/Rewards/RewardInfoRandomCard"
    )]
    public class RewardInfoRandomCard : RewardInfoEcoClicker {
        [SerializeField] private CardWithChanceWeight[] _cardsList;

        public ICardInfo GetRandomCardInfo() {
            var objectsWithWeights = _cardsList.Select(valuesWithWeight => valuesWithWeight.GetObjectWithWeight()).ToArray();
            var random = new RandomizerWeight(objectsWithWeights);
            return random.GetRandom<CardInfo>();
        }

        public override RewardHandler CreateRewardHandler(Reward reward) {
            return new RewardHandlerRandomCard(reward);
        }

        public override string GetCountToString() {
            return "1";
        }
    }
}