using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture {
    public abstract class InteractorsBase {
        protected readonly Dictionary<Type, IInteractor> interactorsMap;

        
        protected InteractorsBase() {
            this.interactorsMap = new Dictionary<Type, IInteractor>();
        }
        

        public abstract void CreateAllInteractors();

        public Coroutine OnCreate() {
            return Coroutines.StartRoutine(this.OnCreateRoutine());
        }

        private IEnumerator OnCreateRoutine() {
            var allInteractors = this.interactorsMap.Values;
            foreach (var interactor in allInteractors) {
                interactor.OnCreate();
                if (!interactor.onCreateInstantly)
                    yield return null;
            }
        }



        public Coroutine InitializeAllInteractors() {
            return Coroutines.StartRoutine(InitializeAllInteractorsRoutine());
        }

        protected virtual IEnumerator InitializeAllInteractorsRoutine() {
            var allInteractors = this.interactorsMap.Values;
            foreach (var interactor in allInteractors) {
                yield return interactor.InitializeInteractor();
            }
        }

        
        
        public Coroutine OnInitialized() {
            return Coroutines.StartRoutine(this.OnInitializedRoutine());
        }

        protected IEnumerator OnInitializedRoutine() {
            var allInteractors = this.interactorsMap.Values;
            foreach (var interactor in allInteractors) {
                interactor.OnInitialized();
//                yield return null;
            }
            yield return null;
        }
        
        
        
        public Coroutine OnReady() {
            return Coroutines.StartRoutine(this.OnReadyRoutine());
        }

        protected IEnumerator OnReadyRoutine() {
            var allInteractors = this.interactorsMap.Values;
            foreach (var interactor in allInteractors) {
                interactor.OnReady();
//                yield return null;
            }
            yield return null;
        }


        
        //TODO: SMART GET!!! DICTIONARY EXTENSION!
        public T GetInteractor<T>() where T : IInteractor {
            Type type = typeof(T);
            var founded = interactorsMap.TryGetValue(type, out var resultInteractor);
            if (founded)
                return (T) resultInteractor;

            foreach (var interactor in interactorsMap.Values) {
                if (interactor is T)
                    return (T) interactor;
            }

            return (T) interactorsMap[type];
        }

        public IEnumerable<T> GetInteractors<T>() where T : IInteractor {
            var allInteractors = this.interactorsMap.Values;
            var requiredInteractors = new HashSet<T>();
            foreach (var interactor in allInteractors) {
                if (interactor is T) {
                    requiredInteractors.Add((T) interactor);
                }
            }

            return requiredInteractors;
        }


        public void CreateInteractor<T>() where T : IInteractor, new() {
            var interactor = new T();
            var type = interactor.GetType();
            this.interactorsMap[type] = interactor;
        }
        
        public void CreateInteractors(List<Type> interactorTypes) {
            foreach (var interactorType in interactorTypes) {
                var interactor = Activator.CreateInstance(interactorType) as Interactor;
                var type = interactor.GetType();
                this.interactorsMap[type] = interactor;
            }
        }
    }
}