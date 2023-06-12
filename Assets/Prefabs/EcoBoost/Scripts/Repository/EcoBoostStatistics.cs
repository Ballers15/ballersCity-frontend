using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class EcoBoostStatistics : ICloneable<EcoBoostStatistics>
    {
        [SerializeField]
        public int version;

        [SerializeField]
        public bool isUnlocked;

        [SerializeField]
        public bool isEnabled;

        [SerializeField]
        public int remainingTimeSec;

        public EcoBoostStatistics Clone()
        {
            return new EcoBoostStatistics
            {
                version = this.version,
                isUnlocked = this.isUnlocked,
                isEnabled = this.isEnabled,
                remainingTimeSec = this.remainingTimeSec
            };
        }
    }
}