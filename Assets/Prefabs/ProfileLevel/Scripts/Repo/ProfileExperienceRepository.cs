using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using Orego.Util;
using SinSity.Data;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public sealed class ProfileExperienceRepository : Repository, IUpdateVersionRepository {
        #region Const

        private const string PREF_KEY = "PROFILE_LEVEL_PREF_KEY";

        #endregion

        public bool isVersionUpdated { get; private set; }

        private readonly IVersionUpdater<ProfileExperienceData> internalVersionUpdater;
        private readonly IVersionUpdater<ProfileExperienceData> externalVersionUpdater;
        private ProfileExperienceData experienceData;

        public ProfileExperienceRepository() {
            internalVersionUpdater = ScriptableObject.Instantiate(
                Resources.Load<InternalProfileExperienceVersionUpdater>(
                    "InternalProfileExperienceVersionUpdater"
                ));
            externalVersionUpdater = ScriptableObject.Instantiate(
                Resources.Load<ExternalProfileExperienceVersionUpdater>(
                    "ExternalProfileExperienceVersionUpdater"
                ));
        }

        protected override IEnumerator InitializeRepositoryRoutine() {
            using var request = new GameDataRequest(PREF_KEY);
            
            yield return request.Send(RequestType.GET);
            
            experienceData = request.GetGameData<ProfileExperienceData>(null);
            internalVersionUpdater.UpdateVersion(ref experienceData);
        }

        public override void Save() {
            if (isSavingInProcess) return;
            Coroutines.StartRoutine(SaveAsync());
        }
        
        private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            using var client = new GameDataRequest(PREF_KEY, experienceData);
            
            yield return client.Send();
            
            isSavingInProcess = false;
        }

        public void SetExperience(ulong experience) {
            experienceData.currentExperience = experience;
            Save();
        }

        public ProfileExperienceData GetExperienceData() {
            return experienceData.Clone();
        }

        public IEnumerator OnUpdateVersion(Reference<bool> isUpdated) {
            if (this.isVersionUpdated) {
                yield break;
            }
#if DEBUG
            Debug.Log("ON START UPDATE EXPERIENCE");
#endif
            var isVersionUpdated = this.externalVersionUpdater.UpdateVersion(ref experienceData);
            if (isVersionUpdated) {
                Save();
            }

            isUpdated.value = isVersionUpdated;
            this.isVersionUpdated = true;
        }
    }
}