using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SinSity.Core;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain {
    public sealed class GemTreeBranchDataInteractor : Interactor {
        #region Const

        private const string GEM_BRANCH_INFO_DIRECTORY_PATH = "GemBranches";

        #endregion

        #region Event

        public event Action<object, GemBranchObject> OnBranchObjectChangedEvent;

        #endregion

        private new GemTreeRepository repository;

        private readonly Dictionary<string, GemBranchObject> branchMap;

        public GemTreeBranchDataInteractor() {
            this.branchMap = new Dictionary<string, GemBranchObject>();
        }

        #region Initialize

        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            this.repository = this.GetRepository<GemTreeRepository>();
            this.Setup();
        }

        private void Setup() {
            var branchInfoSet = Resources.LoadAll<GemBranchObjectInfo>(GEM_BRANCH_INFO_DIRECTORY_PATH);
            foreach (var branchInfo in branchInfoSet) {
                this.SetupBranchObject(branchInfo);
            }
        }

        private void SetupBranchObject(GemBranchObjectInfo branchInfo) {
            var branchId = branchInfo.id;
            var branchData = this.repository.GetBranchData(branchId);
            var branchObjectState = new GemBranchObjectState(branchData);
            var branchObject = new GemBranchObject(branchInfo, branchObjectState);
            this.branchMap[branchId] = branchObject;
        }

        #endregion

        public IEnumerable<GemBranchObject> GetBranchObjects() {
            return this.branchMap.Values.ToList();
        }

        public GemBranchObject GetBranchObject(string id) {
            return this.branchMap[id];
        }

        public void NotifyAboutBranchObjectChanged(object sender, GemBranchObject branchObject) {
            this.OnBranchObjectChangedEvent?.Invoke(sender, branchObject);
        }

        public bool HasTreeAnyReadyFruit() {
            foreach (var pair in branchMap) {
                GemBranchObject gemBranchObject = pair.Value;
                if (gemBranchObject.state.isReady)
                    return true;
            }

            return false;
        }
    }
}