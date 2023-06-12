using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public class ADSRepository : Repository {

        public ADSState stateCurrent { get; protected set; }
        
        protected const string PREF_KEY_ADS_STATE = "ADS_REPOSITORY_DATA";

        protected override void Initialize() {
            base.Initialize();
            this.LoadFromStorage();
        }

        private void LoadFromStorage() {
            stateCurrent = Storage.GetCustom(PREF_KEY_ADS_STATE, ADSState.GetDefault());
            Logging.Log("ADS REPOSITORY: Loaded from the Storage");
        }

        public override void Save() {
            Storage.SetCustom(PREF_KEY_ADS_STATE, stateCurrent);
            Logging.Log("ADS REPOSITORY: Saved to the Storage");
        }
        
        public void ActivateADS() {
            stateCurrent.isActive = true;
            this.Save();
        }

        public void DeactivateADS() {
            stateCurrent.isActive = false;
            this.Save();
        }
    }
}