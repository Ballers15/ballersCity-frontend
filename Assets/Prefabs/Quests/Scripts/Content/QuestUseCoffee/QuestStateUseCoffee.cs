using System;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [Serializable]
    public sealed class QuestStateUseCoffee : QuestStateEcoClicker
    {
        [SerializeField] 
        public int requiredTimes;

        [SerializeField] 
        public int currentTimes;

        public QuestStateUseCoffee(string stateJson) : base(stateJson)
        {
        }

        public QuestStateUseCoffee(QuestInfo info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateUseCoffee>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isCompleted = state.isCompleted;
            this.isViewed = state.isViewed;
            this.completeTimes = state.completeTimes;
            this.requiredTimes = state.requiredTimes;
            this.currentTimes = state.currentTimes;
            this.isRewardTaken = state.isRewardTaken;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest)
        {
            var localizingString = Localization.GetTranslation("ID_Q_USE_COFFEE");
            var finalString = string.Format(localizingString, this.requiredTimes);
            return finalString;
        }
    }
}