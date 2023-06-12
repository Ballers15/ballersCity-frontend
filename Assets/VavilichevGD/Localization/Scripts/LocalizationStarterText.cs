using System;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Tools;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationStarterText : MonoBehaviour {
       
        [Serializable]
        private class Pair {
            public Text textField;
            public string defaultText;
            public LocalizationValue[] values;

            public bool Contains(string languageCode, out string text) {
                foreach (var value in this.values) {
                    if (value.languageCode == languageCode) {
                        text = value.text;
                        return true;
                    }
                }

                text = null;
                return false;
            }
        }

        [Serializable]
        private class LocalizationValue {
            public string languageCode;
            public string text;
        }

        [SerializeField] private Pair[] pairs;

        private void Awake() {
            this.Translate();
        }

        private void Translate() {
            var data = Storage.GetCustom(LocalizationRepository.PREF_KEY_LOCALIZATION, LocalizationData.GetDefault());
            var languageCode = data.languageCode;
            foreach (var pair in this.pairs) {
                if (pair.Contains(languageCode, out string text))
                    pair.textField.text = text;
                else
                    pair.textField.text = pair.defaultText;
            }
        }
        
    }
}