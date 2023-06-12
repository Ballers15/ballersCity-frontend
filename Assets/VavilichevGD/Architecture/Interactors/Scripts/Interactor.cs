using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    //Андрей, красавчик!)))
    public abstract class Interactor : IInteractor {

        #region DELEGATES

        public event IInteractorHandler OnInitializedEvent;

        #endregion
        
        public bool isInitialized { get; private set; }
        public abstract bool onCreateInstantly { get; }

        public Interactor() {
            Logging.Log($"{this.GetType().Name.ToUpper()} was created.");
        }

        /// <summary>
        /// This method executes when all repos and interactors are created;
        /// </summary>
        public virtual void OnCreate() { }

        /// <summary>
        /// This method executes when all repos and interactors are initialized and can be used in another repos and interactors;
        /// </summary>
        public virtual void OnInitialized() { }

        /// <summary>
        /// This method executes when all repos and interactors are fully initialized and ready for using anywhere;
        /// </summary>
        public virtual void OnReady() { }


        public Coroutine InitializeInteractor() {
            return Coroutines.StartRoutine(InitializeInteractorRoutine());
        }

        protected IEnumerator InitializeInteractorRoutine() {
            Logging.Log($"{this.GetType().Name.ToUpper()} start initializing...");
            this.Initialize();
            yield return Coroutines.StartRoutine(this.InitializeRoutineNew());
            this.isInitialized = true;
            this.OnInitializedEvent?.Invoke(this);
            Logging.Log($"{this.GetType().Name.ToUpper()} successfully initialized.");
        }

        protected virtual void Initialize() { }

        protected virtual IEnumerator InitializeRoutineNew() { yield return null; }



        protected T GetRepository<T>() where T : Repository {
            return Game.GetRepository<T>();
        }

        protected T GetInteractor<T>() where T : IInteractor {
            return Game.GetInteractor<T>();
        }

        protected IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            return Game.GetInteractors<T>();
        }
    }
}