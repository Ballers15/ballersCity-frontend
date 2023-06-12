using System;
using UnityEngine;
using UnityEngine.UI;


namespace VavilichevGD.LocalizationFramework {
    public class LocalizationImage : LocalizationObject {

        [Serializable]
        private class LocalizationImageEntity {
            public Image imgField;
            public Sprite spriteDefault;
            public LocalizationSpriteEntity[] localizationImages;

            public Sprite GetSprite(SystemLanguage language) {
                foreach (LocalizationSpriteEntity localizationImagePair in localizationImages) {
                    if (localizationImagePair.language == language)
                        return localizationImagePair.sprite;
                }

                return spriteDefault;
            }
        }

        [SerializeField] private LocalizationImageEntity[] entities;
        
        protected override void UpdateVisual() {
            SystemLanguage language = Localization.GetCurrentLanguage();
            foreach (LocalizationImageEntity entity in entities)
                entity.imgField.sprite = entity.GetSprite(language);
        }
    }
}