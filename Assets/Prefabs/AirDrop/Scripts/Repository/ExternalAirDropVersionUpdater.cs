using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "ExternalAirDropVersionUpdater",
        menuName = "Repo/AirDrop/New ExternalAirDropVersionUpdater"
    )]
    public sealed class ExternalAirDropVersionUpdater : ScriptableObject, IVersionUpdater<AirDropStatistics>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        [SerializeField]
        private ProfileLevelTable levelTable;

        [SerializeField]
        private int reachLevel = 6;

        private bool isUpdated;
        
        public bool UpdateVersion(ref AirDropStatistics data)
        {
            
            if (this.isUpdated)
            {
                return false;
            }
            
            var experienceRepository = Game.GetRepository<ProfileExperienceRepository>();
            if (!experienceRepository.isVersionUpdated)
            {
                return false;
            }

            var requiredVersion = Instantiate(this.scriptableVersion).value;
            var experienceData = experienceRepository.GetExperienceData();
            var currentExperience = experienceData.currentExperience;
            var currentLevel = Instantiate(this.levelTable).GetCurrentLevel(currentExperience);
            var isAirDropEnabled = currentLevel >= this.reachLevel;
            data.isAirDropEnabled = isAirDropEnabled;
            data.version = requiredVersion;
            this.isUpdated = true;
            return true;
        }
    }
}