using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "ResearchInfoComplex",
        menuName = "Game/Research/New ResearchInfoComplex"
    )]
    public sealed class ResearchObjectInfoComplex : ResearchObjectInfo
    {
        public override BigNumber LoadNextPrice()
        {
            return new BigNumber(0);
        }
    }
}