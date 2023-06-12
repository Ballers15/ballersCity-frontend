using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class GemTreeStatistics : ICloneable<GemTreeStatistics>
    {
        [SerializeField]
        public int version;
        
        [SerializeField] 
        public bool isUnlocked;

        [SerializeField]
        public bool isViewed;
        
        [SerializeField]
        public int level;

        [SerializeField]
        public int progress;

        public GemTreeStatistics Clone()
        {
            return new GemTreeStatistics
            {
                version = this.version,
                isUnlocked = this.isUnlocked,
                isViewed = this.isViewed,
                level = this.level,
                progress = this.progress
            };
        }
    }
}