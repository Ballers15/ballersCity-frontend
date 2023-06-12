using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using SinSity.Data;
using VavilichevGD.Meta;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public class QuestRepository : Repository {

        public string[] stateJsons => states.listOfStates.ToArray();
        protected QuestStates states;
        
        
        public QuestRepository() {
            states = QuestStates.empty;
        }

        protected const string PREF_KEY = "QUEST_STATES";

        public void Save(QuestState[] statesArray) {
            states = new QuestStates(statesArray);
            Save();
        }
        
        protected override IEnumerator InitializeRepositoryRoutine() {
            using var request = new GameDataRequest(PREF_KEY);
            
            yield return request.Send(RequestType.GET);
            
            states = request.GetGameData(QuestStates.empty);
            Logging.Log("QUEST REPOSITORY: Loaded from the Storage");
        }

        public override void Save() {
            if (isSavingInProcess) return;
            Coroutines.StartRoutine(SaveAsync());
        }
        
        private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            using var request = new GameDataRequest(PREF_KEY, states);
            
            yield return request.Send();
            
            isSavingInProcess = false;
            Logging.Log("QUEST REPOSITORY: Saved to the Storage");
        }
    }
}
