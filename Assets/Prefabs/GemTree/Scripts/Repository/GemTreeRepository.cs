using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo
{
    public sealed class GemTreeRepository : Repository, IUpdateVersionRepository
    {
        #region Const

        private const string TREE_PREF_KEY = "GEM_TREE_PREF_KEY";

        private const string BRANCH_PREF_KEY = "GEM_BRANCH_PREF_KEY";

        #endregion

        private IVersionUpdater<GemTreeStatistics> internalTreeVersionUpdater;

        private IVersionUpdater<GemTreeStatistics> externalTreeVersionUpdater;

        private IVersionUpdater<GemBranchDataSet> branchVersionUpdater;

        private Dictionary<string, GemBranchData> branchDataMap;

        private GemTreeStatistics statistics;

        protected override void Initialize() {
            base.Initialize();
            this.internalTreeVersionUpdater = Resources.Load<InternalGemTreeVersionUpdater>(
                nameof(InternalGemTreeVersionUpdater)
            );
            this.externalTreeVersionUpdater = Resources.Load<ExternalGemTreeVersionUpdater>(
                nameof(ExternalGemTreeVersionUpdater)
            );
            this.branchVersionUpdater = Resources.Load<GemBranchDataVersionUpdater>(
                nameof(GemBranchDataVersionUpdater)
            );
            this.LoadFromStorage();
        }

        private void LoadFromStorage()
        {
            this.statistics = Storage.GetCustom<GemTreeStatistics>(TREE_PREF_KEY, null);
            if (this.internalTreeVersionUpdater.UpdateVersion(ref this.statistics))
            {
                Storage.SetCustom(TREE_PREF_KEY, this.statistics);
            }

            var branchDataSet = Storage.GetCustom<GemBranchDataSet>(BRANCH_PREF_KEY, null);
            if (this.branchVersionUpdater.UpdateVersion(ref branchDataSet))
            {
                Storage.SetCustom(BRANCH_PREF_KEY, branchDataSet);
            }

            this.branchDataMap = branchDataSet.dataSet.ToDictionary(it => it.id);
        }

        public bool GetIsViewed()
        {
            return this.statistics.isViewed;
        }

        public void SetViewed(bool isViewed)
        {
            this.statistics.isViewed = isViewed;
            Storage.SetCustom(TREE_PREF_KEY, this.statistics);
        }

        public int GetTreeLevel()
        {
            return this.statistics.level;
        }

        public void SetTreeLevel(int newLevel)
        {
            this.statistics.level = newLevel;
            Storage.SetCustom(TREE_PREF_KEY, this.statistics);
        }

        public int GetTreeProgress()
        {
            return this.statistics.progress;
        }

        public void SetTreeProgress(int progress)
        {
            this.statistics.progress = progress;
            Storage.SetCustom(TREE_PREF_KEY, this.statistics);
        }

        public bool GetIsUnlocked()
        {
            return this.statistics.isUnlocked;
        }

        public void SetInUnlocked(bool isUnlocked)
        {
            this.statistics.isUnlocked = isUnlocked;
            Storage.SetCustom(TREE_PREF_KEY, this.statistics);
        }

        public IEnumerable<GemBranchData> GetBranchDataSet()
        {
            return this.branchDataMap.Values.Select(it => it.Clone());
        }

        public GemBranchData GetBranchData(string id)
        {
            return this.branchDataMap[id].Clone();
        }

        public void UpdateBranchDataSet(IEnumerable<GemBranchData> branchDataSet)
        {
            foreach (var branchData in branchDataSet)
            {
                this.branchDataMap[branchData.id] = branchData.Clone();
            }

            Storage.SetCustom(BRANCH_PREF_KEY, new GemBranchDataSet (this.branchDataMap));
        }

        public void UpdateBranchData(GemBranchData branchData)
        {
            this.branchDataMap[branchData.id] = branchData.Clone();
            Storage.SetCustom(BRANCH_PREF_KEY, new GemBranchDataSet(this.branchDataMap));
        }

        public IEnumerator OnUpdateVersion(Reference<bool> isUpdated)
        {
            var isVersionUpdated = this.externalTreeVersionUpdater.UpdateVersion(ref this.statistics);
            if (isVersionUpdated)
            {
                Storage.SetCustom(TREE_PREF_KEY, this.statistics);
            }

            isUpdated.value = isVersionUpdated;
            yield break;
        }
    }
}