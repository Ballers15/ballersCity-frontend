using System;
using System.Collections;
using SinSity.Repo;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public sealed class ResearchObjectRewardInteractor : Interactor
    {
        #region Const

        private const int ZERO = 0;

        #endregion

        #region Event

        public event Action<object, ResearchObject> OnResearchObjectRewardReceivedEvent;

        #endregion

        private new ResearchDataRepository repository;

        private ResearchObjectDataInteractor dataInteractor;

        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            this.repository = this.GetRepository<ResearchDataRepository>();
        }

        public override void OnInitialized()
        {
            this.dataInteractor = this.GetInteractor<ResearchObjectDataInteractor>();
        }

        public void ReceiveReward(object sender, ResearchObject researchObject)
        {
            var state = researchObject.state;
            if (!state.isRewardReady)
            {
                throw new Exception("Reward is not ready!");
            }

            state.isEnabled = false;
            state.isRewardReady = false;
            state.remainingTimeSec = ZERO;
            state.price = researchObject.info.LoadNextPrice();
            var researchData = researchObject.ToData();
            this.repository.UpdateData(researchData);
            this.dataInteractor.NotifyAboutDataChanged(researchObject);
            this.OnResearchObjectRewardReceivedEvent?.Invoke(sender, researchObject);
        }
    }
}