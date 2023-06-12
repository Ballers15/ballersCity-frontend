using System;
using SinSity.Repo;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public sealed class ProfileExperienceDataInteractor : Interactor
    {
        #region Event

        public event Action<object, ulong> OnExperienceChangedEvent;
        public event Action<object, ulong, ulong> OnExperienceReceivedEvent;

        #endregion

        public ulong currentExperience { get; private set; }

        private new ProfileExperienceRepository repository;

        #region Initialize

        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            this.repository = this.GetRepository<ProfileExperienceRepository>();
            this.Setup();
        }

        private void Setup()
        {
            var experienceData = this.repository.GetExperienceData();
            this.currentExperience = experienceData.currentExperience;
        }

        #endregion

        public void NotifyAboutExperienceChanged(object sender, ulong newExperience)
        {
            this.currentExperience = newExperience;
            this.OnExperienceChangedEvent?.Invoke(sender, this.currentExperience);
        }

        public void NotifyAboutExperienceReceived(object sender, ulong receivedExp, ulong requiredLevelExp) {
            this.OnExperienceReceivedEvent?.Invoke(sender, receivedExp, requiredLevelExp);
        }
    }
}