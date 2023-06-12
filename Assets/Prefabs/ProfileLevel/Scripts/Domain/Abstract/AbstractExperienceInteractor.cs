using System;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public abstract class AbstractExperienceInteractor : Interactor, IExperienceVisualizer
    {
        #region Event

        public abstract event Action<object, Transform, ulong> OnVisualizeAddedExperienceEvent;

        #endregion

        private ProfileExperienceDataInteractor experienceDataInteractor;

        private ProfileExperienceRepository experienceRepository;

        public override void OnInitialized()
        {
            this.experienceRepository = this.GetRepository<ProfileExperienceRepository>();
            this.experienceDataInteractor = this.GetInteractor<ProfileExperienceDataInteractor>();
        }

        public virtual void AddExperience(object sender, ulong range)
        {
            var experience = this.experienceDataInteractor.currentExperience;
            var newExperience = experience + range;
            this.experienceRepository.SetExperience(newExperience);
            this.experienceDataInteractor.NotifyAboutExperienceChanged(sender, newExperience);
        }
    }
}