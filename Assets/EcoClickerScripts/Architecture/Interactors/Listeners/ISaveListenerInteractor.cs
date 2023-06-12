using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public interface ISaveListenerInteractor : IInteractor
    {
        void OnSave();
    }
}