using System.Collections.Generic;
using SinSity.Core;
using SinSity.Monetization;
using SinSity.Services;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization
{
    public static class Shop {
        public static bool isInitialized => interactor != null;
        private static ShopInteractor interactor;

        public static void Initialize(ShopInteractor _interactor)
        {
            interactor = _interactor;
            Logging.Log("SHOP: Initialized");
        }

        public static void PurchaseProduct(Product product, ProductPurchaseHandler callback = null)
        {
            #if DEBUG
            if (product.info.isRealPayment) {
                ForcePurchase(product, callback);
                return;
            }
            #endif
            
            interactor.Purchase(product, (it, success) =>
            {
                callback.Invoke(it, success);
                CommonAnalytics.LogProductPurchased(
                    it.id,
                    it.info.paymentType.ToString(),
                    Bank.hardCurrencyCount,
                    success
                );
            });
        }

        public static void ForcePurchase(Product product, ProductPurchaseHandler callback = null)
        {
            interactor.ForcePurchase(product, callback);
        }

        public static Product[] GetAllProducts()
        {
            return interactor.GetAllProducts();
        }

        public static Product[] GetAllRealPaymentProducts()
        {
            return interactor.GetAllRealPaymentProducts();
        }

        public static Product GetProduct(string productId)
        {
            return interactor.GetProduct(productId);
        }

        public static Product[] GetProducts<T>() where T : ProductInfo
        {
            return interactor.GetProducts<T>();
        }

        public static void SaveAllProducts()
        {
            interactor.SaveAllProducts();
        }
        
        public static bool HasAnyCases() {
            Product[] cases = GetProducts<ProductInfoCase>();
            foreach (Product product in cases) {
                ProductStateCase state = product.GetState<ProductStateCase>();
                if (state.countCurrent > 0)
                    return true;
            }

            return false;
        }

        public static bool HasAnyBoosters() {
            Product[] boosters = GetProducts<ProductInfoTimeBooster>();
            foreach (Product product in boosters) {
                ProductStateTimeBooster state = product.GetState<ProductStateTimeBooster>();
                if (state.countCurrent > 0)
                    return true;
            }

            return false;
        }
    }
}