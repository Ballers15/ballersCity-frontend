using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIWidgetPriceProperties : UIProperties {
        [SerializeField] private Text textPriceValue;
        [SerializeField] private Image imgBackground;
        [SerializeField] private Image imgHighlight;
        [SerializeField] private Image imgPriceBackground;
        [Space] 
        [SerializeField] private Sprite spriteBackgroundEnoughCurrency;
        [SerializeField] private Sprite spriteBackgroundNotEnoughCurrency;
        [SerializeField] private Sprite spriteHighlightEnoughCurrency;
        [SerializeField] private Sprite spriteHighlightNotEnoughCurrency;
        [SerializeField] private Sprite spritePriceBackgroundEnoughCurrency;
        [SerializeField] private Sprite spritePriceBackgroundNotEnoughCurrency;

        public void SetPriceValue(string priceValue) {
            textPriceValue.text = priceValue;
        }

        public void SetVisualAsEnoughCurrency() {
            SetVisual(spriteBackgroundEnoughCurrency, spriteHighlightEnoughCurrency, spritePriceBackgroundEnoughCurrency);
        }

        private void SetVisual(Sprite spriteBackground, Sprite spriteHighlight, Sprite spritePriceBackground) {
            imgBackground.sprite = spriteBackground;
            imgHighlight.sprite = spriteHighlight;
            imgPriceBackground.sprite = spritePriceBackground;
        }

        public void SetVisualAsNotEnoughCurrency() {
            SetVisual(spriteBackgroundNotEnoughCurrency, spriteHighlightNotEnoughCurrency, spritePriceBackgroundNotEnoughCurrency);
        }
    }
}