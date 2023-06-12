using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using SinSity.Data;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public sealed class TutorialRepository : Repository {
        #region Const

        private const string PREF_KEY = "TUTORIAL_STATE";

        #endregion

        private TutorialStatistics tutorialStatistics;
        private IVersionUpdater<TutorialStatistics> versionUpdater;

        protected override IEnumerator InitializeRepositoryRoutine() {
            versionUpdater = new TutorialVersionUpdater();
            using var request = new GameDataRequest(PREF_KEY);
            
            yield return request.Send(RequestType.GET);
            
            tutorialStatistics = request.GetGameData<TutorialStatistics>(null);
            if (versionUpdater.UpdateVersion(ref tutorialStatistics)) {
                Save();
            }
        }

        public void Update(TutorialStatistics tutorialStatistics) {
            this.tutorialStatistics = tutorialStatistics.Clone();
            Save();
        }

        public override void Save() {
            Coroutines.StartRoutine(SaveAsync());
        }
        
        private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            using var request = new GameDataRequest(PREF_KEY, tutorialStatistics);
            
            yield return request.Send();
            
            isSavingInProcess = false;
        }

        public TutorialStatistics GetStatistics() {
            return tutorialStatistics.Clone();
        }
    }
}