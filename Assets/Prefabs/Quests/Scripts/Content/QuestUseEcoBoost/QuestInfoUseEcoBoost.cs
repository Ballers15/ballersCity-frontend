using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [CreateAssetMenu(
        fileName = "QuestInfoUseEcoBoost",
        menuName = "Meta/Quests/QuestInfoUseEcoBoost"
    )]
    public sealed class QuestInfoUseEcoBoost : DailyQuestInfo
    {
        [SerializeField]
        private int m_times = 1;

        public override bool CanCreateQuest()
        {
            var ecoBoostInteractor = Game.GetInteractor<EcoBoostInteractor>();
            return ecoBoostInteractor.isEcoboostUnlocked;
        }

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorUseEcoBoost(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateUseEcoBoost(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            var state = new QuestStateUseEcoBoost(this)
            {
                requiredTimes = this.m_times
            };
            return state;
        }

        public override string GetDescription()
        {
            return "Использовать экобуст";
        }
    }
}