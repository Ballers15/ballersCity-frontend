using System;
using System.Collections;
using System.Collections.Generic;
using Orego.Util;
using SinSity.Core;
using SinSity.Repo;
using SinSity.Tools;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Domain
{
    public sealed class GemTreeStateInteractor : Interactor
    {
        #region Const

        private const string GEM_TREE_INFO_RESOURCE_PATH = "GemTreeInfo_KIRILL";
        private const string GEM_TREE_INFO_RESOURCE_PATH_TEST = "GemTreeInfo_Test";

        private const bool TEST = false;

        private const int ONE = 1;

        private const int ZERO = 0;

        #endregion

        #region Event

        public event Action<object> OnTreeUnlockedEvent;

        public event Action<object> OnTreeViewedEvent;
        
        public event Action<object, int> OnCurrentLevelChangedEvent;

        public event Action<object, int> OnCurrentProgressChangedEvent;

        public event Action<object, GemBranchObject> OnBranchObjectOpenedEvent;
        
        #endregion

        public bool isTreeUnlocked { get; private set; }
        
        public bool isTreeViewed { get; private set; }

        public int currentLevelIndex { get; private set; }

        public int currentProgress { get; private set; }

        public GemTreeInfo treeInfo { get; private set; }

        private new GemTreeRepository repository;

        private GemTreeBranchDataInteractor branchDataInteractor;

        public override bool onCreateInstantly { get; } = true;

        #region Initialize

        protected override void Initialize() {
            base.Initialize();
            this.repository = this.GetRepository<GemTreeRepository>();
            string path = TEST ? GEM_TREE_INFO_RESOURCE_PATH_TEST : GEM_TREE_INFO_RESOURCE_PATH;
            this.treeInfo = Resources.Load<GemTreeInfo>(path);
            this.Setup();
        }

        private void Setup()
        {
            this.isTreeUnlocked = this.repository.GetIsUnlocked();
            this.isTreeViewed = this.repository.GetIsViewed();
            this.currentLevelIndex = this.repository.GetTreeLevel();
            this.currentProgress = this.repository.GetTreeProgress();
        }

        public override void OnInitialized()
        {
            this.branchDataInteractor = this.GetInteractor<GemTreeBranchDataInteractor>();
        }


        #endregion

        #region UpgradeTree

        public bool CanUpgradeTree()
        {
            return this.isTreeUnlocked && !this.IsLastLevel() && this.IsEnoughSoftCurrencyForUpgrade();
        }

        public void UpgradeTree(object sender)
        {
            if (!this.CanUpgradeTree())
                throw new Exception("Can not upgrade tree!");

            var currentUpgradePrice = this.GetCurrentUpgradePrice();
            Bank.SpendSoftCurrency(currentUpgradePrice, this);
            var maxProgress = this.GetMaxProgress();
            if (this.currentProgress + ONE < maxProgress)
                this.IncrementProgress(sender);
            else
                this.IncrementLevel(sender);

            GemTreeAnalytics.LogTreeUpgradeBought(this.currentProgress, this.currentLevelIndex);
        }

        private void IncrementProgress(object sender)
        {
            this.currentProgress++;
            this.repository.SetTreeProgress(this.currentProgress);
            this.OnCurrentProgressChangedEvent?.Invoke(sender, this.currentProgress);
        }

        private void IncrementLevel(object sender)
        {
            this.currentProgress = ZERO;
            this.repository.SetTreeProgress(ZERO);
            this.currentLevelIndex++;
            this.repository.SetTreeLevel(this.currentLevelIndex);
            var openBranchesIdSet = this.GetCurrentTreeLevel().openBranchesIdSet;
            var branchDataSet = new List<GemBranchData>();
            foreach (var openedBranchId in openBranchesIdSet)
            {
                var branchObject = this.branchDataInteractor.GetBranchObject(openedBranchId);
                branchObject.state.isOpened = true;
                var branchData = branchObject.ToData();
                branchDataSet.Add(branchData);
                this.branchDataInteractor.NotifyAboutBranchObjectChanged(sender, branchObject);
                this.OnBranchObjectOpenedEvent?.Invoke(sender, branchObject);
            }

            this.repository.UpdateBranchDataSet(branchDataSet);
            GemTreeAnalytics.LogLevelUpEvent(this.currentLevelIndex + 1);
            this.OnCurrentLevelChangedEvent?.Invoke(sender, this.currentLevelIndex);
        }

        #endregion

        #region ViewTree

        public void ViewTree(object sender)
        {
            this.isTreeViewed = true;
            this.repository.SetViewed(true);
            this.OnTreeViewedEvent?.Invoke(sender);
        }

        #endregion

        #region UnlockTree

        public void UnlockTree(object sender)
        {
            this.isTreeUnlocked = true;
            this.repository.SetInUnlocked(true);
            this.OnTreeUnlockedEvent?.Invoke(sender);
        }

        #endregion

        public BigNumber GetCurrentUpgradePrice()
        {
            var currentTreeLevel = this.GetCurrentTreeLevel();
            return currentTreeLevel.GetPrice(this.repository.GetTreeProgress());
        }

        public int GetMaxProgress()
        {
            var currentTreeLevel = this.GetCurrentTreeLevel();
            return currentTreeLevel.maxProgress;
        }

        public GemTreeInfo.TreeLevel GetCurrentTreeLevel()
        {
            var treeLevels = this.treeInfo.treeLevels;
            var currentTreeLevel = treeLevels[this.currentLevelIndex];
            return currentTreeLevel;
        }

        public IEnumerable<string> GetOpenedBranchesIdSet()
        {
            var openBranchesIdSet = new HashSet<string>();
            var treeLevels = this.treeInfo.treeLevels;
            for (var i = 0; i <= this.currentLevelIndex; i++)
            {
                var treeLevel = treeLevels[i];
                openBranchesIdSet.AddRange(treeLevel.openBranchesIdSet);
            }

            return openBranchesIdSet;
        }
        
        public bool IsEnoughSoftCurrencyForUpgrade()
        {
            return Bank.softCurrencyCount >= this.GetCurrentUpgradePrice();
        }
        
        public bool IsLastLevel()
        {
            return this.currentLevelIndex >= this.GetMaxLevel();
        }

        public int GetMaxLevel()
        {
            return this.treeInfo.treeLevels.Length - ONE;
        }
    }
}