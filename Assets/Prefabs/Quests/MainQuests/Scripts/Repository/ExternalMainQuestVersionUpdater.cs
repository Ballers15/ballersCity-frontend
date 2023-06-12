using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "ExternalMainQuestVersionUpdater",
        menuName = "Repo/MainQuest/New ExternalMainQuestVersionUpdater"
    )]
    public sealed class ExternalMainQuestVersionUpdater : ScriptableObject, IVersionUpdater<MainQuestData>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        [SerializeField]
        private ProfileLevelTable levelTable;

        [SerializeField]
        private int reachLevel = 4;

        private bool isUpdated;

        public bool UpdateVersion(ref MainQuestData data)
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