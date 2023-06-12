using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class GemBranchDataSet : ICloneable<GemBranchDataSet>
    {
        [SerializeField]
        public List<GemBranchData> dataSet;

        public GemBranchDataSet(Dictionary<string, GemBranchData> branchDataMap) {
            this.dataSet = new List<GemBranchData>();
            int count = branchDataMap.Count;
            
            for (int i = 0; i < count; i++) {
                var gemBranchData = branchDataMap.ElementAt(i).Value;
                dataSet.Add(gemBranchData);
            }
        }

        public GemBranchDataSet(List<GemBranchData> dataSet) {
            this.dataSet = dataSet;
        }

        public GemBranchDataSet Clone()
        {
            var dataSet = new List<GemBranchData>();
            foreach (var gemBranchData in this.dataSet)
            {
                var branchData = gemBranchData.Clone();
                dataSet.Add(branchData);
            }

            return new GemBranchDataSet(dataSet);
        }
    }
}