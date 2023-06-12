using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Domain;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Scripts {
    public class GameSinSityForTests : Game, IDisposable {
        public static int VERSION = 160;
        public delegate void OnSessionPauseHandler(bool paused);
        public static event GameSinSityForTests.OnSessionPauseHandler OnSessionPaused;
        
        private List<Type> _repositoriesTypes;
        private List<Type> _interactorTypes;

        public static void Run(List<Type> repositoryTypes, List<Type> interactorTypes) {
            if (instance == null) {
                instance = new GameSinSityForTests(repositoryTypes, interactorTypes);
                instance.Initialize();
            }
        }

        public GameSinSityForTests(List<Type> repositoryTypes, List<Type> interactorTypes) {
            _repositoriesTypes = repositoryTypes;
            _interactorTypes = interactorTypes;
        }

        protected override IEnumerator InitializeRoutine() {
            LoadingScreen.Show(true);

            this.repositoriesBase = new RepositoriesBaseSinSityForTests(_repositoriesTypes);
            this.repositoriesBase.CreateAllRepositories();
            yield return this.repositoriesBase.OnCreate();

            this.interactorsBase = new InteractorsBaseSinSityForTests(_interactorTypes);
            this.interactorsBase.CreateAllInteractors();
            yield return this.interactorsBase.OnCreate();

            yield return this.repositoriesBase.InitializeAllRepositories();
            yield return this.repositoriesBase.OnInitialized();

            yield return this.interactorsBase.InitializeAllInteractors();
            yield return this.interactorsBase.OnInitialized();

            this.NotifyAboutGameInitialized();

            yield return this.repositoriesBase.OnReady();
            yield return this.interactorsBase.OnReady();

            yield return null;
            this.NotifyAboutGameReady();
            this.NotifyAboutGameStart();

            firstPlay = false;
            LoadingScreen.Hide();
        }

        private void OnInteractorBaseStatusChanged(string statusText) {
            NotifyAboutStatusChanged(statusText);
        }

        private void OnRepositoryBaseStatusChanged(string statusText) {
            NotifyAboutStatusChanged(statusText);
        }


        public static void PauseSession() {
            GameTimeInteractor gameTimeInteractor = GetInteractor<GameTimeInteractor>();
            gameTimeInteractor.Save();

            GameTime.Pause();
            OnSessionPaused?.Invoke(true);
        }

        public static void UnpauseSession() {
            var gameEcoClicker = instance as GameSinSityForTests;
            Coroutines.StartRoutine(gameEcoClicker.FastInitializeRoutine());
        }

        private IEnumerator FastInitializeRoutine() {
            LoadingScreen.Show(false);

            var repoEcoClicker = repositoriesBase as RepositoriesBaseSinSityForTests;
            yield return repoEcoClicker.FastInitialize();

            var interactorsEcoClicker = interactorsBase as InteractorsBaseSinSityForTests;
            yield return interactorsEcoClicker.FastInitialize();

            //Перемотка времени -- очень сложный алгоритмический процесс!!!
            var rewindTimeInteractor = GetInteractor<RewindTimeInteractor>();
            var offlineTime = (float) GameTime.timeSinceLastSessionEndedToCurrentSessionStartedSeconds;
            var offlineSeconds = Mathf.FloorToInt(offlineTime);
            var rewindTimeIntent = new RewindTimeIntentFocusChanged(offlineSeconds);
            yield return rewindTimeInteractor.RewindTime(rewindTimeIntent);

            LoadingScreen.Hide();
            GameTime.Unpause();
            OnSessionPaused?.Invoke(false);
        }

        public static void Dispose() {
            instance = null;
        }

        void IDisposable.Dispose() {
            Dispose();
        }
    }
}