using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [CreateAssetMenu(
        fileName = "QuestInfoCollectAirDrop",
        menuName = "Meta/Quests/QuestInfoCollectAirDrop"
    )]
    public sealed class QuestInfoCollectAirDrop : DailyQuestInfo
    {
        [SerializeField]
        private int minTimes;

        [SerializeField]
        private int maxTimes;

        public override bool CanCreateQuest()
        {
            var airDropInteractor = Game.GetInteractor<AirDropInteractor>();
            return airDropInteractor.isAirDropEnabled;
        }

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorCollectAirDrop(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateCollectAirDrop(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            var questState = new QuestStateCollectAirDrop(this)
            {
                needToCollectTimes = new System.Random().Next(this.minTimes, this.maxTimes + 1)
            };
            return questState;
        }

        public override string GetDescription()
        {
            return "Собрать аирдропы";
        }
    }
}