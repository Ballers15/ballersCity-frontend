using System;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [Serializable]
    public sealed class QuestStateUseCertainTimeBooster : QuestStateEcoClicker
    {
        [SerializeField]
        public string needUseCertainBoosterId;

        public QuestStateUseCertainTimeBooster(string stateJson) : base(stateJson)
        {
        }

        public QuestStateUseCertainTimeBooster(QuestInfoUseCertainTimeBooster info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateUseCertainTimeBooster>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isViewed = state.isViewed;
            this.isCompleted = state.isCompleted;
            this.isRewardTaken = state.isRewardTaken;
            this.completeTimes = state.completeTimes;
            this.needUseCertainBoosterId = state.needUseCertainBoosterId;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest)
        {
            return "USE CERTAIN TIME BOOSTER: АНДРЕЙ, ПОЖАЛУЙСТА, ПОЖАЛУЙСТА, ПОЖАЛУЙСТА, ПОПРАВЬ МЕНЯ!";
        }
    }
}