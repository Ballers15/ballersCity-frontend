using System;
using UnityEngine;

namespace SinSity.Domain
{
    [Serializable]
    public sealed class HintStateFirstQuest : HintState
    {
        [SerializeField]
        public bool isReady;
    }
}