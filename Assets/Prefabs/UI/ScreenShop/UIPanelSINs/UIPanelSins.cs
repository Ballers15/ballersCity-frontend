using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPanelSins : UIPanel<UIPanelSinsProperties>
    {
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
            properties.widget1.Setup();
            properties.widget2.Setup();
            properties.widget3.Setup();
            isInitialized = true;
        }

        private void Resetup() {
            properties.widget1.Resetup();
            properties.widget2.Resetup();
            properties.widget3.Resetup();
        }

        private void OnEnable() {
            if (isInitialized)
                Resetup();
        }
    }
}


