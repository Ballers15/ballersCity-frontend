using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "InternalResearchVersionUpdater",
        menuName = "Repo/Research/New InternalResearchVersionUpdater"
    )]
    public sealed class InternalResearchVersionUpdater : ScriptableObject, IVersionUpdater<ResearchDataState>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        public bool UpdateVersion(ref ResearchDataState data)
        {
            if (data != null)
            {
                return false;
            }

            data = this.BuildData();
            return true;
        }

        private ResearchDataState BuildData()
        {
            return new ResearchDataState
            {
                isUnlocked = false,
                version = Instantiate(this.scriptableVersion).value
            };
        }
    }
}