using System.Collections.Generic;
using System.Linq;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo
{
    public sealed class HintRepository : Repository
    {
        #region Const

        private const string PREF_KEY = "HINT_STATE";

        #endregion

        private Dictionary<string, string> hintMap;

        private IVersionUpdater<HintStatistics> versionUpdater;

        protected override void Initialize() {
            base.Initialize();
            this.versionUpdater = new HintVersionUpdater();
            this.LoadFromStorage();
        }

        private void LoadFromStorage()
        {
            this.hintMap = new Dictionary<string, string>();
            var hintStatistics = Storage.GetCustom<HintStatistics>(PREF_KEY, null);
            this.versionUpdater.UpdateVersion(ref hintStatistics);
            var keys = hintStatistics.hintJsonKeys;
            var values = hintStatistics.hintJsonValues;
            for (var i = 0; i < keys.Count; i++)
            {
                var id = keys[i];
                var json = values[i];
                this.hintMap[id] = json;
            }
        }

        public string GetState(string id)
        {
            if (this.hintMap.ContainsKey(id))
            {
                return this.hintMap[id];
            }

            return null;
        }

        public void Update(string id, string json)
        {
            this.hintMap[id] = json;
            this.Save();
        }

        public override void Save()
        {
            var keys = this.hintMap.Keys.ToList();
            var values = this.hintMap.Values.ToList();
            var hintStatistics = new HintStatistics
            {
                hintJsonKeys = keys,
                hintJsonValues = values
            };
            Storage.SetCustom(PREF_KEY, hintStatistics);
        }
    }
}