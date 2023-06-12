using UnityEngine;
using VavilichevGD.Monetization;

namespace SinSity.Monetization {
	[CreateAssetMenu(fileName = "ProductInfoBooster", menuName = "Monetization/ProductInfo/TimeBooster")]
	public class ProductInfoTimeBooster : ProductInfo {

		[SerializeField] private int m_timeHours;

		public int timeHours => m_timeHours;

		public override ProductHandler CreateHandler(Product product) {
			return new ProductHandlerTimeBooster(product);
		}

		public override ProductState CreateState(string stateJson) {
			return new ProductStateTimeBooster(stateJson);
		}

		public override ProductState CreateDefaultState() {
			return ProductStateTimeBooster.GetDefault(this);
		}
	}
}