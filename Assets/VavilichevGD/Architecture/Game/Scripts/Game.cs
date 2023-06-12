using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Architecture
{
    public abstract class Game
    {

        #region CONSTANTS

        private const string KEY_FIRST_PLAY = "FIRST_PLAY";

        #endregion
        
        public static bool isInitialized { get; private set; }

        protected static Game instance;

        public delegate void GameInitializedHandler(Game game);

        public static event GameInitializedHandler OnGameInitialized;

        public static event Action<Game> OnGameReady;

        public static event Action<Game> OnGameStart;

        public delegate void GameInitializingStatusHandler(Game game, string statusText);

        public static event GameInitializingStatusHandler OnGameInitializingStatusChanged;

        protected RepositoriesBase repositoriesBase;
        
        protected InteractorsBase interactorsBase;

        public static bool firstPlay {
            get => Storage.GetBool(KEY_FIRST_PLAY, true);
            protected set => Storage.SetBool(KEY_FIRST_PLAY, value);
        }


        // TODO: You should write your own Game*name* script and past something like that:
//        protected static void Run() {
//            // Create instance.
//            // Initialize instance.
//        }

        public virtual void Initialize()
        {
            Coroutines.StartRoutine(InitializeRoutine());
        }

        protected abstract IEnumerator InitializeRoutine();

        protected void NotifyAboutGameInitialized()
        {
            OnGameInitialized?.Invoke(this);
        }

        protected void NotifyAboutGameReady()
        {
            OnGameReady?.Invoke(this);
        }

        protected void NotifyAboutGameStart()
        {
            isInitialized = true;
            OnGameStart?.Invoke(this);
        }

        public static T GetInteractor<T>() where T : IInteractor
        {
            return instance.interactorsBase.GetInteractor<T>();
        }

        public static IEnumerable<T> GetInteractors<T>() where T : IInteractor
        {
            return instance.interactorsBase.GetInteractors<T>();
        }

        public static T GetRepository<T>() where T : Repository
        {
            return instance.repositoriesBase.GetRepository<T>();
        }

        protected virtual void NotifyAboutStatusChanged(string statusText)
        {
            OnGameInitializingStatusChanged?.Invoke(instance, statusText);
        }
    }
}