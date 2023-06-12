using System;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationTextWithPrefix : LocalizationObject {
        [Serializable]
        private class Pair {
            public Text textField;
            [TextArea]
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