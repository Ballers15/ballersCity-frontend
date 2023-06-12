using System;
using SinSity.Domain.Utils;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [CreateAssetMenu(
        fileName = "QuestInfoBuildSmth",
        menuName = "Meta/Quests/QuestInfoBuildSmth"
    )]
    public sealed class QuestInfoBuildSmth : QuestInfoEcoClicker
    {
        public override bool CanCreateQuest()
        {
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            return idleObjectInteractor.HasMostCheapNotBuiltIdleObject(out _);
        }

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorBuildSmth(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateBuildSmth(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            if (!idleObjectInteractor.HasMostCheapNotBuiltIdleObject(out var idleObject))
            {
                throw new Exception("Can not create state!");
            }

            var idleObjectId = idleObject.id;
            var stateDefault = new QuestStateBuildSmth(this)
            {
                idleObjectId = idleObjectId
            };
            return stateDefault;
        }

        public override string GetDescription()
        {
            return "Построить объект";
        }
    }
}