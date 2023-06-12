using System;
using System.Linq;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationSettings : ScriptableObject {

        [SerializeField] private LocalizationLanguageSettings[] languageSettings;

        public LocalizationLanguageSettings GetSettings(SystemLanguage language) {
            foreach (LocalizationLanguageSettings languageSetting in languageSettings) {
                if (languageSetting.language == language)
                    return languageSetting;
            }
            throw new Exception($"There is no language settings with language: {language}");
        }

        public LocalizationLanguageSettings GetSettings(string languageCode) {
            foreach (LocalizationLanguageSettings languageSetting in languageSettings) {
                if (languageSetting.code == languageCode)
                    return languageSetting;
            }
            throw new Exception($"There is no language settings with language: {languageCode}");
        }

        public string ConvertLanguageToCode(SystemLanguage language) {
            return this.languageSettings.First(settings => settings.language == language).code;
        }

        public bool IsValidLanguage(string languageCode) {
            foreach (LocalizationLanguageSettings languageSetting in languageSettings) {
                if (languageSetting.code == languageCode)
                    return true;
            }

            Logging.LogError($"There is no language settings with language {languageCode}");
            return false;
        }

        public string GetNextLanguageCodeOf(string languageCode) {
            int indexNext = 0;
            for (int i = 0; i < languageSettings.Length; i++) {
                if (languageSettings[i].code == languageCode) {
                    indexNext = i + 1;
                    if (indexNext >= languageSettings.Length)
                        indexNext = 0;
                    return languageSettings[indexNext].code;
                }
            }
            throw new Exception($"There is no language settings with language: {languageCode}");
        }

        public string GetPreviousLanguageCodeOf(string languageCode) {
            int indexPrevious = 0;
            for (int i = 0; i < languageSettings.Length; i++) {
                if (languageSettings[i].code == languageCode) {
                    indexPrevious = i - 1;
                    if (indexPrevious <= 0)
                        indexPrevious = languageSettings.Length - 1;
                    return languageSettings[indexPrevious].code;
                }
            }
            throw new Exception($"There is no language settings with language: {languageCode}");
        }
        
        #if UNITY_EDITOR
        public void UpdateAllTables() {
            foreach (LocalizationLanguageSettings localizationLanguageSettingse in languageSettings) {
                localizationLanguageSettingse.UpdateSpreadsheetsInEditorMode();
            }
        }
        #endif
    }
}