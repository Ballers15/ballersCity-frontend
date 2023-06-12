using System.Collections.Generic;
using SinSity.Domain;
using SinSity.Domain.Utils;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "QuestUnlockConfig",
        menuName = "Game/Quests/New QuestUnlockConfig"
    )]
    public sealed class QuestUnlockConfig : ProfileLevelUnlockWithActivityConfig
    {
        [SerializeField]
        private RewardInfo[] rewardInfoSet;

//        [SerializeField]
//        private RewardInfoSetupPersonageCard rewardInfoCard;
//
//        [SerializeField]
//        private int cardCount;

        protected override IEnumerable<Reward> GenerateRewards()
        {
            var rewards = new List<Reward>();
            foreach (var rewardInfo in this.rewardInfoSet)
            {
                rewards.Add(new Reward(rewardInfo));
            }

//            this.AddRandomPersonCard(rewards);
            return rewards;
        }

//        private void AddRandomPersonCard(List<Reward> rewards)
//        {
//            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
//            var cardObjectDataInteractor = Game.GetInteractor<CardObjectDataInteractor>();
//            if (!idleObjectInteractor.HasRandomBuiltIdleObjectWithNotMaximumCards(
//                cardObjectDataInteractor, out var randomBuiltIdleObject
//            ))
//            {
//                return;
//            }
//
////            var idleObjectId = randomBuiltIdleObject.id;
////            var cardInteractor = Game.GetInteractor<CardObjectDataInteractor>();
////            var cardObject = cardInteractor.GetCardObjectByIdleObjectId(idleObjectId);
////            var cardId = cardObject.info.id;
////            var rewardInfo = Instantiate(this.rewardInfoCard);
////
////            rewardInfo.Setup(cardId, this.cardCount);
////            var reward = new Reward(rewardInfo);
////            rewards.Add(reward);
//        }
    }
}