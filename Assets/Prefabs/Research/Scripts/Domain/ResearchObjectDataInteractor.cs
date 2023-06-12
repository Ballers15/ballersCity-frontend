using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SinSity.Core;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public sealed class ResearchObjectDataInteractor : Interactor, IDataInteractor<ResearchObject>
    {
        #region Const

        private const string RESEARCH_INFO_FOLDER = "Researches";

        #endregion

        #region Events

        public event Action<ResearchObject> OnDataChangedEvent;

        #endregion

        private new ResearchDataRepository repository;

        private Dictionary<string, ResearchObject> researchMap;

        public override bool onCreateInstantly { get; } = true;

        #region Initialize

        protected override void Initialize() {
            base.Initialize();
            this.researchMap = new Dictionary<string, ResearchObject>();
            this.repository = this.GetRepository<ResearchDataRepository>();
            this.LoadState();
        }

        private void LoadState()
        {
            var researchInfoSet = Resources.LoadAll<ResearchObjectInfo>(RESEARCH_INFO_FOLDER);
            foreach (var researchInfo in researchInfoSet)
            {
                var researchInfoId = researchInfo.id;
                var researchData = this.repository.GetData(researchInfoId);
                var researchState = new ResearchObjectState(researchData);
                var research = new ResearchObject(researchInfo, researchState);
                this.researchMap[researchInfoId] = research;
            }
        }

        #endregion
        
        public ResearchObject GetResearchObject(string id)
        {
            return this.researchMap[id];
        }

        public List<ResearchObject> GetResearchObjects()
        {
            return this.researchMap.Values.ToList();
        }

        public int GetResearchObjectsCount()
        {
            return this.researchMap.Count;
        }
        
        public void NotifyAboutDataChanged(ResearchObject data)
        {
            this.OnDataChangedEvent?.Invoke(data);
        }

        public bool HasActiveComplexResearch() {
            foreach (KeyValuePair<string,ResearchObject> keyValuePair in researchMap) {
                ResearchObject researchObject = keyValuePair.Value;
                if (researchObject.info.rewardInfo is ComplexResearchRewardInfo && researchObject.state.isEnabled)
                    return true;
            }

            return false;
        }

        public int GetComplexResearchTimeLeftSec() {
            int complexResearchTimeLeftSec = 0;
            
            foreach (KeyValuePair<string,ResearchObject> keyValuePair in researchMap) {
                ResearchObject researchObject = keyValuePair.Value;
                if (researchObject.info.rewardInfo is ComplexResearchRewardInfo) {
                    if (researchObject.state.isEnabled && researchObject.state.remainingTimeSec > complexResearchTimeLeftSec)
                        complexResearchTimeLeftSec = researchObject.state.remainingTimeSec;
                }
            }

            return complexResearchTimeLeftSec;
        }
    }
}