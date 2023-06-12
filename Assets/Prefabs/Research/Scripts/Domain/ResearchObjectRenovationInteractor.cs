using System.Collections;
using System.Collections.Generic;
using SinSity.Repo;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public sealed class ResearchObjectRenovationInteractor : Interactor, IModernizationAsyncListenerInteractor
    {
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


        public IEnumerator OnStartModernizationAsync()
        {
            var researchObjects = this.dataInteractor.GetResearchObjects();
            var researchDataSet = new List<ResearchData>();
            foreach (var researchObject in researchObjects)
            {
                var info = researchObject.info;
                var state = researchObject.state;
                state.price = info.LoadNextPrice();
                var researchData = researchObject.ToData();
                researchDataSet.Add(researchData);
            }

            this.repository.UpdateDataSet(researchDataSet);
            yield break;
        }
    }
}