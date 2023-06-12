using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;


namespace SinSity.UI {
    public class UIPanelPriceShopItem : UIPanelPrice {

        [SerializeField] private float sizeTextWatch;
        [SerializeField] private float sizeTextWatchGerman;
        [SerializeField] private float sizeTextGet;
        [SerializeField] private float sizeTextPrice;
        [Space]
        [SerializeField] private Image imgIconCurrency;
        [SerializeField] private Sprite spriteHardCurrency;
        [SerializeField] private Sprite spriteADCurrency;

        public void SetPrice(Product product, bool enableGet) {
            var isADSPaymmentt = product.info.isADSPayment;
            if (enableGet) {
                SetPriceGet();
                return;
            }

            if (isADSPaymmentt) {
                SetPriceWatch();
                return;
            }


            SetPrice(product.price);
        }

        private void SetPriceWatch() {
            properties.textPrice.text = Localization.GetTranslation("ID_WATCH");
            var isLanguageGerman = Localization.GetCurrentLanguage() == SystemLanguage.German;
            properties.textPrice.fontSize = isLanguageGerman ? (int) sizeTextWatchGerman : (int) sizeTextWatch;
            imgIconCurrency.sprite = spriteADCurrency;
            imgIconCurrency.gameObject.SetActive(true);

            RecalculatetField();
        }

        private void SetPriceGet() {
            properties.textPrice.text = Localization.GetTranslation("ID_GET");
            properties.textPrice.fontSize = (int) sizeTextGet;
            imgIconCurrency.gameObject.SetActive(false);

            RecalculatetField();
        }

        public override void SetPrice(float price) {
            properties.textPrice.text = price.ToString();
            properties.textPrice.fontSize = (int) sizeTextPrice;
            imgIconCurrency.sprite = spriteHardCurrency;
            imgIconCurrency.gameObject.SetActive(true);

            RecalculatetField();
        }

        private void OnEnable() {
            RecalculatetField();
        }

        private void RecalculatetField() {
            RectTransform parentRectTransform = properties.textPrice.transform.parent as RectTransform;
            parentRectTransform.RecalculateWithHorizontalFitterInside(ContentSizeFitter.FitMode.PreferredSize);
        }
    }
}