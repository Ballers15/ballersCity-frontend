using System;
using UnityEngine;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class AirDropStatistics
    {
        [SerializeField] 
        public int version;
        
        [SerializeField]
        public bool isLuckyModeEnabled;

        [SerializeField]
        public int luckyIndex;

        [SerializeField] 
        public bool isAirDropEnabled;

        public AirDropStatistics Clone()
        {
            return new AirDropStatistics
            {
                version = this.version,
                isAirDropEnabled = this.isAirDropEnabled,
                isLuckyModeEnabled = this.isLuckyModeEnabled,
                luckyIndex = this.luckyIndex
            };
        }
    }
}