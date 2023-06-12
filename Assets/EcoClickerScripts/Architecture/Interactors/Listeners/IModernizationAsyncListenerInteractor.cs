using System.Collections;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public interface IModernizationAsyncListenerInteractor : IInteractor
    {
        IEnumerator OnStartModernizationAsync();
    }
}