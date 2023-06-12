using SinSity.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;

public class UIPanelPriceGems : UIPanelPrice {

    [SerializeField] private float sizeTextWatch;
    [SerializeField] private float sizeTextWarchGermany;
    [SerializeField] private float sizeTextPrice;
    [Space] 
    [SerializeField] private Image imgIconCurrency;
    [SerializeField] private Sprite spriteHardCurrency;

    [SerializeField] private Sprite spriteADCurrency;

    public void SetPrice(Product product) {
        var isADSPaymmentt = product.info.isADSPayment;

        if (isADSPaymmentt) {
            SetPriceWatch();
            return;
        }


        SetPrice(product.GetPriceToString());
    }

    private void SetPriceWatch() {
        properties.textPrice.text = Localization.GetTranslation("ID_WATCH");
        var isLanguageGerman = Localization.GetCurrentLanguage() == SystemLanguage.German;
        properties.textPrice.fontSize = isLanguageGerman ? (int) sizeTextWarchGermany : (int) sizeTextWatch;
        imgIconCurrency.sprite = spriteADCurrency;
        imgIconCurrency.gameObject.SetActive(true);

        RecalculatetField();
    }

    private void SetPrice(string price) {
        properties.textPrice.text = price.ToString();
        properties.textPrice.fontSize = (int) sizeTextPrice;
        imgIconCurrency.sprite = spriteHardCurrency;
        imgIconCurrency.gameObject.SetActive(false);

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
