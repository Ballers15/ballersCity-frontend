namespace VavilichevGD.Monetization {
    public enum PaymentType {
        SoftCurrency,
        HardCurrency,
        Real,
        ADS,
        Free
    }

    public static class PaymentsHandlerFactory {
        public static PaymentHandler CreatePaymentHandler(Product product) {
            return CreatePaymentHandler(product.info);
        }

        public static PaymentHandler CreatePaymentHandler(ProductInfo info) {
            switch (info.paymentType) {
                case PaymentType.SoftCurrency:
                    return  new PaymentHandlerSoftCurrency();
                case PaymentType.HardCurrency:
                    return new PaymentHandlerHardCurrency();
                case PaymentType.ADS:
                    return new PaymentHandlerADS();
                case PaymentType.Real:
                    return new PaymentHandlerRealCurrency();
                default:
                    return new PaymentHandlerSoftCurrency();
            }
        }
    }
}