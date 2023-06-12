using SinSity.Core;
using SinSity.Domain;
using SinSity.Quests.Meta;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [CreateAssetMenu(
        fileName = "QuestInfoLaunchResearch",
        menuName = "Meta/Quests/QuestInfoLaunchResearch"
    )]
    public sealed class QuestInfoLaunchResearch : DailyQuestInfo
    {
        [SerializeField]
        public int requiredTimes;

        public override bool CanCreateQuest()
        {
            var researchStateInteractor = Game.GetInteractor<ResearchStateInteractor>();
            return researchStateInteractor.isResearchUnlocked;
        }

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorLaunchResearch(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateLaunchResearch(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            var questState = new QuestStateLaunchResearch(this);
            return questState;
        }

        public override string GetDescription()
        {
            return "Запустить исследования";
        }
    }
}