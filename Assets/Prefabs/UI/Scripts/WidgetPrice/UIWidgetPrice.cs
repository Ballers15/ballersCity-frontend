using SinSity.Tools;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetPrice : UIWidget<UIWidgetPriceProperties>, IUIWidgetPrice {
        public void SetPriceSoft(BigNumber priceSoft) {
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            string translatedPrice = priceSoft.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            this.properties.SetPriceValue(translatedPrice);

            bool isEnoughCurrency = Bank.isEnoughtSoftCurrency(priceSoft);
            if (isEnoughCurrency)
                this.properties.SetVisualAsEnoughCurrency();
            else
                this.properties.SetVisualAsNotEnoughCurrency();
        }


        public void SetPriceHard(int priceHard) {
            this.properties.SetPriceValue(priceHard.ToString());
            bool isEnoughCurrency = Bank.isEnoughtHardCurrency(priceHard);
            if (isEnoughCurrency)
                this.SetVisualAsEnoughCurrency();
            else
                this.SetVisualAsNotEnoughCurrency();
        }

        public void SetVisualAsEnoughCurrency() {
            this.properties.SetVisualAsEnoughCurrency();
        }

        public void SetVisualAsNotEnoughCurrency() {
            this.properties.SetVisualAsNotEnoughCurrency();
        }
    }
}