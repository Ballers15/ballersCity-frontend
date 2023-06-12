using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using SinSity.Data;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public class ShopRepository : Repository {

        public string[] stateJsons => states.listOfStates.ToArray();
        protected ProductStates states;
        
        protected const string PREF_KEY = "PRODUCTS_STATES";

        
        public ShopRepository() {
            states = ProductStates.empty;
        }

        protected override IEnumerator InitializeRepositoryRoutine() {
            using var request = new GameDataRequest(PREF_KEY);
            
            yield return request.Send(RequestType.GET);
            
            states = request.GetGameData(ProductStates.empty);
            Logging.Log($"PRODUCT REPOSITORY: Loaded from storage");
        }

        public void Save(ProductState[] statesArray) {
            states = new ProductStates(statesArray);
            Save();
        }
        
        private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            using var request = new GameDataRequest(PREF_KEY, states);
            
            yield return request.Send();
            
            isSavingInProcess = false;
            Logging.Log($"PRODUCT REPOSITORY: Saved to storage");
        }

        public override void Save() {
            if (isSavingInProcess) return;
            Coroutines.StartRoutine(SaveAsync());
        }
    }
}