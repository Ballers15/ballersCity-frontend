using System;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [Serializable]
    public sealed class QuestStateUseTimeBooster : QuestStateEcoClicker
    {
        [SerializeField]
        public int requiredTimes;

        [SerializeField]
        public int currentTimes;

        public QuestStateUseTimeBooster(string stateJson) : base(stateJson)
        {
        }

        public QuestStateUseTimeBooster(QuestInfoUseTimeBooster info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateUseTimeBooster>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isViewed = state.isViewed;
            this.isCompleted = state.isCompleted;
            this.completeTimes = state.completeTimes;
            this.requiredTimes = state.requiredTimes;
            this.currentTimes = state.currentTimes;
            this.isRewardTaken = state.isRewardTaken;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest) {
            return Localization.GetTranslation("ID_Q_USE_TBOOSTER");
        }
    }
}