using System;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class ResearchDataStatistics : ICloneable<ResearchDataStatistics>
    {
        [SerializeField]
        public List<ResearchData> researchDataSet;

        public ResearchDataStatistics Clone()
        {
            var researchDataSet = new List<ResearchData>();
            foreach (var data in this.researchDataSet)
            {
                researchDataSet.Add(data.Clone());
            }

            return new ResearchDataStatistics
            {
                researchDataSet = researchDataSet
            };
        }
    }
}