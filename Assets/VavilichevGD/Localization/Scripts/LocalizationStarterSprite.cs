using System;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationStarterSprite : MonoBehaviour {
        [Serializable]
        private class Pair {
            public Image img;
            public Sprite spriteDefault;
            public LocalizationValue[] values;

            public bool Contains(SystemLanguage language, out Sprite sprite) {
                foreach (var value in this.values) {
                    if (value.language == language) {
                        sprite = value.sprite;
                        return true;
                    }
                }

                sprite = null;
                return false;
            }
        }

        [Serializable]
        private class LocalizationValue {
            public SystemLanguage language;
            public Sprite sprite;
        }

        [SerializeField] private Pair[] pairs;

        private void Awake() {
            this.Translate();
        }

        private void Translate() {
            var language = Application.systemLanguage;
            foreach (var pair in this.pairs) {
                if (pair.Contains(language, out Sprite sprite))
                    pair.img.sprite = sprite;
                else
                    pair.img.sprite = pair.spriteDefault;
            }
        }
    }
}