using System;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [Serializable]
    public sealed class QuestStateUpgradePerson : QuestStateEcoClicker
    {
        [SerializeField]
        public int requiredTimes;

        [SerializeField] 
        public int currentTimes;
        
        public QuestStateUpgradePerson(string stateJson) : base(stateJson)
        {
        }

        public QuestStateUpgradePerson(QuestInfo info) : base(info)
        {
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest) {
            return Localization.GetTranslation("ID_Q_UPGRADE_PERS");
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateUpgradePerson>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isViewed = state.isViewed;
            this.isRewardTaken = state.isRewardTaken;
            this.isCompleted = state.isCompleted;
            this.completeTimes = state.completeTimes;
            this.requiredTimes = state.requiredTimes;
            this.currentTimes = state.currentTimes;
        }
    }
}