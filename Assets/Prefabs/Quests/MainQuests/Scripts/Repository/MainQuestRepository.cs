using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using Orego.Util;
using SinSity.Data;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public sealed class MainQuestRepository : Repository, IUpdateVersionRepository {
        #region Const

        private const string PREF_KEY_STATE = "MAIN_QUEST_STATE";
        private const string PREF_KEY_DATA = "MAIN_QUEST_DATA";

        #endregion

        private readonly IVersionUpdater<MainQuestData> internalVersionUpdater;
        private readonly IVersionUpdater<MainQuestData> externalVersionUpdater;
        private MainQuestStatisitcs questStatisitcs;
        private MainQuestData mainQuestData;

        public MainQuestRepository() {
            this.internalVersionUpdater = ScriptableObject.Instantiate(Resources.Load<InternalMainQuestVersionUpdater>(
                "InternalMainQuestVersionUpdater"
            ));
            this.externalVersionUpdater = ScriptableObject.Instantiate(Resources.Load<ExternalMainQuestVersionUpdater>(
                "ExternalMainQuestVersionUpdater"
            ));
        }

        protected override IEnumerator InitializeRepositoryRoutine() {
            using var statisticsRequest = new GameDataRequest(PREF_KEY_STATE);
            using var dataRequest = new GameDataRequest(PREF_KEY_DATA);
            
            yield return statisticsRequest.Send(RequestType.GET);
            yield return dataRequest.Send(RequestType.GET);
            
            questStatisitcs = statisticsRequest.GetGameData<MainQuestStatisitcs>(null);
            mainQuestData = dataRequest.GetGameData<MainQuestData>(null);
            internalVersionUpdater.UpdateVersion(ref mainQuestData);
        }

        public void UpdateStatistics(MainQuestStatisitcs mainQuestStatisitcs) {
            questStatisitcs = mainQuestStatisitcs.Clone();
            Coroutines.StartRoutine(SaveQuestStatisticsRoutine());
        }

        public MainQuestStatisitcs GetStatistics() {
            return questStatisitcs.Clone();
        }

        public bool HasStatistics(out MainQuestStatisitcs statisitcs) {
            if (questStatisitcs == null) {
                statisitcs = null;
                return false;
            }

            statisitcs = GetStatistics();
            return true;
        }

        public bool GetIsUnlocked() {
            return mainQuestData.isUnlocked;
        }

        public void SetIsUnlocked(bool isUnlocked) {
            mainQuestData.isUnlocked = isUnlocked;
            Coroutines.StartRoutine(SaveQuestDataRoutine());
        }

        private IEnumerator SaveQuestDataRoutine() {
            using var request = new GameDataRequest(PREF_KEY_DATA, mainQuestData);
            yield return request.Send();
        }
        
        private IEnumerator SaveQuestStatisticsRoutine() {
            using var request = new GameDataRequest(PREF_KEY_STATE, questStatisitcs);
            yield return request.Send();
        }

        public IEnumerator OnUpdateVersion(Reference<bool> isUpdated) {
           
            var isVersionUpdated = externalVersionUpdater.UpdateVersion(ref mainQuestData);
            if (isVersionUpdated) {
                yield return SaveQuestDataRoutine();
            }
            isUpdated.value = isVersionUpdated;
            yield break;
        }
    }
}