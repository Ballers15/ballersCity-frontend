using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public sealed class RenovationRepository : Repository {
        #region Const

        private const string PREF_KEY = "RENOVATION_STATE";

        #endregion

        public float points;
        private RenovationStatistics statistics;

        private IVersionUpdater<RenovationStatistics> versionUpdater;

        protected override void Initialize() {
            base.Initialize();
            this.versionUpdater = new RenovationVersionUpdater();
            this.LoadFromStorage();
        }

        private void LoadFromStorage() {
            this.statistics = Storage.GetCustom<RenovationStatistics>(PREF_KEY, null);
            this.versionUpdater.UpdateVersion(ref this.statistics);
        }

        public RenovationStatistics Get() {
            return new RenovationStatistics {
                level = this.statistics.level,
                passedQuestCount = this.statistics.passedQuestCount,
                targetQuestCount = this.statistics.targetQuestCount,
            };
        }

        public void Update(RenovationStatistics statistics) {
            this.statistics = new RenovationStatistics {
                level = statistics.level,
                passedQuestCount = statistics.passedQuestCount,
                targetQuestCount = statistics.targetQuestCount,
            };
            this.Save();
        }

        public override void Save() {
            Storage.SetCustom(PREF_KEY, this.statistics);
        }
    }
}