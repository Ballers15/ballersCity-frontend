using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationInteractor : Interactor {

        private LocalizationRepository localizationRepository;
        private LocalizationLanguageSettings settings;
        private Dictionary<string, string> entities;

        public delegate void LocalizationEntitiesChanged();
        public event LocalizationEntitiesChanged OnLocalizationEntitiesChanged;

        private const string PATH_SETTINGS = "LocalizationSettings";

        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            this.localizationRepository = this.GetRepository<LocalizationRepository>();
        }

        protected override IEnumerator InitializeRoutineNew() {
            entities = new Dictionary<string, string>();
            
            var language = localizationRepository.GetLanguage();
            var languageCode = this.localizationRepository.GetLanguageCode();
            settings = LoadSettings(languageCode, language);
            entities = LocalizationParser.Parse(settings.tableAsset.text);
            Localization.Initialize(this);
            
            yield return null;
            Resources.UnloadUnusedAssets();
            NotifyAboutNewLanguageSetupped();
            Logging.Log($"LOCALIZATION INTERACTOR: Initialized. Language: {GetLanguageTitle()} Entities count = {entities.Count}");
        }

        private LocalizationLanguageSettings LoadSettings(string languageCode, SystemLanguage language = SystemLanguage.English) {
            LocalizationSettings localizationSettings = Resources.Load<LocalizationSettings>(PATH_SETTINGS);
            if (string.IsNullOrEmpty(languageCode)) {
                languageCode = localizationSettings.ConvertLanguageToCode(language);
                this.localizationRepository.SetLanguage(languageCode);
            }
            LocalizationLanguageSettings localizationLanguageSettings = localizationSettings.GetSettings(languageCode);
            Resources.UnloadUnusedAssets();
            return localizationLanguageSettings;
        }
        
        private void NotifyAboutNewLanguageSetupped() {
            OnLocalizationEntitiesChanged?.Invoke();
        }

        
        public string GetLanguageTitle() {
            return settings.languageTitle;
        }

        
        public void SetLanguage(string languageCode) {
            LocalizationSettings localizationSettings = Resources.Load<LocalizationSettings>(PATH_SETTINGS);

            if (localizationSettings.IsValidLanguage(languageCode)) {
                localizationRepository.SetLanguage(languageCode);
                settings = LoadSettings(languageCode);
                entities = LocalizationParser.Parse(settings.tableAsset.text);
            }

            Resources.UnloadUnusedAssets();
            NotifyAboutNewLanguageSetupped();
        }

      
        public void SwitchToNextLanguage() {
            LocalizationSettings localizationSettings = Resources.Load<LocalizationSettings>(PATH_SETTINGS);
            var languageCodeCurrent = this.localizationRepository.GetLanguageCode();
            var languageCodeNext = localizationSettings.GetNextLanguageCodeOf(languageCodeCurrent);
            SetLanguage(languageCodeNext);
        }

        
        public void SwitchToPreviousLanguage() {
            LocalizationSettings localizationSettings = Resources.Load<LocalizationSettings>(PATH_SETTINGS);
            var languageCodeCurrent = this.localizationRepository.GetLanguageCode();
            var languageCodePrevious = localizationSettings.GetPreviousLanguageCodeOf(languageCodeCurrent);
            SetLanguage(languageCodePrevious);
        }

        
        public string GetTranslation(string key) {
            if (entities.ContainsKey(key))
                return entities[key];
            Logging.LogError($"Cannot find Localization Entity with key {key}. Returned key.");
            return key;
        }

        
        public SystemLanguage GetCurrentLanguage() {
            return localizationRepository.GetLanguage();
        }

        public string GetCurrentLanguageCode() {
            return this.localizationRepository.GetLanguageCode();
        }
    }
}