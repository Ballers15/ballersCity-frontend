using System.Collections.Generic;
using SinSity.Domain.Utils;
using SinSity.Core;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "DefaultLevelUnlockConfigWithRandomCard",
        menuName = "Game/ProfileLevel/New DefaultLevelUnlockConfigWithRandomCard"
    )]
    public class DefaultLevelUnlockConfigWithRandomCard : DefaultLevelUnlockConfig
    {
        [SerializeField] private RewardInfoSetupCard rewardInfoCard;
        [SerializeField] private int cardCount;

        protected override IEnumerable<Reward> GenerateRewards()
        {
            var baseRewards = base.GenerateRewards();
            var rewards = new List<Reward>(baseRewards);
            this.AddRandomPersonCard(rewards);
            return rewards;
        }

        private void AddRandomPersonCard(List<Reward> rewards)
        {
            //TODO: OLD CARDS SYSTEM DELETED
            /*if(cardCount <= 0)
                return;
            
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            var cardObjectDataInteractor = Game.GetInteractor<CardObjectDataInteractor>();
            if (!idleObjectInteractor.HasRandomBuiltIdleObjectWithNotMaximumCards(
                cardObjectDataInteractor, out var randomBuiltIdleObject
            ))
            {
                return;
            }

            var idleObjectId = randomBuiltIdleObject.id;
            var cardInteractor = Game.GetInteractor<CardObjectDataInteractor>();
            var cardObject = cardInteractor.GetCardObjectByIdleObjectId(idleObjectId);
            var cardId = cardObject.info.id;
            var rewardInfo = Instantiate(this.rewardInfoCard);

            rewardInfo.Setup(cardId, this.cardCount);
            var reward = new Reward(rewardInfo);
            rewards.Add(reward);*/
        }
    }
}