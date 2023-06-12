using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Domain;
using SinSity.Scripts;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Core
{
    public sealed class GameManager : MonoBehaviour
    {
        #region Const

        private const int TARGET_FRAME_RATE = 60;

        #endregion

        public static GameManager instance { get; private set; }
        
        private bool isLaunched;

        private SaveGameInteractor saveGameInteractor;

        private IEnumerable<IUpdateListenerInteractor> updateInteractors;

        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            Game.OnGameStart += this.OnGameStart;
        }

        private void Start()
        {
            GameEcoClicker.Run();
            Application.targetFrameRate = TARGET_FRAME_RATE;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void OnGameStart(Game game)
        {
            this.saveGameInteractor = Game.GetInteractor<SaveGameInteractor>();
            this.updateInteractors = Game.GetInteractors<IUpdateListenerInteractor>();
            this.StartCoroutine(this.LaunchUpdate());
        }

        private IEnumerator LaunchUpdate()
        {
            var offlineTime = (float) GameTime.timeSinceLastSessionEndedToCurrentSessionStartedSeconds;
            var offlineSeconds = Mathf.FloorToInt(offlineTime);
            var rewindTimeIntent = new RewindTimeIntentGameStarted(offlineSeconds);
            var rewindTimeInteractor = Game.GetInteractor<RewindTimeInteractor>();
            yield return rewindTimeInteractor.RewindTime(rewindTimeIntent);
            this.isLaunched = true;
        }

        private void Update()
        {
            if (!this.isLaunched)
            {
                return;
            }

            if (GameTime.isPaused)
            {
                return;
            }

            var unscaledDeltaTime = Time.unscaledDeltaTime;
            foreach (var updateInteractor in this.updateInteractors)
            {
                updateInteractor.OnUpdate(unscaledDeltaTime);
            }
        }

        private void OnApplicationPause(bool pauseStatus) {
            if (!pauseStatus)
                return;
            
            if (!Game.isInitialized)
                return;
            
            this.saveGameInteractor?.SaveAll();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!Game.isInitialized)
            {
                return;
            }

            if (hasFocus)
            {
                GameEcoClicker.UnpauseSession();
            }
            else
            {
                this.saveGameInteractor?.SaveAll();
                GameEcoClicker.PauseSession();
            }

            CommonAnalytics.LogGamePause(GameTime.timeSinceGameStarted - GameTime.timeSinceGameStarted % 30, hasFocus);
        }

        private void OnDisable()
        {
            Game.OnGameStart -= this.OnGameStart;
            this.saveGameInteractor?.SaveAll();
        }

        public static void ResetEndExit()
        {
            PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }
    }
}