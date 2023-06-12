using SinSity.Tools;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.UI {
    public class UIPanelPriceBuildIdleObject : UIPanelPrice {
        public override void SetPrice(BigNumber price) {
            RectTransform parent = properties.textPrice.transform.parent as RectTransform;

            if (price == 0)
            {
                parent.gameObject.SetActive(false);
                return;
            }
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            var strPrice = price.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            parent.gameObject.SetActive(true);
            this.properties.textPrice.text = strPrice;
        }
    }
}