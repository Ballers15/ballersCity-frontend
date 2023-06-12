using UnityEngine;

namespace VavilichevGD.Architecture
{
    public delegate void IInteractorHandler(IInteractor iIteractor);
    
    public interface IInteractor {

        #region DELEGATES

        event IInteractorHandler OnInitializedEvent;

        #endregion
        
        Coroutine InitializeInteractor();
        void OnCreate();
        void OnInitialized();
        void OnReady();
        bool onCreateInstantly { get; }

    }
}