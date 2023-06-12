using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture
{
    public abstract class RepositoriesBase
    {
        protected readonly Dictionary<Type, Repository> repositoriesMap;

        protected RepositoriesBase() {
            this.repositoriesMap = new Dictionary<Type, Repository>();
        }

        public abstract void CreateAllRepositories();

        public Coroutine OnCreate() {
            return Coroutines.StartRoutine(this.OnCreateRoutine());
        }

        private IEnumerator OnCreateRoutine() {
            var allRepositories = this.repositoriesMap.Values;
            foreach (var repository in allRepositories) {
                repository.OnCreate();
//                yield return null;
            }

            yield return null;
        }
        
        
        
        

        public Coroutine InitializeAllRepositories() {
            return Coroutines.StartRoutine(this.InitializeAllRepositoriesRoutine());
        }

        protected virtual IEnumerator InitializeAllRepositoriesRoutine() {
            var allRepositories = this.repositoriesMap.Values;
            foreach (var repository in allRepositories) {
                yield return repository.InitializeRepository();
            }
        }


        public Coroutine OnInitialized() {
            return Coroutines.StartRoutine(this.OnInitializedRoutine());
        }

        protected IEnumerator OnInitializedRoutine() {
            var allRepositories = this.repositoriesMap.Values;
            foreach (var repository in allRepositories) {
                repository.OnInitialized();
                //yield return null;
            }
            yield return null;

        }
        
        
        public Coroutine OnReady() {
            return Coroutines.StartRoutine(this.OnReadyRoutine());
        }

        protected IEnumerator OnReadyRoutine() {
            var allRepositories = this.repositoriesMap.Values;
            foreach (var repository in allRepositories) {
                repository.OnReady();
                //yield return null;
            }
            yield return null;

        }
        
        
        //Refactor to is Dictionary extension named Find<T>
        public T GetRepository<T>() where T : Repository
        {
            var type = typeof(T);
            Repository resultRepository = null;
            var founded = repositoriesMap.TryGetValue(type, out resultRepository);
            if (founded)
                return (T) resultRepository;

            foreach (Repository repository in repositoriesMap.Values)
            {
                if (repository is T)
                    return (T) repository;
            }

            return (T) repositoriesMap[type];
        }

        public IEnumerable<T> GetRepositories<T>()
        {
            var requiredRepositories = new HashSet<T>();
            var repositories = repositoriesMap.Values;
            foreach (var repository in repositories)
            {
                if (repository is T requiredRepository)
                {
                    requiredRepositories.Add(requiredRepository);
                }
            }

            return requiredRepositories;
        }

        public void CreateRepository<T>() where T : Repository, new() {
            var repository = new T();
            var type = repository.GetType();
            this.repositoriesMap[type] = repository;
        }
        
        public void CreateRepositories(List<Type> repoTypes) {
            foreach (var repoType in repoTypes) {
                var repository = Activator.CreateInstance(repoType) as Repository;
                var type = repository.GetType();
                this.repositoriesMap[type] = repository;
            }
        }
    }
}