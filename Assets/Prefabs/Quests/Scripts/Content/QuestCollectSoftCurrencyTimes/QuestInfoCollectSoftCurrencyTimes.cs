using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [CreateAssetMenu(
        fileName = "QuestInfoCollectSoftCurrencyTimes",
        menuName = "Meta/Quests/QuestInfoCollectSoftCurrencyTimes"
    )]
    public sealed class QuestInfoCollectSoftCurrencyTimes : QuestInfoEcoClicker
    {
        [SerializeField]
        private int m_needToCollectTimes;

        public int needToCollectTimes
        {
            get { return this.m_needToCollectTimes; }
        }

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorCollectSoftCurrencyTimes(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateCollectSoftCurrencyTimes(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            return new QuestStateCollectSoftCurrencyTimes(this);
        }
    }
}