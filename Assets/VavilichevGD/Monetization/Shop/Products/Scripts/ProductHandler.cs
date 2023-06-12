namespace VavilichevGD.Monetization {
    public abstract class ProductHandler {
        protected readonly Product product;

        public ProductHandler(Product product) {
            this.product = product;
        }

        public abstract void DistributeProduct();
    }
}