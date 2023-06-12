using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public interface IUpdateListenerInteractor : IInteractor
    {
        void OnUpdate(float unscaledDeltaTime);
    }
}