using System;
using UnityEngine;

namespace SinSity.Domain
{
    [Serializable]
    public sealed class HintStateRenovationInfo : HintState
    {
        [SerializeField]
        public bool isReady;
    }
}