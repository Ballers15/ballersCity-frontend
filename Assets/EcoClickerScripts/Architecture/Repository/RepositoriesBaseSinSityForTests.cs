using System;
using System.Collections;
using System.Collections.Generic;
using Orego.Util;
using SinSity.Achievements;
using SinSity.Core;
using SinSity.Meta;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Scripts {
    public class RepositoriesBaseSinSityForTests : RepositoriesBase {
        private List<Type> _repositoriesTypes;

        public RepositoriesBaseSinSityForTests(List<Type> repositories) {
            _repositoriesTypes = repositories;
        }

        public override void CreateAllRepositories() {
            CreateRepositories(_repositoriesTypes);
        }

        protected override IEnumerator InitializeAllRepositoriesRoutine() {
            yield return base.InitializeAllRepositoriesRoutine();
            yield return this.UpdateVersionInRepositories();
        }

        private IEnumerator UpdateVersionInRepositories() {
            var repositories = this.GetRepositories<IUpdateVersionRepository>();
            var isRequiredUpdate = true;
            while (isRequiredUpdate) {
                isRequiredUpdate = false;
                foreach (var repository in repositories) {
                    var isUpdatedReference = new Reference<bool>();

                    yield return repository.OnUpdateVersion(isUpdatedReference);
                    if (isUpdatedReference.value) {

#if DEBUG
                        Debug.Log("UPDATED REPO: " + repository.GetType().Name);
#endif
                        isRequiredUpdate = true;
                        break;
                    }
                }
            }
        }

        public Coroutine FastInitialize() {
            return Coroutines.StartRoutine(FastInitializeRoutine());
        }

        private IEnumerator FastInitializeRoutine() {
            GameTimeRepository gameTimeRepository = GetRepository<GameTimeRepository>();
            yield return gameTimeRepository.InitializeRepository();
        }
    }
}