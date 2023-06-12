using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo
{
    [CreateAssetMenu(
        fileName = "InternalProfileExperienceVersionUpdater",
        menuName = "Repo/ProfileExperience/New InternalProfileExperienceVersionUpdater"
    )]
    public sealed class InternalProfileExperienceVersionUpdater : ScriptableObject,
        IVersionUpdater<ProfileExperienceData>
    {
        [SerializeField]
        private ScriptableVersion scriptableVersion;

        public bool UpdateVersion(ref ProfileExperienceData data)
        {
            if (data != null)
            {
                return false;
            }

            data = this.BuildData();
            return true;
        }

        private ProfileExperienceData BuildData()
        {
#if DEBUG
            Debug.Log("<color=red>BUILD INTERNAL DATA!!!!!!!</color>");
#endif
            const string TUTORIAL_STATE_KEY = "TUTORIAL_STATE";
            var startVersion = Instantiate(this.scriptableVersion).value;
            if (Storage.HasObject(TUTORIAL_STATE_KEY))
            {
                var tutorialStatistics = Storage.GetCustom<TutorialStatistics>(TUTORIAL_STATE_KEY, null);
                
                if (tutorialStatistics.isCompleted)
                {
                    startVersion = 0;
                }
            }


#if DEBUG
            Debug.Log($"<color=red>INTERNAL EXP VERSION: {startVersion}</color>");
#endif

            return new ProfileExperienceData
            {
                currentExperience = 0,
                version = startVersion
            };
        }
    }
}