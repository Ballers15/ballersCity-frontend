using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public class RealPaymentInteractor : Interactor {

        private RealPaymentBehavior behavior;

        public override bool onCreateInstantly { get; } = false;

        protected override void Initialize() {
            base.Initialize();
            
#if UNITY_ANDROID
            ShopInteractor shopInteractor = Game.GetInteractor<ShopInteractor>();
            if (shopInteractor == null) {
                Logging.LogError($"REAL PAYMENT INTERACTOR: ProductInteractor is not initialized yet. Real payments is not initialized;");
                return;
            }

            Product[] products = shopInteractor.GetAllRealPaymentProducts();
            behavior = new RealPaymentBehaviorUnity(products); // You can change payments behavior here.
#endif
        }

        public bool IsPurchasedProduct(Product product) {
            return behavior.IsPurchasedProduct(product);
        }

        public void PurchaseProduct(Product product, PaymentHandler.PaymentResultHandler callback) {
            behavior.PurchaseProduct(product, callback);
        }

        public string GetPriceOfProduct(Product product) {
            return behavior.GetPriceOfProduct(product);
        }
        
        public string GetPriceOfProduct(ProductInfo info) {
            return behavior.GetPriceOfProduct(info);
        }

        public T GetBehavior<T>() where T : RealPaymentBehavior {
            return (T) behavior;
        }
    }
}