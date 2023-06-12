using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;

namespace SinSity.Monetization {
    [System.Serializable]
    public class ProductStateCase : ProductState {

        public delegate void ProductStateCaseHandler(int casesCount);
        public static event ProductStateCaseHandler OnCasesCountChanged;
        
        public int countCurrent;
        
        public ProductStateCase(string stateJson) : base(stateJson) { }
        public ProductStateCase(ProductInfo info) : base(info) { }

        private const int MAX_CASES_COUNT = 99;

        public static ProductStateCase GetDefault(ProductInfo info) {
            ProductStateCase state = new ProductStateCase(info);
            state.id = state.id;
            state.isPurchased = false;
            state.isViewed = false;
            state.countCurrent = 0;
            return state;
        }
        
        public override string GetStateJson() {
            return JsonUtility.ToJson(this);
        }
        
        public override void SetState(string stateJson) {
            ProductStateCase state = JsonUtility.FromJson<ProductStateCase>(stateJson);
            this.id = state.id;
            this.isPurchased = state.isPurchased;
            this.isViewed = state.isViewed;
            this.countCurrent = state.countCurrent;
            NotifyAboutCaseCountChanged();
        }

        private void NotifyAboutCaseCountChanged() {
            OnCasesCountChanged?.Invoke(countCurrent);
        }

        public override void SetState(ProductState state) {
            ProductStateCase productStateCase = state as ProductStateCase;
            this.id = productStateCase.id;
            this.isPurchased = productStateCase.isPurchased;
            this.isViewed = productStateCase.isViewed;
            this.countCurrent = productStateCase.countCurrent;
            NotifyAboutCaseCountChanged();
        }

        public void AddCase() {
            this.countCurrent = Mathf.Min(countCurrent + 1, MAX_CASES_COUNT);
            Shop.SaveAllProducts();
            CommonAnalytics.LogCaseReceived(id, true, this.countCurrent);
            NotifyAboutCaseCountChanged();
        }

        public void AddCases(int count) {
            this.countCurrent = Math.Min(countCurrent + count, MAX_CASES_COUNT);
            Shop.SaveAllProducts();
            CommonAnalytics.LogCaseReceived(id, true, this.countCurrent);
            NotifyAboutCaseCountChanged();
        }

        public void SpendCase() {
            this.countCurrent = Math.Max(countCurrent - 1, 0);
            Shop.SaveAllProducts();
            NotifyAboutCaseCountChanged();
        }
        
        public static void ApplyRewards(IEnumerable<RewardInfo> rewardInfos)
        {
            foreach (var rewardInfo in rewardInfos)
            {
                var reward = new Reward(rewardInfo);
                reward.Apply("Case", true);
            }
        }
    }
}