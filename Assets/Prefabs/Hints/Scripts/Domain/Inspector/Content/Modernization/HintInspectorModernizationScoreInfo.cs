using System;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorModernizationScoreInfo",
        menuName = "Domain/Hint/HintInspectorModernizationScoreInfo"
    )]
    public sealed class HintInspectorModernizationScoreInfo : HintStateInspector<HintState>
    {
        protected override HintState CreateDefaultState()
        {
            return new HintState();
        }
    }
}