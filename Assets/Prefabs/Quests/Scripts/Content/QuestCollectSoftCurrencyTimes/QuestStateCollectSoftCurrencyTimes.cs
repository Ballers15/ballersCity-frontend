using System;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [Serializable]
    public sealed class QuestStateCollectSoftCurrencyTimes : QuestStateEcoClicker
    {
        [SerializeField]
        public int timesCollected;

        public QuestStateCollectSoftCurrencyTimes(string stateJson) : base(stateJson)
        {
        }

        public QuestStateCollectSoftCurrencyTimes(QuestInfoCollectSoftCurrencyTimes info) : base(info) {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateCollectSoftCurrencyTimes>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isCompleted = state.isCompleted;
            this.isViewed = state.isViewed;
            this.isRewardTaken = state.isRewardTaken;
            this.completeTimes = state.completeTimes;
            this.timesCollected = state.timesCollected;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void AddTime()
        {
            this.timesCollected += 1;
        }
        
        public override string GetDescription(Quest quest) {
            QuestInfoCollectSoftCurrencyTimes questInfo = quest.GetInfo<QuestInfoCollectSoftCurrencyTimes>();
            string localizingString = Localization.GetTranslation("ID_Q_COLLECT_INCOME");
            return string.Format(localizingString, questInfo.needToCollectTimes);
        }
    }
}