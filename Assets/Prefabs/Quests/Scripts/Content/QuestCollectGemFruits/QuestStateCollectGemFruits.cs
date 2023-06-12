using System;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [Serializable]
    public sealed class QuestStateCollectGemFruits : QuestStateEcoClicker
    {
        [SerializeField]
        public int currentTimes;
        public int requiredTimes;
        
        public QuestStateCollectGemFruits(string stateJson) : base(stateJson)
        {
        }

        public QuestStateCollectGemFruits(QuestInfo questInfo) : base(questInfo) {
            QuestInfoCollectGemFruits questInfoCollectGemFruits = questInfo as QuestInfoCollectGemFruits;
            this.requiredTimes = questInfoCollectGemFruits.requiredTimes;
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateCollectGemFruits>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isViewed = state.isViewed;
            this.isCompleted = state.isCompleted;
            this.completeTimes = state.completeTimes;
            this.currentTimes = state.currentTimes;
            this.requiredTimes = state.requiredTimes;
            this.isRewardTaken = state.isRewardTaken;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest){
            string localizingString = Localization.GetTranslation("ID_Q_COLLECT_GEM_FRUITS");
            return string.Format(localizingString, requiredTimes);
        }
    }
}