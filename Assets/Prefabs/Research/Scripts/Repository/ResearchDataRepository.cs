using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public sealed class ResearchDataRepository : Repository, IUpdateVersionRepository {
        #region Const

        private const string PREF_KEY_RESEARCHES = "RESEARCH_DATA_PREF_KEY";

        private const string PREF_KEY_STATES = "RESEARCH_STATE_KEY";

        #endregion

        private readonly ResearchStatisticsVersionUpdater statisticsVersionUpdater;

        private readonly ExternalResearchVersionUpdater externalVersionUpdater;

        private readonly InternalResearchVersionUpdater internalVersionUpdater;

        private ResearchDataState researchDataState;

        private Dictionary<string, ResearchData> researchDataMap;

        #region Initialize

        public ResearchDataRepository() {
            this.statisticsVersionUpdater = ScriptableObject.Instantiate(
                Resources.Load<ResearchStatisticsVersionUpdater>(
                    "ResearchStatisticsVersionUpdater"
                ));
            this.internalVersionUpdater = ScriptableObject.Instantiate(Resources.Load<InternalResearchVersionUpdater>(
                "InternalResearchVersionUpdater"
            ));
            this.externalVersionUpdater = ScriptableObject.Instantiate(Resources.Load<ExternalResearchVersionUpdater>(
                "ExternalResearchVersionUpdater"
            ));
        }

        protected override void Initialize() {
            base.Initialize();
            this.LoadResearchState();
            this.LoadResearches();
        }

        private void LoadResearchState() {
            this.researchDataState = Storage.GetCustom<ResearchDataState>(PREF_KEY_STATES, null);
            this.internalVersionUpdater.UpdateVersion(ref this.researchDataState);
        }

        private void LoadResearches() {
            var statistics = Storage.GetCustom<ResearchDataStatistics>(PREF_KEY_RESEARCHES, null);
            this.statisticsVersionUpdater.UpdateVersion(ref statistics);
            this.researchDataMap = statistics.researchDataSet.ToDictionary(it => it.id);
        }

        #endregion


        public ResearchData GetData(string id) {
            return this.researchDataMap[id].Clone();
        }

        public bool GetIsUnlocked() {
            return this.researchDataState.isUnlocked;
        }

        public void SetIsUnlocked(bool isUnlocked) {
            this.researchDataState.isUnlocked = isUnlocked;
            Storage.SetCustom(PREF_KEY_STATES, this.researchDataState);
        }

        public void UpdateData(ResearchData researchData) {
            var researchDataId = researchData.id;
            var cloneData = researchData.Clone();
            this.researchDataMap[researchDataId] = cloneData;
            this.SaveResearches();
        }

        public void UpdateDataSet(IEnumerable<ResearchData> researchDataSet) {
            foreach (var researchData in researchDataSet) {
                var researchDataId = researchData.id;
                var cloneData = researchData.Clone();
                this.researchDataMap[researchDataId] = cloneData;
            }

            this.SaveResearches();
        }

        private void SaveResearches() {
            var statistics = new ResearchDataStatistics {
                researchDataSet = this.researchDataMap.Values.ToList()
            };
            Storage.SetCustom(PREF_KEY_RESEARCHES, statistics);
        }

        public IEnumerator OnUpdateVersion(Reference<bool> isUpdated) {
            var isVersionUpdated = this.externalVersionUpdater.UpdateVersion(ref this.researchDataState);
            if (isVersionUpdated) {
                Storage.SetCustom(PREF_KEY_STATES, this.researchDataState);
            }

            isUpdated.value = isVersionUpdated;
            yield break;
        }
    }
}