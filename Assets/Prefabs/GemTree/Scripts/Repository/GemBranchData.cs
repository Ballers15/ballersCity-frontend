using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class GemBranchData : ICloneable<GemBranchData>
    {
        [SerializeField] 
        public string id;
        
        [SerializeField]
        public bool isOpened;

        [SerializeField] 
        public bool isReady;
        
        [SerializeField]
        public int remainingTimeSec;

        public GemBranchData Clone()
        {
            return new GemBranchData
            {
                id = this.id,
                isOpened = this.isOpened,
                isReady = this.isReady,
                remainingTimeSec = this.remainingTimeSec
            };
        }
    }
}