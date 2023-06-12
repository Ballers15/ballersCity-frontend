using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "ExternalResearchVersionUpdater",
        menuName = "Repo/Research/New ExternalResearchVersionUpdater"
    )]
    public sealed class ExternalResearchVersionUpdater : ScriptableObject, IVersionUpdater<ResearchDataState>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        [SerializeField]
        private ProfileLevelTable levelTable;

        [SerializeField]
        private int reachLevel = 8;

        private bool isUpdated;

        public bool UpdateVersion(ref ResearchDataState data)
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
            var isUnlocked = currentLevel >= this.reachLevel;
            data.version = requiredVersion;
            data.isUnlocked = isUnlocked;
            this.isUpdated = true;

            return true;
        }
    }
}