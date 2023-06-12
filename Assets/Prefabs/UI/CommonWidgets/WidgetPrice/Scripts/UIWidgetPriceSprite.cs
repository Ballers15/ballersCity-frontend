using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetPriceSprite : UIWidget<UIWidgetPriceSprite.Properties> {

        public void SetPrice(string priceText) {
            if (this.properties.textPrice)
                this.properties.textPrice.text = priceText;

            if (this.properties.tmpPrice)
                this.properties.tmpPrice.text = priceText;
        }

        public void SetVisualAsEnoughMoney() {
            this.properties.imgBackground.sprite = this.properties.spriteEnoughMoney;
        }

        public void SetVisualAsNotEnoughMoney() {
            this.properties.imgBackground.sprite = this.properties.spriteNotEnoughMoney;
        }
        
        [System.Serializable]
        public class Properties : UIProperties {
            public Text textPrice;
            public TextMeshProUGUI tmpPrice;
            [Space]
            public Image imgBackground;
            public Sprite spriteEnoughMoney;
            public Sprite spriteNotEnoughMoney;
        }
    }
}