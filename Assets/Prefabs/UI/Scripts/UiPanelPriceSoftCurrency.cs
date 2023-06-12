using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Tools;

namespace SinSity.UI {
    public class UiPanelPriceSoftCurrency : UIPanelPrice {

        [SerializeField] private Image imgIcon;

        public override void SetPrice(BigNumber price) {
            bool free = price == 0;
            imgIcon.gameObject.SetActive(!free);
            base.SetPrice(price);
        }
    }
}