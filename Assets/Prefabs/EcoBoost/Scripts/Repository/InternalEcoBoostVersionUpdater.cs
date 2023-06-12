using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "InternalEcoBoostVersionUpdater",
        menuName = "Repo/EcoBoost/New InternalEcoBoostVersionUpdater"
    )]
    public sealed class InternalEcoBoostVersionUpdater : ScriptableObject, IVersionUpdater<EcoBoostStatistics>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        public bool UpdateVersion(ref EcoBoostStatistics data)
        {
            if (data == null)
            {
                data = this.BuildStatistics();
                return true;
            }

            return false;
        }

        private EcoBoostStatistics BuildStatistics()
        {
            return new EcoBoostStatistics
            {
                version = Instantiate(this.scriptableVersion).value,
                isEnabled = false,
                isUnlocked = false
            };
        }
    }
}