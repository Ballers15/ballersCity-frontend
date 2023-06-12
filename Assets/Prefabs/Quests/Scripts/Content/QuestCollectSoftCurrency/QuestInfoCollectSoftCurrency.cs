using SinSity.Domain.Utils;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Tools;

namespace SinSity.Quests.Meta
{
    [CreateAssetMenu(
        fileName = "QuestInfoCollectSoftCurrency",
        menuName = "Meta/Quests/QuestInfoCollectSoftCurrency"
    )]
    public sealed class QuestInfoCollectSoftCurrency : DailyQuestInfo
    {
        [SerializeField]
        public int minMultiplier = 3000;

        [SerializeField]
        public int maxMultiplier = 6000;

        [SerializeField]
        public BigNumber minNeedToCollect = new BigNumber(5);

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorCollectSoftCurrency(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateCollectSoftCurrency(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            var fullIncome = idleObjectInteractor.GetFullIncomeFromIdleObjects();
            if (fullIncome < this.minNeedToCollect)
            {
                fullIncome = new BigNumber(this.minNeedToCollect);
            }

            var ramdomMultiplier = new System.Random().Next(this.minMultiplier, this.maxMultiplier);
            var needToCollectSoftCurrency = fullIncome * ramdomMultiplier;
            var questState = new QuestStateCollectSoftCurrency(this);
            questState.SetCollectedSoftCurrency(new BigNumber(0));
            questState.SetNeedToCollectSoftCurrency(needToCollectSoftCurrency);
            return questState;
        }

        public override string GetDescription()
        {
            return "Собрать чистую энергию";
        }
    }
}