using SinSity.Monetization;
using UnityEngine;
using VavilichevGD.Monetization;

[CreateAssetMenu(fileName = "ProductGemsInfo", menuName = "Monetization/ProductInfo/Gems")]
public class ProductInfoGems : ProductInfo {

	[Space, SerializeField] private int m_gemsCount;

	public int gemsCount => m_gemsCount;

	public override ProductHandler CreateHandler(Product product) {
		return new ProductHandlerGems(product);
	}

	public override ProductState CreateState(string stateJson) {
		return new ProductStateGems(stateJson);
	}

	public override ProductState CreateDefaultState() {
		 return new ProductStateGems(this);
	}
}
