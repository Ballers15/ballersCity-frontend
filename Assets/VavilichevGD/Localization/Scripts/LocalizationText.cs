﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationText : LocalizationObject {
        
        [Serializable]
        private class Pair {
            public Text textField;
            public string translationKey;
        }

        [SerializeField] private Pair[] entities;
        
        protected override void UpdateVisual() {
            foreach (Pair entity in entities)
                entity.textField.text = Localization.GetTranslation(entity.translationKey);
        }
    }
}