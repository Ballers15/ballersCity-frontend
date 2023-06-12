using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "ExternalEcoBoostVersionUpdater",
        menuName = "Repo/EcoBoost/New ExternalEcoBoostVersionUpdater"
    )]
    public sealed class ExternalEcoBoostVersionUpdater : ScriptableObject, IVersionUpdater<EcoBoostStatistics>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        [SerializeField]
        private ProfileLevelTable levelTable;

        [SerializeField]
        private int reachLevel = 10;

        private bool isUpdated;

        public bool UpdateVersion(ref EcoBoostStatistics data)
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
//            if (data.version >= requiredVersion)
//            {
//                return false;
//            }

            var experienceData = experienceRepository.GetExperienceData();
            var currentExperience = experienceData.currentExperience;
            var currentLevel = Instantiate(this.levelTable).GetCurrentLevel(currentExperience);
#if DEBUG
            Debug.Log($"<color=green>UPDATE ECOBOOST CURR LEVEL {currentLevel} </color>");
#endif
            
            var isUnlocked = currentLevel >= this.reachLevel;
            data.version = requiredVersion;
            data.isUnlocked = isUnlocked;
            this.isUpdated = true;
            return true;
        }
    }
}