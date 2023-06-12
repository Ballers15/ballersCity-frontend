using System;
using SinSity.Tools;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Tools;
using BigNumberLocalizator = VavilichevGD.Tools.BigNumberLocalizator;

namespace SinSity.Quests.Meta
{
    [Serializable]
    public sealed class QuestStateCollectSoftCurrency : QuestStateEcoClicker
    {
        public BigNumber collectedSoftCurrency { get; private set; }

        public BigNumber needToCollectSoftCurrency { get; private set; }

        [SerializeField]
        public string collectedSoftCurrencyString;

        [SerializeField] 
        public string needToCollectSoftCurrencyString;

        public QuestStateCollectSoftCurrency(string stateJson) : base(stateJson)
        {
        }

        public QuestStateCollectSoftCurrency(QuestInfoCollectSoftCurrency info) : base(info)
        {
        }

        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<QuestStateCollectSoftCurrency>(stateJson);
            this.id = state.id;
            this.isActive = state.isActive;
            this.isViewed = state.isViewed;
            this.isCompleted = state.isCompleted;
            this.isRewardTaken = state.isRewardTaken;
            this.completeTimes = state.completeTimes;
            this.collectedSoftCurrencyString = state.collectedSoftCurrencyString;
            this.needToCollectSoftCurrencyString = state.needToCollectSoftCurrencyString;
            this.collectedSoftCurrency = new BigNumber(this.collectedSoftCurrencyString);
            this.needToCollectSoftCurrency = new BigNumber(this.needToCollectSoftCurrencyString);
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string GetDescription(Quest quest) {
            string localizingString = Localization.GetTranslation("ID_Q_COLLECT_SOFT_TOTAL");
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            string numberLocalized = needToCollectSoftCurrency.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            return string.Format(localizingString, numberLocalized);
        }


        public void SetCollectedSoftCurrency(BigNumber bigNumber)
        {
            this.collectedSoftCurrency = new BigNumber(bigNumber);
            this.collectedSoftCurrencyString = bigNumber.ToString(BigNumber.FORMAT_FULL);
        }

        public void SetNeedToCollectSoftCurrency(BigNumber bigNumber)
        {
            this.needToCollectSoftCurrency = new BigNumber(bigNumber);
            this.needToCollectSoftCurrencyString = bigNumber.ToString(BigNumber.FORMAT_FULL);
        }
    }
}