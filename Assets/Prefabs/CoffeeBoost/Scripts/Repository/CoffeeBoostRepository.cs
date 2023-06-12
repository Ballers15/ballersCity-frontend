using System.Collections;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public class CoffeeBoostRepository : Repository, IUpdateVersionRepository {

        #region CONSTANTS

        private const string COFFEE_PREF_KEY = "COFFEE_BOOST_PREF_KEY";

        #endregion

        private IVersionUpdater<CoffeeBoostStatistics> internalCoffeeBoostVersionUpdater;
        private IVersionUpdater<CoffeeBoostStatistics> externalCoffeeBoostVersionUpdater;

        private CoffeeBoostStatistics statistics;

        protected override void Initialize() {
            base.Initialize();
            this.internalCoffeeBoostVersionUpdater = Resources.Load<InternalCoffeeBoostVersionUpdater>(
                nameof(InternalCoffeeBoostVersionUpdater)
            );
            this.externalCoffeeBoostVersionUpdater = Resources.Load<ExternalCoffeeBoostVersionUpdater>(
                nameof(ExternalCoffeeBoostVersionUpdater)
            );
            this.LoadFromStorage();
        }


        private void LoadFromStorage() {
            this.statistics = Storage.GetCustom<CoffeeBoostStatistics>(COFFEE_PREF_KEY, null);
            if (this.internalCoffeeBoostVersionUpdater.UpdateVersion(ref this.statistics))
                Storage.SetCustom(COFFEE_PREF_KEY, this.statistics);
        }


        public bool GetIsUnlocked() {
            return this.statistics.isUnlocked;
        }

        public void SetInUnlocked(bool isUnlocked) {
            this.statistics.isUnlocked = isUnlocked;
            Storage.SetCustom(COFFEE_PREF_KEY, this.statistics);
        }

        public IEnumerator OnUpdateVersion(Reference<bool> isUpdated) {
            var isVersionUpdated = this.externalCoffeeBoostVersionUpdater.UpdateVersion(ref this.statistics);
            if (isVersionUpdated)
                Storage.SetCustom(COFFEE_PREF_KEY, this.statistics);

            isUpdated.value = isVersionUpdated;
            yield break;
        }
        
    }
}