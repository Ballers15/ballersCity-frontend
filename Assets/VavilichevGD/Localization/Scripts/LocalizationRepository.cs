using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationRepository : Repository {

        protected LocalizationData data;
        public const string PREF_KEY_LOCALIZATION = "LOCALIZATION_DATA";

        protected override void Initialize() {
            base.Initialize();
            this.LoadFromStorage();
        }


        private void LoadFromStorage() {
            data = Storage.GetCustom(PREF_KEY_LOCALIZATION, LocalizationData.GetDefault());
            Logging.Log("LOCALIZATION REPOSITORY: Loaded from the Storage");
        }


        public void SetLanguage(string languageCode) {
            data.languageCode = languageCode;
            Save();
        }

        public override void Save() {
            Storage.SetCustom(PREF_KEY_LOCALIZATION, data);
            Logging.Log("LOCALIZATION REPOSITORY: Saved to the Storage");
        }
        
        public SystemLanguage GetLanguage() {
            return data.language;
        }

        public string GetLanguageCode() {
            return data.languageCode;
        }
    }
}