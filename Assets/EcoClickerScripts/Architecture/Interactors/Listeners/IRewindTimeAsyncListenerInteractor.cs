using System.Collections;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public interface IRewindTimeAsyncListenerInteractor : IInteractor
    {
        IEnumerator OnRewindTimeAsync(RewindTimeIntent intent);
    }
}