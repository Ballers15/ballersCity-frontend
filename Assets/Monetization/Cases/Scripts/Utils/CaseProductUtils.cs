using SinSity.Core;
using VavilichevGD.Monetization;

namespace SinSity.Monetization
{
    public static class CaseProductUtils
    {
        public static bool HasNextPriorityCase(out Product nextCase)
        {
            var products = Shop.GetProducts<ProductInfoCase>();
            Product nextSteelCase = null;
            Product nextSimpleCase = null;
            foreach (var product in products)
            {
                var caseState = product.GetState<ProductStateCase>();
                if (caseState.countCurrent <= 0)
                {
                    continue;
                }

                var info = product.GetInfo<ProductInfoCase>();
                if (info is ProductInfoGoldCase)
                {
                    nextCase = product;
                    return true;
                }

                if (info is ProductInfoSteelCase)
                {
                    if (nextSteelCase == null)
                    {
                        nextSteelCase = product;
                    }
                }

                if (info is ProductInfoSimpleCase)
                {
                    if (nextSimpleCase == null)
                    {
                        nextSimpleCase = product;
                    }
                }
            }

            if (nextSteelCase != null)
            {
                nextCase = nextSteelCase;
                return true;
            }

            if (nextSimpleCase != null)
            {
                nextCase = nextSimpleCase;
                return true;
            }

            nextCase = null;
            return false;
        }
    }
}