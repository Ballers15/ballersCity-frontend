using SinSity.Core;
using SinSity.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace IdleClicker.UI {
	public class UIPanelCases : UIPanel<UIPanelCasesProperties> {

		private bool isInitialized;

		protected override void Awake() {
			base.Awake();
			Game.OnGameInitialized += OnGameInitialized;    
		}

		private void OnGameInitialized(Game game) {
			Game.OnGameInitialized -= OnGameInitialized;
			Setup();
			gameObject.SetActive(false);
		}

		private void Setup() {
			Product[] products = Shop.GetProducts<ProductInfoCase>();
			properties.widgetCase1.Setup(products[0]);
			properties.widgetCase2.Setup(products[1]);
			properties.widgetCase3.Setup(products[2]);
			isInitialized = true;
		}

		private void Resetup() {
			properties.widgetCase1.Resetup();
			properties.widgetCase2.Resetup();
			properties.widgetCase3.Resetup();
		}

		private void OnEnable() {
			if (isInitialized)
				Resetup();
		}
	}
}