using System;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [Serializable]
    public sealed class QuestStateOpenCases : QuestStateEcoClicker
    {
        [SerializeField]
        public int openedCases;

        [SerializeField]
        public int needOpenCases;

        public QuestStateOpenCases(string stateJson) : base(stateJson)
        {
        }

        public QuestStateOpenCases(QuestInfoOpenCases info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateOpenCases>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isViewed = state.isViewed;
            this.isRewardTaken = state.isRewardTaken;
            this.isCompleted = state.isCompleted;
            this.completeTimes = state.completeTimes;
            this.openedCases = state.openedCases;
            this.needOpenCases = state.needOpenCases;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest) {
            string localizingString = Localization.GetTranslation("ID_Q_OPEN_CASES");
            return string.Format(localizingString, needOpenCases);
        }
    }
}