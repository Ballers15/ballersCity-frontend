using System;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    [Serializable]
    public sealed class AirDropParcel
    { 
        [SerializeField]
        public bool needWatchAds;

        [SerializeField]
        public ScriptableRewardInfoBuilder rewardInfoBuilder;
    }
}