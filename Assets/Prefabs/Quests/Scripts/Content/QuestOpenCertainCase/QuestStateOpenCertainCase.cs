using System;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Monetization;

namespace SinSity.Quests.Meta
{
    [Serializable]
    public sealed class QuestStateOpenCertainCase : QuestStateEcoClicker
    {
        [SerializeField]
        public string needOpenCaseId;

        public QuestStateOpenCertainCase(string stateJson) : base(stateJson)
        {
        }

        public QuestStateOpenCertainCase(QuestInfoOpenCertainCase info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateOpenCertainCase>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isViewed = state.isViewed;
            this.isRewardTaken = state.isRewardTaken;
            this.isCompleted = state.isCompleted;
            this.completeTimes = state.completeTimes;
            this.needOpenCaseId = state.needOpenCaseId;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest) {
            string localizingString = Localization.GetTranslation("ID_Q_OPEN_CASE_CERTAIN");
            Product productCase = Shop.GetProduct(needOpenCaseId);

            ProductInfo caseInfo = productCase.GetInfo<ProductInfo>();
            string caseTitle = Localization.GetTranslation(caseInfo.GetTitle());

            return string.Format(localizingString, caseTitle);
        }
    }
}