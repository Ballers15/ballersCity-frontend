using UnityEngine.Events;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetShopButton : UIWidget<UIWidgetShopButtonProperties> {

        public void SetPrice(string pricetText) {
            if (properties.canSetPrice)
                properties.textPrice.text = pricetText;
        }

        public void AddListener(UnityAction callback) {
            properties.btn.onClick.AddListener(callback);
        }

        public void RemoveListener(UnityAction callback) {
            properties.btn.onClick.RemoveListener(callback);
        }
    }
}