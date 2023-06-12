using System;

namespace VavilichevGD.Architecture
{
    public interface IDataInteractor<T>
    {
        event Action<T> OnDataChangedEvent;

        void NotifyAboutDataChanged(T data);
    }
}