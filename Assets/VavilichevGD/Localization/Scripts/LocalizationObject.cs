using UnityEngine;

namespace VavilichevGD.LocalizationFramework {
    public abstract class LocalizationObject : MonoBehaviour {
        protected virtual void OnEnable() {
            Localization.OnLanguageChanged += OnLanguageChanged;
            if (Localization.isInitialized)
                UpdateVisual();
        }

        protected virtual void OnLanguageChanged() {
            UpdateVisual();
        }

        protected abstract void UpdateVisual();

        protected virtual void OnDisable() {
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }
    }
}