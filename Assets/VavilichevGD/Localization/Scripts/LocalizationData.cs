using System;
using UnityEngine;

namespace VavilichevGD.LocalizationFramework {
    [Serializable]
    public class LocalizationData {
        public SystemLanguage language;
        public string languageCode;

        public static LocalizationData GetDefault() {
            LocalizationData data = new LocalizationData();
            data.language = GetLanguageDefault();
            data.languageCode = "";
            return data;
        }

        private static SystemLanguage GetLanguageDefault() {
            SystemLanguage systemLanguage = Application.systemLanguage;
            switch (systemLanguage) {
                /*case SystemLanguage.Russian:
                    return systemLanguage;
                case SystemLanguage.Spanish:
                    return systemLanguage;
                case SystemLanguage.German:
                    return systemLanguage;*/
                default:
                    return SystemLanguage.English;
            }
        }
    }
}