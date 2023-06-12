using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPanelGems : UIPanel<UIPanelGemsProperties> {

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
            Product[] products = Shop.GetProducts<ProductInfoGems>();
            
            properties.widgetGems1.Setup(products[0]);
            properties.widgetGems2.Setup(products[1]);
            properties.widgetGems3.Setup(products[2]);
            properties.widgetGems4.Setup(products[3]);

            isInitialized = true;
        }
    }
}