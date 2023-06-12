using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "InternalAirDropVersionUpdater",
        menuName = "Repo/AirDrop/New InternalAirDropVersionUpdater"
    )]
    public sealed class InternalAirDropVersionUpdater : ScriptableObject, IVersionUpdater<AirDropStatistics>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        public bool UpdateVersion(ref AirDropStatistics data)
        {
            if (data != null)
            {
                return false;
            }

            data = this.BuildData();
            return true;
        }

        private AirDropStatistics BuildData()
        {
            return new AirDropStatistics
            {
                version = Instantiate(this.scriptableVersion).value,
                isAirDropEnabled = false,
                isLuckyModeEnabled = true,
                luckyIndex = 0
            };
        }
    }
}