using System;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Monetization;

namespace SinSity.Monetization
{
    [Serializable]
    public class ProductStateTimeBooster : ProductState {

        public delegate void ProductStateTimeBoosterHandler(int timeBoostersCount);
        public static event ProductStateTimeBoosterHandler OnTimeBoostersCountChanged;
        
        #region Const
        
        private const int MAXX_BOOSTERS_COUNT = 99;

        #endregion

        public int countCurrent;

        public ProductStateTimeBooster(string stateJson) : base(stateJson)
        {
        }

        public ProductStateTimeBooster(ProductInfo info) : base(info)
        {
        }


        public static ProductStateTimeBooster GetDefault(ProductInfoTimeBooster info)
        {
            ProductStateTimeBooster state = new ProductStateTimeBooster(info);
            state.id = info.GetId();
            state.isPurchased = false;
            state.isViewed = false;
            state.countCurrent = 0;
            return state;
        }

        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override void SetState(string stateJson)
        {
            ProductStateCase state = JsonUtility.FromJson<ProductStateCase>(stateJson);
            this.id = state.id;
            this.isPurchased = state.isPurchased;
            this.isViewed = state.isViewed;
            this.countCurrent = state.countCurrent;
            NotifyAboutCountChanged();
        }

        private void NotifyAboutCountChanged() {
            OnTimeBoostersCountChanged?.Invoke(countCurrent);
        }

        public override void SetState(ProductState state)
        {
            ProductStateCase productStateCase = state as ProductStateCase;
            this.id = productStateCase.id;
            this.isPurchased = productStateCase.isPurchased;
            this.isViewed = productStateCase.isViewed;
            this.countCurrent = productStateCase.countCurrent;
            NotifyAboutCountChanged();
        }

        public void AddBooster()
        {
            this.countCurrent = Mathf.Min(countCurrent + 1, MAXX_BOOSTERS_COUNT);
            Shop.SaveAllProducts();
            NotifyAboutCountChanged();
            CommonAnalytics.LogTimeBoosterReceived(id, true, Bank.hardCurrencyCount);
        }

        public void AddBoosters(int count) {
            this.countCurrent = Mathf.Min(countCurrent + count, MAXX_BOOSTERS_COUNT);
            Shop.SaveAllProducts();
            NotifyAboutCountChanged();
            CommonAnalytics.LogTimeBoosterReceived(id, true, Bank.hardCurrencyCount);
        }

        public void SpendBooster()
        {
            if (this.countCurrent <= 0)
            {
                throw new Exception("Can not spend booster!");
            }

            Debug.Log("SPEND TIME BOOSTER!");
            this.countCurrent--;
            Shop.SaveAllProducts();
            NotifyAboutCountChanged();
        }
    }
}