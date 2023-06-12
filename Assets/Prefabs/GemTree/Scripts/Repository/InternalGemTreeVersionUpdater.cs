using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "InternalGemTreeVersionUpdater",
        menuName = "Repo/GemTree/New InternalGemTreeVersionUpdater"
    )]
    public sealed class InternalGemTreeVersionUpdater : ScriptableObject, IVersionUpdater<GemTreeStatistics>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        public bool UpdateVersion(ref GemTreeStatistics data)
        {
            if (data != null)
            {
                return false;
            }

            data = new GemTreeStatistics
            {
                version = Instantiate(this.scriptableVersion).value,
                isUnlocked = false,
                isViewed = false,
                level = 0,
                progress = 0
            };

            return true;
        }
    }
}