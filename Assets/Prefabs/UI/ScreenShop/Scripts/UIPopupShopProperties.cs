using UnityEngine;

namespace SinSity.UI {
    [System.Serializable]
    public class UIPopupShopProperties : UIProperties {
        public UIWidgetShopButton btnWatchADS;
        public UIWidgetShopButton btnBuyForGems;
        public UIWidgetShopButton btnGet;
        public UIWidgetShopButton btnBuyForReal;
        [Space] 
        public GameObject fxTimeBooster;

        [Space] 
        public RectTransform rectPanelCases;
        public RectTransform rectContent;
    }
}