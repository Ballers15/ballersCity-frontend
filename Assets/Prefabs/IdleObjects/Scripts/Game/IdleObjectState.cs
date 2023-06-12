using System;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Core
{
    [Serializable]
    public sealed class IdleObjectState
    {
        #region Const

        private const float INCOME_PERIOD_MIN = 0.2f;

        #endregion

        public string id;

        public string strPriceImprovement;

        public string strIncome;

        public string strCollectedCurrency;

        public IdleObjectMultiplicator<Coefficient> incomeConstMultiplicator { get; } =
            new IdleObjectMultiplicator<Coefficient>();

        public IdleObjectMultiplicator<TimeCoefficient> incomeTimeMultiplicator { get; } =
            new IdleObjectMultiplicator<TimeCoefficient>();

        public int localMultiplicatorDynamic;

        public int level;

        public float incomePeriod;

        public bool isBuilded;

        public bool isBuildedAnyTime;

        public float progressNomalized;

        public bool autoPlayEnabled;

        public float progressInTime => this.progressNomalized * this.incomePeriod;

        public BigNumber fullIncome
        {
            get
            {
                double externalConstMultiplier = this.incomeConstMultiplicator.GetTotalMultiplier();
                double externalTimeMultiplier = this.incomeTimeMultiplicator.GetTotalMultiplier();
                double totalCoefficient = this.level * this.localMultiplicatorDynamic
                                                           * externalConstMultiplier
                                                           * externalTimeMultiplier;
                totalCoefficient = Math.Max(totalCoefficient, 0f);

                return this.income * totalCoefficient;
            }
        }

        public bool hasAnyCollectedCurrency
        {
            get { return this.collectedCurrency > 0; }
        }

        public BigNumber priceImprovement { get; set; }

        public BigNumber income { get; set; }

        public BigNumber collectedCurrency { get; set; }

        public IdleObjectState(IdleObjectInfo info)
        {
            this.CleanExceptConstants(info);
            this.localMultiplicatorDynamic = 1;
        }

        public IdleObjectState(string stateJson)
        {
            this.Set(stateJson);
        }

        public string GetJson()
        {
            this.UpdateState();
            var stateJson = JsonUtility.ToJson(this);
            return stateJson;
        }

        private void UpdateState()
        {
            this.strPriceImprovement = this.priceImprovement.ToString(BigNumber.FORMAT_FULL);
            this.strIncome = this.income.ToString(BigNumber.FORMAT_FULL);
            this.strCollectedCurrency = this.collectedCurrency.ToString(BigNumber.FORMAT_FULL);
        }

        public void Set(string jsonState)
        {
            if (string.IsNullOrEmpty(jsonState))
            {
                return;
            }

            var state = JsonUtility.FromJson<IdleObjectState>(jsonState);
            this.id = state.id;
            this.priceImprovement = new BigNumber(state.strPriceImprovement);
            this.income = new BigNumber(state.strIncome);
            this.incomePeriod = state.incomePeriod;
            this.isBuilded = state.isBuilded;
            this.isBuildedAnyTime = state.isBuildedAnyTime;
            this.level = state.level;
            this.collectedCurrency = new BigNumber(state.strCollectedCurrency);
            this.localMultiplicatorDynamic = state.localMultiplicatorDynamic;
            this.progressNomalized = state.progressNomalized;
            this.autoPlayEnabled = state.autoPlayEnabled;
        }

        public void CleanExceptConstants(IdleObjectInfo info)
        {
            this.id = info.id;
            this.priceImprovement = info.priceImproveDefault;
            this.income = info.incomeDefault;
            this.isBuilded = false;
            this.incomePeriod = info.incomePeriodDefault;
            this.level = 0;
            this.collectedCurrency = new BigNumber(0);
            this.localMultiplicatorDynamic = 1;
            this.progressNomalized = 0f;
            this.autoPlayEnabled = false;
        }

        public void SetIncomePeriod(float newIncomePeriod)
        {
            this.incomePeriod = Mathf.Max(newIncomePeriod, INCOME_PERIOD_MIN);
        }

        public override string ToString()
        {
            return $"StateJson: {this.GetJson()}";
        }
    }
}