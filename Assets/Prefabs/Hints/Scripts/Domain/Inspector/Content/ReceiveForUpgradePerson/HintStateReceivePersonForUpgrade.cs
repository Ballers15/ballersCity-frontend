using System;
using UnityEngine;

namespace SinSity.Domain
{
    [Serializable]
    public sealed class HintStateReceivePersonForUpgrade : HintState
    {
        [SerializeField]
        public bool isReady;
    }
}