using System;
using SinSity.Core;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [Serializable]
    public sealed class QuestStateBuildSmth : QuestStateEcoClicker
    {
        [SerializeField]
        public string idleObjectId;
        
        public QuestStateBuildSmth(string stateJson) : base(stateJson)
        {
        }

        public QuestStateBuildSmth(QuestInfoBuildSmth info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateBuildSmth>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isViewed = state.isViewed;
            this.isCompleted = state.isCompleted;
            this.completeTimes = state.completeTimes;
            this.idleObjectId = state.idleObjectId;
            this.isRewardTaken = state.isRewardTaken;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest) {
            IdleObjectsInteractor ioInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            IdleObject io = ioInteractor.GetIdleObject(idleObjectId);
            string ioTitle = Localization.GetTranslation(io.info.GetTitle());
            
            string localizedString = Localization.GetTranslation("ID_Q_BUILD_IO");
            return string.Format(localizedString, ioTitle, io.info.number);
        }
    }
}