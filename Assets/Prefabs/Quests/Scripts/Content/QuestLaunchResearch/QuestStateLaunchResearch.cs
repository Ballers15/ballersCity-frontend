using System;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [Serializable]
    public sealed class QuestStateLaunchResearch : QuestStateEcoClicker
    {
        [SerializeField]
        public int currentTimes;
        
        public QuestStateLaunchResearch(string stateJson) : base(stateJson)
        {
        }

        public QuestStateLaunchResearch(QuestInfo info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateLaunchResearch>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isCompleted = state.isCompleted;
            this.isRewardTaken = state.isRewardTaken;
            this.isViewed = state.isViewed;
            this.completeTimes = state.completeTimes;
            this.currentTimes = state.currentTimes;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest)
        {
            string localizingString = Localization.GetTranslation("ID_Q_LAUNCH_RESEARCH");
            QuestInfoLaunchResearch infoLaunchResearch = quest.GetInfo<QuestInfoLaunchResearch>();
            string finalString = string.Format(localizingString, infoLaunchResearch.requiredTimes);
            return finalString;
        }
    }
}