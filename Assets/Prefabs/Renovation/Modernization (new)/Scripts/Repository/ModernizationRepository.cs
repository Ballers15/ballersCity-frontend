using System;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public class ModernizationRepository : Repository {

        #region CONSTANTS

        private const string DATA_KEY = "MODERNIZATION_DATA";

        #endregion
        
        public ModernizationData data { get; private set; }

        #region INITIALIZE

        protected override void Initialize() {
            base.Initialize();
            this.LoadFromStorage();
        }

        private void LoadFromStorage() {
            this.data = Storage.GetCustom(DATA_KEY, new ModernizationData());
        }

        public override void OnInitialized() {
            this.AdaptFromOldVersion();
        }

        private void AdaptFromOldVersion() {
            var renovationRepository = Game.GetRepository<RenovationRepository>();
            var versionAdapter = new ModernizationVersionAdapter(renovationRepository, this);
            versionAdapter.Adapt();
        }

        #endregion

        
        public override void Save() {
            Storage.SetCustom(DATA_KEY, this.data);
        }

        public void AddScores(int scores) {
            var clampedValue = Math.Max(scores, 0);
            this.data.scores += clampedValue;
            this.Save();
        }

        public void TransferScoresToMultiplier() {
            var scores = this.data.scores;
            var oldMultiplier = this.data.multiplier;
            var addictiveMultiplier = scores / 100f;
            this.data.multiplier = oldMultiplier + addictiveMultiplier;
            this.data.scores = 0;
        }

        public void SetDataValues(float multiplier, int scores, bool isAvailable) {
            this.data.multiplier = multiplier;
            this.data.renovationIndex = Convert.ToInt32(multiplier);
            this.data.scores = scores;
            this.data.isAvailable = isAvailable;
        }
    }
}