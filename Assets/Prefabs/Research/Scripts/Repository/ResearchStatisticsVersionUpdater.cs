using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "ResearchStatisticsVersionUpdater",
        menuName = "Repo/Research/New ResearchStatisticsVersionUpdater"
    )]
    public sealed class ResearchStatisticsVersionUpdater : ScriptableObject, IVersionUpdater<ResearchDataStatistics>
    {
        [SerializeField]
        private ResearchDataStatistics startDataStatistics;

        public bool UpdateVersion(ref ResearchDataStatistics data)
        {
            if (data == null)
            {
                data = this.startDataStatistics.Clone();
                return true;
            }

            return false;
        }
    }
}