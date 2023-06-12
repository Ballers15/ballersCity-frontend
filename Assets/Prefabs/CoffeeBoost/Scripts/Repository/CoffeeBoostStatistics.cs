using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo {
    [Serializable]
    public sealed class CoffeeBoostStatistics : ICloneable<CoffeeBoostStatistics> {
        [SerializeField] public int version;
        [SerializeField] public bool isUnlocked;
        
        public CoffeeBoostStatistics Clone()
        {
            return new CoffeeBoostStatistics
            {
                version = this.version,
                isUnlocked = this.isUnlocked,
            };
        }
    }
}