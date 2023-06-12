namespace VavilichevGD.Monetization {

    
    public abstract class PaymentHandler {
        
        public delegate void PaymentResultHandler(Product product, bool success);
       //public abstract event PaymentResultHandler OnPaymentResultsEvent;


        protected const bool FAIL = false;
        protected const bool SUCCESS = true;

        public abstract void Pay(Product product, PaymentResultHandler callback);
    }
}