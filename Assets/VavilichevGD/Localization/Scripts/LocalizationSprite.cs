using System;
using UnityEngine;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationSprite : LocalizationObject {
        [Serializable]
        private class LocalizationImageEntity {
            public SpriteRenderer spriteRenderer;
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
                entity.spriteRenderer.sprite = entity.GetSprite(language);
        }
    }
}