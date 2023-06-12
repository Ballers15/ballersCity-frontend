using System.Collections;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public interface IRenovationAsyncListenerInteractor : IInteractor
    {
        IEnumerator OnStartRenovationAsync();
    }
}