using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "ExternalGemTreeVersionUpdater",
        menuName = "Repo/GemTree/New ExternalGemTreeVersionUpdater"
    )]
    public sealed class ExternalGemTreeVersionUpdater : ScriptableObject, IVersionUpdater<GemTreeStatistics>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        [SerializeField]
        private ProfileLevelTable levelTable;

        [SerializeField]
        private int reachLevel = 10;

        private bool isUpdated;

        public bool UpdateVersion(ref GemTreeStatistics data)
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
#if DEBUG
            Debug.Log($"<color=green>UPDATE GEM TREE CURR LEVEL {currentLevel} </color>");
#endif

            var isUnlocked = currentLevel >= this.reachLevel;
            data.version = requiredVersion;
            data.isUnlocked = isUnlocked;
            this.isUpdated = true;
            return true;
        }
    }
}