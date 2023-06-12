using System;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorModernizationInfo",
        menuName = "Domain/Hint/HintInspectorModernizationInfo"
    )]
    public sealed class HintInspectorModernizationInfo : HintStateInspector<HintState>
    {
        protected override HintState CreateDefaultState()
        {
            return new HintState();
        }
    }
}