using System;
using TMPro;
using UnityEngine;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationTextMeshProWithPrefix : LocalizationObject {
        [Serializable]
        private class Pair {
            public TextMeshProUGUI textField;
            public string prefixText;
            public string translationKey;
        }

        [SerializeField] private Pair[] entities;
        
        protected override void UpdateVisual() {
            foreach (Pair entity in entities) {
                string localizedText = $"{entity.prefixText}{Localization.GetTranslation(entity.translationKey)}";
                entity.textField.text = localizedText;
            }
        }
    }
}