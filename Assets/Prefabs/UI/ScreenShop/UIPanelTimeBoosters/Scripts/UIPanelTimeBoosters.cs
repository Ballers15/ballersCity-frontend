using System;
using System.Collections;
using SinSity.Monetization;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPanelTimeBoosters : UIPanel<UIPanelTimeBoostersProperties> {
        private bool isInitialized;
        
        protected override void Awake() {
            base.Awake();
            Game.OnGameInitialized += OnGameInitialized;    
        }

        private void OnGameInitialized(Game game) {
            Game.OnGameInitialized -= OnGameInitialized;
            SetupInfo();
            gameObject.SetActive(false);
        }

        private void SetupInfo() {
            Product[] products = Shop.GetProducts<ProductInfoTimeBooster>();
            products = Sort(products);
            
            properties.widgetTimeBooster1.Setup(products[0]);
            properties.widgetTimeBooster2.Setup(products[1]);
            properties.widgetTimeBooster3.Setup(products[2]);

            isInitialized = true;
        }

        private Product[] Sort(Product[] products) {
            Array.Sort(products,
                (product1, product2) => (product1.info as ProductInfoTimeBooster).timeHours.CompareTo(
                    (product2.info as ProductInfoTimeBooster).timeHours));
            return products;
        }

        private void OnEnable() {
            if (isInitialized)
                Resetup();
        }

        private void Resetup() {
            properties.widgetTimeBooster1.Resetup();
            properties.widgetTimeBooster2.Resetup();
            properties.widgetTimeBooster3.Resetup();
        }
    }
}