using System;
using Spine.Unity;
using UnityEngine;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIGemTreeProperties : UIProperties
    {
        [SerializeField]
        public SkeletonGraphic skeletonGraphic;

        [SerializeField]
        public UIGemBranch[] gemBranches;
    }
}