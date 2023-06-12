using UnityEngine.Video;
using VavilichevGD.Monetization;

namespace SinSity.Monetization
{
    public class ProductHandlerCase : ProductHandler
    {
        public ProductHandlerCase(Product product) : base(product)
        {
        }

        public override void DistributeProduct()
        {
            var caseState = this.product.GetState<ProductStateCase>();
            caseState.AddCase();
        }
    }
}