using VavilichevGD.Monetization;

namespace SinSity.Monetization {
    public class ProductHandlerGems : ProductHandler {
        public ProductHandlerGems(Product product) : base(product) { }
        public override void DistributeProduct() {
//            ProductInfoGems info = product.info as ProductInfoGems;
//            Bank.AddHardCurrency(info.gemsCount);
        }
    }
}