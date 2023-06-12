using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using SinSity.Data;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Core {
    public sealed class IdleObjectsRepository : Repository {
        private const string PREF_KEY_STATES = "IDLE_OBJECT_STATES";

        public IdleObjectStates states { get; private set; }

        protected override IEnumerator InitializeRepositoryRoutine() {
            using var request = new GameDataRequest(PREF_KEY_STATES);
            
            yield return request.Send(RequestType.GET);
            
            states = request.GetGameData(new IdleObjectStates());
            Logging.Log($"IDLE OBJECTS REPOSITORY: Loaded from storage.");
        }

        private void LoadFromStorage() {
            this.states = Storage.GetCustom(PREF_KEY_STATES, new IdleObjectStates());
            Logging.Log($"IDLE OBJECTS REPOSITORY: Loaded from storage.");
        }

        public void Save(IdleObjectState[] statesArray) {
            states.SetStates(statesArray);
            Save();
            Storage.SetCustom(PREF_KEY_STATES, states);
        }

        public override void Save() {
            if (isSavingInProcess) return;
            Coroutines.StartRoutine(SaveAsync());
        }

        private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            using var request = new GameDataRequest(PREF_KEY_STATES, states);
            
            yield return request.Send();
            
            isSavingInProcess = false;
            Logging.Log($"IDLE OBJECTS REPOSITORY: Saved to storage: {states}");
        }
    }
}