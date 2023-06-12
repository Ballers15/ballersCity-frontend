using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [CreateAssetMenu(
        fileName = "QuestInfoUpgradePerson",
        menuName = "Meta/Quests/QuestInfoUpgradePerson"
    )]
    public sealed class QuestInfoUpgradePerson : DailyQuestInfo
    {
        [SerializeField]
        private int m_times = 1;

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorUpgradePerson(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateUpgradePerson(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            var questState = new QuestStateUpgradePerson(this)
            {
                requiredTimes = this.m_times
            };
            return questState;
        }

        public override string GetDescription()
        {
            return "Прокачать любой персонал";
        }
    }
}