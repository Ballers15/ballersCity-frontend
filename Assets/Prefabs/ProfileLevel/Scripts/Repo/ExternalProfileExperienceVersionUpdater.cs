using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "ExternalProfileExperienceVersionUpdater",
        menuName = "Repo/ProfileExperience/New ExternalProfileExperienceVersionUpdater"
    )]
    public sealed class ExternalProfileExperienceVersionUpdater : ScriptableObject,
        IVersionUpdater<ProfileExperienceData>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        [SerializeField]
        private ProfileLevelTable levelTable;

        public bool UpdateVersion(ref ProfileExperienceData data)
        {
            var version = data.version;
#if DEBUG
            Debug.Log("MY EXPERIENCE VERSION: " + version);
#endif
            
            var requiredVersion = Instantiate(this.scriptableVersion).value;
            if (version >= requiredVersion)
            {
                Debug.Log("VERSION UPDATER NO UPDATE EXPERIENCE");
                return false;
            }

            data.currentExperience = this.LoadStartExperience();
            data.version = requiredVersion;
            return true;
        }

        private ulong LoadStartExperience()
        {
            var idleObjectsRepository = Game.GetRepository<IdleObjectsRepository>();
            var idleObjectsData = idleObjectsRepository.states;
            var jsons = idleObjectsData.stateJsons;
            var startLevel = 1;
            foreach (var json in jsons)
            {
                var state = JsonUtility.FromJson<IdleObjectState>(json);
                if (state.isBuilded)
                {
                    startLevel++;
                }
            }

            var startExperience = Instantiate(this.levelTable).GetRequiredAbsExperience(startLevel);
            Debug.Log($"<color=red>VERSION UPDATER START EXPERIENCE: {startExperience}</color>");
            return startExperience;
        }
    }
}