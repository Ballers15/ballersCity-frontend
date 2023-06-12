using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using SinSity.Data;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.UI {
    public class UITutorialRepository : Repository {
        
        public UITutorialState state { get; protected set; }

        protected const string PREF_KEY_STATE = "REPOSITORY_TUTORIAL_STATE";
        
        protected override IEnumerator InitializeRepositoryRoutine() {
            using var request = new GameDataRequest(PREF_KEY_STATE);
            
            yield return request.Send(RequestType.GET);
            
            state = request.GetDownloadedData<UITutorialState>(UITutorialState.GetDefault());
            Logging.Log($"TUTORIAL REPOSITORY: Loaded from storage");
        }

        public override void Save() {
            Coroutines.StartRoutine(SaveAsync());
        }
        
        private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            using var request = new GameDataRequest(PREF_KEY_STATE, state);
            
            yield return request.Send();
            
            isSavingInProcess = false;
            Logging.Log($"TUTORIAL REPOSITORY: Saved to storage");
        }

        public void CompleteStep(int indexLastTutorialStep) {
            state.lastIndexTutorialStep = indexLastTutorialStep;
            Save();
        }

        public void MarkAsComplete() {
            state.isComplete = true;
            Save();
        }
    }
}