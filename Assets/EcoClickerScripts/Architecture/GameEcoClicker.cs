using System.Collections;
using SinSity.Domain;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Scripts
{
    public class GameEcoClicker : Game
    {
        public static int VERSION = 160;
        
        public delegate void OnSessionPauseHandler(bool paused);

        public static event OnSessionPauseHandler OnSessionPaused;

        public static void Run()
        {
            if (instance == null)
            {
                instance = new GameEcoClicker();
                instance.Initialize();
            }
        }

        protected override IEnumerator InitializeRoutine()
        {
            LoadingScreen.Show(true);

            this.repositoriesBase = new RepositoriesBaseEcoClicker();
            this.repositoriesBase.CreateAllRepositories();
            yield return this.repositoriesBase.OnCreate();
            
            this.interactorsBase = new InteractorsBaseEcoClicker();
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

        private void OnInteractorBaseStatusChanged(string statusText)
        {
            NotifyAboutStatusChanged(statusText);
        }

        private void OnRepositoryBaseStatusChanged(string statusText)
        {
            NotifyAboutStatusChanged(statusText);
        }


        public static void PauseSession()
        {
            GameTimeInteractor gameTimeInteractor = GetInteractor<GameTimeInteractor>();
            gameTimeInteractor.Save();

            GameTime.Pause();
            OnSessionPaused?.Invoke(true);
        }

        public static void UnpauseSession()
        {
            GameEcoClicker gameEcoClicker = instance as GameEcoClicker;
            Coroutines.StartRoutine(gameEcoClicker.FastInitializeRoutine());
        }

        private IEnumerator FastInitializeRoutine()
        {
            ReinitializeScreen.Show(false);

            RepositoriesBaseEcoClicker repoEcoClicker = repositoriesBase as RepositoriesBaseEcoClicker;
            yield return repoEcoClicker.FastInitialize();

            InteractorsBaseEcoClicker interactorsEcoClicker = interactorsBase as InteractorsBaseEcoClicker;
            yield return interactorsEcoClicker.FastInitialize();

            //Перемотка времени -- очень сложный алгоритмический процесс!!!
            var rewindTimeInteractor = GetInteractor<RewindTimeInteractor>();
            var offlineTime = (float) GameTime.timeSinceLastSessionEndedToCurrentSessionStartedSeconds;
            var offlineSeconds = Mathf.FloorToInt(offlineTime);
            var rewindTimeIntent = new RewindTimeIntentFocusChanged(offlineSeconds);
            yield return rewindTimeInteractor.RewindTime(rewindTimeIntent);

            ReinitializeScreen.Hide();
            GameTime.Unpause();
            OnSessionPaused?.Invoke(false);
        }
    }
}