using System.Collections.Generic;
using System.Linq;

namespace SinSity.Domain
{
    public static class ResearchObjectDataInteractorExtensions
    {
        public static IEnumerable<ResearchObject> GetProcessingResearchObjects(this ResearchObjectDataInteractor interactor)
        {
            return interactor
                .GetResearchObjects()
                .Where(it => it.state.isEnabled);
        }
    }
}