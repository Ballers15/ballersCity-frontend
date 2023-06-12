using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture
{
    public abstract class Repository {

        #region DELEGATES

        public delegate void RepositoryHandler(Repository repository);
        public event RepositoryHandler OnInitializedEvent;

        #endregion
        
        public bool isInitialized { get; private set; }

        protected bool isSavingInProcess;

        public Repository() {
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

        public virtual void Save() {
        }
        public virtual void Reset() { }
        
        
        public Coroutine InitializeRepository() {
            return Coroutines.StartRoutine(InitializeRepositoryRoutineCurrent());
        }

        protected IEnumerator InitializeRepositoryRoutineCurrent() {
            Logging.Log($"{this.GetType().Name.ToUpper()} start initializing...");
            this.Initialize();
            yield return Coroutines.StartRoutine(this.InitializeRepositoryRoutine());
            this.isInitialized = true;
            this.OnInitializedEvent?.Invoke(this);
            Logging.Log($"{this.GetType().Name.ToUpper()} successfully initialized.");
        }

        protected virtual void Initialize() { }

        protected virtual IEnumerator InitializeRepositoryRoutine() {
            yield break;
        }
        
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