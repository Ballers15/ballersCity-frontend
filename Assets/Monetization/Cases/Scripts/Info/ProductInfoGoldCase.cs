using System.Collections.Generic;
using SinSity.Domain.Utils;
using SinSity.Monetization;
using Orego.Util;
using SinSity.Domain;
using SinSity.Extensions;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Core
{
    //TODO: ТУТ ПО ХОРОШЕМУ НУЖНО БИЛДЕР ПИСАТЬ!!!!
    [CreateAssetMenu(fileName = "ProductInfoGoldCase", menuName = "Monetization/ProductInfo/New GoldCase")]
    public sealed class ProductInfoGoldCase : ProductInfoCase
    {
        [Header("Cards")]
        [SerializeField] private RewardInfoSetupCard m_rewardInfoPersonageCard;
        [SerializeField] private ChanceGroupWithCount chanceGroupWithCountPersonageCard;

        [Header("Clean Energy")]
        [SerializeField] private RewardInfoSetupSoftCurrency m_rewardInfoSoftCurrency;
        [SerializeField] private ChanceGroupWithCount chanceGroupWithCountSoftCurrency;

        [Header("Gems")]
        [SerializeField] private RewardInfoSetupHardCurrency m_rewardInfoHardCurrency;
        [SerializeField] private ChanceGroupWithCount chanceGroupWithCountHardCurrency;

        [Header("Time Booster")]
        [SerializeField] private ChanceGroupTimeBoosters chanceGroupTimeBoosters;

        public override IEnumerable<RewardInfoEcoClicker> GetRewardInfoSet()
        {
            var rewardInfoSet = new List<RewardInfoEcoClicker>();
            this.AddRandomPersonageCard(rewardInfoSet);
            this.AddRandomCleanEnergy(rewardInfoSet);
            this.AddRandomGems(rewardInfoSet);
            this.AddTimeBooster(rewardInfoSet);
            return rewardInfoSet;
        }

        private void AddRandomPersonageCard(List<RewardInfoEcoClicker> rewardInfoSet)
        {
            /*var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            CardObjectDataInteractor cardObjectDataInteractor = Game.GetInteractor<CardObjectDataInteractor>();
            if (!idleObjectInteractor.HasRandomBuiltIdleObjectWithNotMaximumCards(cardObjectDataInteractor, out var randomBuiltIdleObject))
            {
                return;
            }

            var cardCount = this.chanceGroupWithCountPersonageCard.GetRandomCount();
            var idleObjectId = randomBuiltIdleObject.id;
            var cardInteractor = Game.GetInteractor<CardObjectDataInteractor>();
            var cardObject = cardInteractor.GetCardObjectByIdleObjectId(idleObjectId);
            var cardId = cardObject.info.id;
            var rewardInfo = Instantiate(this.m_rewardInfoPersonageCard);
            
            int finalCount = cardObject.currentLevelIndex == 0 && cardObject.currentCardCount <= 0 ? 1 : cardCount;
            rewardInfo.Setup(cardId, finalCount);
            rewardInfoSet.Add(rewardInfo);*/
        }

        private void AddRandomCleanEnergy(List<RewardInfoEcoClicker> rewardInfoSet)
        {
            int cleanEnergyCoef = this.chanceGroupWithCountSoftCurrency.GetRandomCount();
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            var fullObjectsIncome = idleObjectInteractor.GetFullIncomeByIdleObjectsForTime(cleanEnergyCoef);
            if (fullObjectsIncome.bigIntegerValue <= 0)
            {
                fullObjectsIncome = new BigNumber(1000) * cleanEnergyCoef;
            }

            var rewardInfo = Instantiate(this.m_rewardInfoSoftCurrency);
            rewardInfo.Setup(fullObjectsIncome);
            rewardInfoSet.Add(rewardInfo);
        }

        private void AddRandomGems(List<RewardInfoEcoClicker> rewardInfoSet)
        {
            int gemsValue = this.chanceGroupWithCountHardCurrency.GetRandomCount();
            var rewardInfo = Instantiate(this.m_rewardInfoHardCurrency);
            rewardInfo.Setup(gemsValue);
            rewardInfoSet.Add(rewardInfo);
        }

        private void AddTimeBooster(List<RewardInfoEcoClicker> rewardInfoSet) {
            ChanceTimeBooster chanceTimeBooster = this.chanceGroupTimeBoosters.GetRandomChanceTimeBooster();

            if (chanceTimeBooster == null)
                return;
            
            int count = chanceTimeBooster.randomCount;
            RewardInfoTimeBooster rewardInfoTimeBooster = chanceTimeBooster.rewardInfoTimeBooster;
            
            if (rewardInfoTimeBooster == null)
                return;
             
            var rewardInfo = Instantiate(rewardInfoTimeBooster);
            rewardInfo.Setup(count);
            rewardInfoSet.Add(rewardInfo);
        }
    }
}