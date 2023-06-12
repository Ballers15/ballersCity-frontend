using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class ResearchData : ICloneable<ResearchData>
    {
        [SerializeField]
        public string id;

        [SerializeField]
        public bool isEnabled;

        [SerializeField]
        public int remainingTimeSec;

        [SerializeField] 
        public bool isRewardReady;

        [SerializeField]
        public string priceBigNumberStr = "1000";
        
        public ResearchData Clone()
        {
            return new ResearchData
            {
                id = this.id,
                isEnabled = this.isEnabled,
                remainingTimeSec = this.remainingTimeSec,
                isRewardReady = this.isRewardReady,
                priceBigNumberStr = this.priceBigNumberStr
            };
        }
    }
}