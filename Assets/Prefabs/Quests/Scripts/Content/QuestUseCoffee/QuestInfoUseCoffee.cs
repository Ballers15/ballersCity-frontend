using IdleClicker.Gameplay;
using SinSity.Core;
using SinSity.Quests.Meta;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [CreateAssetMenu(
        fileName = "QuestInfoUseCoffee",
        menuName = "Meta/Quests/QuestInfoUseCoffee"
    )]
    public sealed class QuestInfoUseCoffee : DailyQuestInfo
    {
        [SerializeField]
        private int minTimes;

        [SerializeField]
        private int maxTimes;

        public override bool CanCreateQuest()
        {
            var coffeeBoostInteractor = Game.GetInteractor<CoffeeBoostInteractor>();
            return coffeeBoostInteractor.isCoffeeBoostUnlocked;
        }

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorUseCoffee(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateUseCoffee(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            var questState = new QuestStateUseCoffee(this)
            {
                requiredTimes = new System.Random().Next(this.minTimes, this.maxTimes + 1)
            };
            return questState;
        }

        public override string GetDescription()
        {
            return "Использовать кофебуст";
        }
    }
}