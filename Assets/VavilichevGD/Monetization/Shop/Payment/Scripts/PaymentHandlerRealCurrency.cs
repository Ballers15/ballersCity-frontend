using VavilichevGD.Architecture;

namespace VavilichevGD.Monetization {
    public class PaymentHandlerRealCurrency : PaymentHandler {
        
        public override void Pay(Product product, PaymentResultHandler callback) {
            RealPaymentInteractor interactor = Game.GetInteractor<RealPaymentInteractor>();
            interactor.PurchaseProduct(product, callback);
        }

    }
}