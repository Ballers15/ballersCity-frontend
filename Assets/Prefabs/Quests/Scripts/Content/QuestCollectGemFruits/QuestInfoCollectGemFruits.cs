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
        fileName = "QuestInfoCollectGemFruits",
        menuName = "Meta/Quests/QuestInfoCollectGemFruits"
    )]
    public sealed class QuestInfoCollectGemFruits : DailyQuestInfo
    {
        [SerializeField]
        public int requiredTimes;

        public override bool CanCreateQuest()
        {
            var researchStateInteractor = Game.GetInteractor<GemTreeStateInteractor>();
            return researchStateInteractor.isTreeUnlocked;
        }

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorCollectGemFruits(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateCollectGemFruits(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            return new QuestStateCollectGemFruits(this);
        }

        public override string GetDescription()
        {
            return "Собрать фрукты";
        }
    }
    
    
}