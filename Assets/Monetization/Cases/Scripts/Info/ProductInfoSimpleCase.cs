using System.Collections.Generic;
using SinSity.Domain.Utils;
using SinSity.Domain;
using SinSity.Extensions;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using Random = System.Random;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "ProductInfoSimpleCase",
        menuName = "Monetization/ProductInfo/New SimpleCase"
    )]
    public sealed class ProductInfoSimpleCase : ProductInfoCase
    {
        [SerializeField] private RewardInfoSetupCard m_rewardInfoPersonageCard;
        [SerializeField] private ChanceGroupWithCount chanceGroupWithCountPersonageCard;
        
        [Space]
        [SerializeField] private RewardInfoSetupSoftCurrency m_rewardInfoSoftCurrency;
        [SerializeField] private ChanceGroupWithCount chanceGroupWithCountSoftCurrency;

        [Space]
        [SerializeField] private RewardInfoSetupHardCurrency m_rewardInfoHardCurrency;
        [SerializeField] private ChanceGroupWithCount chanceGroupWithCountHardCurrency;
        
        public override IEnumerable<RewardInfoEcoClicker> GetRewardInfoSet()
        {
            var rewardInfoSet = new List<RewardInfoEcoClicker>();
            this.AddRandomPersonageCard(rewardInfoSet);
            this.TryAddRandomCleanEnergy(rewardInfoSet);
            this.AddRandomGems(rewardInfoSet);
            Debug.Log("REWARD INFO SIZE: " + rewardInfoSet.Count);
            return rewardInfoSet;
        }

        private void TryAddRandomCleanEnergy(List<RewardInfoEcoClicker> rewardInfoSet) {
            int randomCoef = this.chanceGroupWithCountSoftCurrency.GetRandomCount();
            if (randomCoef <= 0) {
                Logging.Log("SIMPLE_CASE: NO CLEAN ENERGY!");
                return;
            }

            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            
            
            var fullObjectsIncome = idleObjectInteractor.GetFullIncomeByIdleObjectsForTime(randomCoef);
            if (fullObjectsIncome.bigIntegerValue <= 0)
                fullObjectsIncome = new BigNumber(1000);

            var rewardInfo = Instantiate(this.m_rewardInfoSoftCurrency);
            rewardInfo.Setup(fullObjectsIncome);
            rewardInfoSet.Add(rewardInfo);
        }
        
        private void AddRandomGems(List<RewardInfoEcoClicker> rewardInfoSet)
        {
            Logging.Log("SIMPLE_CASE: ADDED GEMS!!!");
            var rewardInfo = Instantiate(this.m_rewardInfoHardCurrency);
            var gemsCount = this.chanceGroupWithCountHardCurrency.GetRandomCount();
            rewardInfo.Setup(gemsCount);
            rewardInfoSet.Add(rewardInfo);
        }

        //TODO: OLD CARDS SYSTEM DELETED
        private void AddRandomPersonageCard(List<RewardInfoEcoClicker> rewardInfoSet) {
            /*int rCount = chanceGroupWithCountPersonageCard.GetRandomCount();
            if (rCount <= 0) {
                Logging.Log($"SIMPLE_CASE: created {rCount} personage cards");
                return;
            }
            
            Logging.Log("SIMPLE_CASE: TRY ADD RANDOM PERSONAGE! ");
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            CardObjectDataInteractor cardObjectDataInteractor = Game.GetInteractor<CardObjectDataInteractor>();
            if (!idleObjectInteractor.HasRandomBuiltIdleObjectWithNotMaximumCards(cardObjectDataInteractor, out var randomBuiltIdleObject))
            {
                Logging.Log("SIMPLE_CASE: NO RANDOM PERSONAGE! ");
                return;
            }

            Logging.Log("SIMPLE_CASE: ADD RANDOM PERSONAGE! ");
            var idleObjectId = randomBuiltIdleObject.id;
            var cardInteractor = Game.GetInteractor<CardObjectDataInteractor>();
            var cardObject = cardInteractor.GetCardObjectByIdleObjectId(idleObjectId);
            var cardId = cardObject.info.id;
            var rewardInfo = Instantiate(this.m_rewardInfoPersonageCard);

            int finalCount = cardObject.currentLevelIndex == 0 && cardObject.currentCardCount <= 0 ? 1 : rCount;
            rewardInfo.Setup(cardId, finalCount);
            rewardInfoSet.Add(rewardInfo);*/
        }
    }
}