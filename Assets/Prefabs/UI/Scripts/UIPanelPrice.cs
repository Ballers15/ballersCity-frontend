using SinSity.Tools;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public class UIPanelPrice : UIPanel<UIPanelPriceProperties>
    {
        public virtual void SetPrice(BigNumber price) {
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            var strPrice = price.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            if (price == 0)
            {
                //TODO: Спросить у Андрея, что это значит?
                strPrice = Localization.GetTranslation("ID_GET");
            }

            this.properties.textPrice.text = strPrice;
            RectTransform parent = properties.textPrice.transform.parent as RectTransform;
            parent.Recalculate();
        }

        public virtual void SetPrice(float price)
        {
            this.properties.textPrice.text = price.ToString();
        }
    }
}