using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.LocalizationFramework {
    public static class Localization {
        public static bool isInitialized => interactor != null;
        private static LocalizationInteractor interactor;

        public delegate void LanguageChangeHandler();
        public static event LanguageChangeHandler OnLanguageChanged;
        
        public static void Initialize(LocalizationInteractor _interactor) {
            interactor = _interactor;
            interactor.OnLocalizationEntitiesChanged += OnLocalizationEntitiesChanged;
            Logging.Log("LOCALIZATION: Initialized");
        }

        private static void OnLocalizationEntitiesChanged() {
            OnLanguageChanged?.Invoke();
        }

        public static string GetTranslation(string key) {
            return interactor.GetTranslation(key);
        }

        public static void SwitchToNextLanguage() {
            interactor.SwitchToNextLanguage();
        }

        public static void SwitchToPreviousLanguage() {
            interactor.SwitchToPreviousLanguage();
        }

        public static string GetLanguageTitle() {
            return interactor.GetLanguageTitle();
        }

        public static SystemLanguage GetCurrentLanguage() {
            return interactor.GetCurrentLanguage();
        }

        public static string GetCurrentLanguageCode() {
            return interactor.GetCurrentLanguageCode();
        }

    }
}
