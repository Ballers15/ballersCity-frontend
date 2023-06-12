using System;
using System.Collections;
using SinSity.Repo;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public sealed class ResearchStateInteractor : Interactor
    {
        #region Event

        public event Action<object> OnResearchUnlockedEvent;

        #endregion

        public bool isResearchUnlocked { get; private set; }

        private new ResearchDataRepository repository;

        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            this.repository = this.GetRepository<ResearchDataRepository>();
            this.Setup();
        }

        private void Setup()
        {
            this.isResearchUnlocked = this.repository.GetIsUnlocked();
        }

        public void UnlockResearch(object sender)
        {
            this.isResearchUnlocked = true;
            this.repository.SetIsUnlocked(true);
            this.OnResearchUnlockedEvent?.Invoke(sender);
        }
    }
}