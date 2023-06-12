using System;
using TMPro;
using UnityEngine;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationTextMeshPro : LocalizationObject {
        
        [Serializable]
        private class Pair {
            public TextMeshProUGUI textField;
            public string translationKey;
        }

        [SerializeField] private Pair[] entities;
        
        protected override void UpdateVisual() {
            foreach (Pair entity in entities)
                entity.textField.text = Localization.GetTranslation(entity.translationKey);
        }
    }
}