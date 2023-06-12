using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public class PaymentHandlerSoftCurrency : PaymentHandler {
        
        public override void Pay(Product product, PaymentResultHandler callback) {
            if (!Bank.isEnoughtMoney(product)) {
                callback?.Invoke(product, FAIL);
                Logging.Log("PAYMENT HANDLER SOFT CURRENCY: Not enough SOFT currency");
                return;
            }
            
            // Bank.SpendSoftCurrency(product.price);
            callback?.Invoke(product, SUCCESS);
        }
        
    }
}