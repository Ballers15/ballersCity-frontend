using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public class PaymentHandlerHardCurrency : PaymentHandler {
        
        public override void Pay(Product product, PaymentResultHandler callback) {
            if (!Bank.isEnoughtMoney(product)) {
                callback?.Invoke(product, FAIL);
                Logging.Log("PAYMENT HANDLER HARD CURRENCY: Not enough HARD currency");
                return;
            }
            
            Bank.SpendHardCurrency((int)product.price, this);
            callback?.Invoke(product, SUCCESS);
        }
        
    }
}