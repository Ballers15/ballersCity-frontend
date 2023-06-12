using System;
using UnityEngine;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIGemBranchProperties : UIProperties
    {
        [SerializeField]
        public string id;

        [SerializeField]
        public UIGemFruit gemFruit;
    }
}