using System;
using System.Collections.Generic;
using SinSity.Meta.Rewards;
using SinSity.Monetization;
using VavilichevGD.Monetization;
using ProductInfo = VavilichevGD.Monetization.ProductInfo;

namespace SinSity.Core
{
    public abstract class ProductInfoCase : ProductInfo
    {
        public static event Action<ProductInfoCase> OnProductCaseOpenedEvent;

        public static void NotifyOnProductCaseOpened(ProductInfoCase productInfoCase)
        {
            OnProductCaseOpenedEvent?.Invoke(productInfoCase);
        }

        public override ProductHandler CreateHandler(Product product)
        {
            return new ProductHandlerCase(product);
        }

        public override ProductState CreateState(string stateJson)
        {
            return new ProductStateCase(stateJson);
        }

        public override ProductState CreateDefaultState()
        {
            return ProductStateCase.GetDefault(this);
        }

        public abstract IEnumerable<RewardInfoEcoClicker> GetRewardInfoSet();
    }
}