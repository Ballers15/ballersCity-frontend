using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [CreateAssetMenu(
        fileName = "QuestInfoUseTimeBooster",
        menuName = "Meta/Quests/QuestInfoUseTimeBooster"
    )]
    public sealed class QuestInfoUseTimeBooster : DailyQuestInfo
    {
        [SerializeField]
        private int m_minTimes = 1;

        [SerializeField] 
        private int m_maxTimes = 1;

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorUseTimeBooster(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateUseTimeBooster(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            var requiredTimes = new System.Random().Next(this.m_minTimes, this.m_maxTimes + 1);
            var questState = new QuestStateUseTimeBooster(this)
            {
                requiredTimes = requiredTimes
            };
            return questState;
        }

        public override string GetDescription()
        {
            return "Использовать любой тайм-бустер";
        }
    }
}