using SinSity.Domain.Utils;
using SinSity.Core;
using SinSity.Quests.Meta;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [CreateAssetMenu(
        fileName = "QuestInfoUpgradeBuildingLevel",
        menuName = "Meta/Quests/QuestInfoUpgradeBuildingLevel"
    )]
    public sealed class QuestInfoUpgradeBuildingLevel : DailyQuestInfo
    {
        [SerializeField]
        private int m_minLevelSteps;

        [SerializeField]
        private int m_maxLevelSteps;

        public int minLevelSteps
        {
            get { return this.m_minLevelSteps; }
        }

        public int maxLevelSteps
        {
            get { return this.m_maxLevelSteps; }
        }

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorUpgradeBuildingLevel(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateUpgradeBuildingLevel(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            if (!idleObjectInteractor.HasMostExpensiveBuiltIdleObject(out var idleObject))
            {
                idleObject = idleObjectInteractor.GetIdleObject("io_1");
            }
            
            var idleObjectId = idleObject.id;
            var stateDefault = new QuestStateUpgradeBuildingLevel(this)
            {
                idleObjectId = idleObjectId
            };
            var startLevel = idleObject.state.level;
            stateDefault.startLevel = startLevel;
            var randomLevelStep = new System.Random().Next(this.minLevelSteps, this.maxLevelSteps + 1);
            stateDefault.targetLevel = startLevel + randomLevelStep;
            return stateDefault;
        }
    }
}