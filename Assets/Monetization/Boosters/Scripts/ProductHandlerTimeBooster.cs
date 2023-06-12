using VavilichevGD.Monetization;

namespace SinSity.Monetization
{
    public class ProductHandlerTimeBooster : ProductHandler
    {
        public ProductHandlerTimeBooster(Product product) : base(product)
        {
        }

        public override void DistributeProduct()
        {
            this.product.GetState<ProductStateTimeBooster>().AddBooster();
        }
    }
}