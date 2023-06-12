using System.Collections;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public sealed class AirDropRepository : Repository, IUpdateVersionRepository {
        #region Const

        private const string PREF_KEY = "AIR_DROP_PREF_KEY";

        #endregion

        private readonly IVersionUpdater<AirDropStatistics> internalVersionUpdater;

        private readonly IVersionUpdater<AirDropStatistics> externalVersionUpdater;

        private AirDropStatistics statistics;

        public AirDropRepository() {
            this.internalVersionUpdater = Object.Instantiate(Resources.Load<InternalAirDropVersionUpdater>(
                "InternalAirDropVersionUpdater"
            ));
            this.externalVersionUpdater = Object.Instantiate(Resources.Load<ExternalAirDropVersionUpdater>(
                "ExternalAirDropVersionUpdater"
            ));
        }

        protected override void Initialize() {
            base.Initialize();
            this.statistics = Storage.GetCustom<AirDropStatistics>(PREF_KEY, null);
            this.internalVersionUpdater.UpdateVersion(ref this.statistics);
        }


        public AirDropStatistics GetStatistics() {
            return this.statistics.Clone();
        }

        public void SetAirDropEnabled(bool isEnabled) {
            this.statistics.isAirDropEnabled = isEnabled;
            this.Save();
        }

        public void SetLuckyModeEnabled(bool isEnabled) {
            this.statistics.isLuckyModeEnabled = isEnabled;
            this.Save();
        }

        public void SetLuckyIndex(int newLuckyIndex) {
            this.statistics.luckyIndex = newLuckyIndex;
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