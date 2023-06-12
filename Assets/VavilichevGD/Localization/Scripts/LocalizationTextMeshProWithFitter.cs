using System;
using SinSity.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationTextMeshProWithFitter : LocalizationObject {
        
        [Serializable]
        private class Pair {
            public TextMeshProUGUI textField;
            public string translationKey;
        }
        
        [SerializeField] private Pair[] entities;
        
        protected override void UpdateVisual() {
            foreach (Pair entity in entities) {
                entity.textField.text = Localization.GetTranslation(entity.translationKey);
                RectTransform parent = entity.textField.transform.parent as RectTransform;
                parent.RecalculateWithHorizontalFitterInside(ContentSizeFitter.FitMode.PreferredSize);                
            }
        }
    }
}