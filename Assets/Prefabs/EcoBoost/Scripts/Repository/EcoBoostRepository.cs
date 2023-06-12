using System.Collections;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public sealed class EcoBoostRepository : Repository, IUpdateVersionRepository {
        #region Const

        private const string PREF_KEY = "ECO_BOOST";

        #endregion

        private readonly IVersionUpdater<EcoBoostStatistics> internalVersionUpdater;

        private readonly IVersionUpdater<EcoBoostStatistics> externalVersionUpdater;

        private EcoBoostStatistics statistics;

        public EcoBoostRepository() {
            this.internalVersionUpdater = ScriptableObject.Instantiate(Resources.Load<InternalEcoBoostVersionUpdater>(
                "InternalEcoBoostVersionUpdater"
            ));
            this.externalVersionUpdater = ScriptableObject.Instantiate(Resources.Load<ExternalEcoBoostVersionUpdater>(
                "ExternalEcoBoostVersionUpdater"
            ));
        }

        protected override void Initialize() {
            base.Initialize();
            this.LoadFromStorage();
        }

        private void LoadFromStorage() {
            this.statistics = Storage.GetCustom<EcoBoostStatistics>(PREF_KEY, null);
            this.internalVersionUpdater.UpdateVersion(ref this.statistics);
#if DEBUG
            Debug.Log("ECO DATA UNLOCKED? " + this.statistics.isUnlocked);
#endif
        }

        public EcoBoostStatistics Get() {
            return this.statistics.Clone();
        }

        public void Update(EcoBoostStatistics statistics) {
            this.statistics = statistics.Clone();
            this.Save();
        }

        public override void Save() {
            Storage.SetCustom(PREF_KEY, this.statistics);
        }

        public IEnumerator OnUpdateVersion(Reference<bool> isUpdated) {
            var isVersionUpdated = this.externalVersionUpdater.UpdateVersion(ref this.statistics);
            if (isVersionUpdated) {
                this.Save();
            }

            isUpdated.value = isVersionUpdated;
            yield break;
        }
    }
}