using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [Serializable]
    public sealed class QuestStateCollectAirDrop : QuestStateEcoClicker
    {
        [SerializeField] 
        public int needToCollectTimes;

        [SerializeField] 
        public int currentCollectTimes;

        public QuestStateCollectAirDrop(string stateJson) : base(stateJson)
        {
        }

        public QuestStateCollectAirDrop(QuestInfo info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateCollectAirDrop>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isCompleted = state.isCompleted;
            this.isViewed = state.isViewed;
            this.completeTimes = state.completeTimes;
            this.needToCollectTimes = state.needToCollectTimes;
            this.currentCollectTimes = state.currentCollectTimes;
            this.isRewardTaken = state.isRewardTaken;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest)
        {
            string localizingString = Localization.GetTranslation("ID_Q_COLLECT_AIRDROPS");
            string finalString = string.Format(localizingString, needToCollectTimes);
            return finalString;
        }
    }
}