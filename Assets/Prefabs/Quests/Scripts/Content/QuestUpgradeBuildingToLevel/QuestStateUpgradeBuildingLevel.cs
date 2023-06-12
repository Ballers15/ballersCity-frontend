using System;
using SinSity.Domain.Utils;
using SinSity.Core;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [Serializable]
    public sealed class QuestStateUpgradeBuildingLevel : QuestStateEcoClicker
    {
        [SerializeField]
        public string idleObjectId;

        [SerializeField]
        public int startLevel;

        [SerializeField]
        public int targetLevel;

        public QuestStateUpgradeBuildingLevel(string stateJson) : base(stateJson)
        {
        }

        public QuestStateUpgradeBuildingLevel(QuestInfoUpgradeBuildingLevel info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateUpgradeBuildingLevel>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isViewed = state.isViewed;
            this.isCompleted = state.isCompleted;
            this.completeTimes = state.completeTimes;
            this.startLevel = state.startLevel;
            this.targetLevel = state.targetLevel;
            this.idleObjectId = state.idleObjectId;
            this.isRewardTaken = state.isRewardTaken;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest) {
            string localizingString = Localization.GetTranslation("ID_Q_UPGRADE_IOBJECT");
            
            IdleObjectsInteractor ioInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            IdleObject io = ioInteractor.GetIdleObject(idleObjectId);
            string ioTitle = Localization.GetTranslation(io.info.GetTitle());
            
            string finalString = string.Format(localizingString, ioTitle, io.info.number, targetLevel);
            return finalString;
        }
    }
}