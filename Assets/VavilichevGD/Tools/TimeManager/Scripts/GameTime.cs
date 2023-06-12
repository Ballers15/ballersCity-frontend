using System;
using UnityEngine;
using static VavilichevGD.Tools.GameTime;

namespace VavilichevGD.Tools
{
    [ScriptOrder(-100)]
    public class GameTime : MonoBehaviour
    {

        #region DELEGATES

        public delegate void GameTimeHandler();
        public static event GameTimeHandler OnSecondTickEvent;

        public delegate void GamePauseHandler(bool paused);
        public static event GamePauseHandler OnGamePaused;

        public delegate void GameTimeInitializeHandler();
        public static event GameTimeInitializeHandler OnGameTimeInitialized;

        #endregion
        
        
        private static GameTime instance { get; set; }
        private static GameTimeInteractor interactor;

        public static bool isInitialized => interactor != null;
        public static float unscaledDeltaTime { get; private set; }
        public static float deltaTime { get; private set; }
        public static bool isPaused { get; private set; }
        public static DateTime now => interactor.now;
        public static GameSessionTimeData gameTimeDataCurrentSession => interactor.gameSessionTimeDataCurrentSession;
        public static GameSessionTimeData gameTimeDataLastSession => interactor.gameSettionTimeDataLastSession;

        /// <summary>
        /// Время оффлайна в секундах.
        /// </summary>

        public static double timeSinceLastSessionEndedToCurrentSessionStartedSeconds
        {
            get { return interactor.timeSinceLastSessionEndedToCurrentSessionStarted; }
        }

        public static double timeSinceGameStarted
        {
            get
            {
                return isInitialized
                    ? interactor.timeSinceGameStarted
                    : 0;
            }
        }

        public static double timeSinceGameInstalledHours
        {
            get
            {
                return isInitialized
                    ? (DateTime.Now - interactor.firstPlayTime).TotalHours
                    : 0;
            }
        }

        private float timer = 0f;      

        

        public static void Initialize(GameTimeInteractor _interactor)
        {
            if (instance != null)
                return;

            interactor = _interactor;
            CreateSingleton();
            Logging.Log("GAME TIME: is initialized.");
            OnGameTimeInitialized?.Invoke();
        }

        private static void CreateSingleton()
        {
            GameObject gameTimeGO = new GameObject("[GAME TIME]");
            instance = gameTimeGO.AddComponent<GameTime>();
            DontDestroyOnLoad(gameTimeGO);
        }


        private void Update()
        {
            unscaledDeltaTime = 0f;
            deltaTime = 0f;

            interactor.Update(Time.unscaledDeltaTime);

            if (!isPaused)
            {
                unscaledDeltaTime = Time.unscaledDeltaTime;
                deltaTime = Time.deltaTime;
            }

            timer += Time.unscaledDeltaTime;
            if (timer > 1f)
            {
                timer = timer - Mathf.FloorToInt(timer);
                OnSecondTickEvent?.Invoke();
            }
        }


        public static void Pause()
        {
            Time.timeScale = 0f;
            isPaused = true;
            NotifyAboutGamePauseStateChanged();
        }

        public static void SetTimeScale(float value) {
            Time.timeScale = Mathf.Max(value, 0f);
        }

        public static void ResetTimeScale() {
            Time.timeScale = 1f;
        }

        private static void NotifyAboutGamePauseStateChanged()
        {
            OnGamePaused?.Invoke(isPaused);
        }

        public static void Unpause()
        {
            Time.timeScale = 1f;
            isPaused = false;
            NotifyAboutGamePauseStateChanged();
        }

        public static void SwitchPauseState()
        {
            if (isPaused)
                Unpause();
            else
                Pause();
        }


        public static void Save()
        {
            interactor.Save();
        }

        private void OnApplicationPause(bool pauseStatus) {
            if (pauseStatus)
                Save();
        }

        private void OnApplicationFocus(bool hasFocus) {
            if (!hasFocus)
                Save();
        }

        private void OnDisable()
        {
            Save();
        }


        public static bool Equals(DateTime lastTime, DateTime previousTime, TimeSpan infelicity)
        {
            if (lastTime < previousTime)
            {
                DateTime tempDateTime = lastTime;
                lastTime = previousTime;
                previousTime = tempDateTime;
            }

            return (lastTime - previousTime).TotalSeconds <= infelicity.TotalSeconds;
        }

        public static string ConvertToFormatMS(int seconds) {
            int min = seconds / 60;
            int sec = seconds % 60;
            string strMin = min < 10 ? $"0{min}" : min.ToString();
            string strSec = sec < 10 ? $"0{sec}" : sec.ToString();
            return $"{strMin}:{strSec}";
        }

        public static string ConvertToFormatHMS(int seconds) {
            int hours = seconds / 3600;
            int min = (seconds / 60) % 60;
            int sec = seconds % 60;
            string strHours = hours < 10 ? $"0{hours}" : hours.ToString();
            string strMin = min < 10 ? $"0{min}" : min.ToString();
            string strSec = sec < 10 ? $"0{sec}" : sec.ToString();
            return $"{strHours}:{strMin}:{strSec}";
        }
        
        public static string ConvertToFormatHM(int seconds) {
            int hours = seconds / 3600;
            int min = (seconds / 60) % 60;
            string strHours = hours < 10 ? $"0{hours}" : hours.ToString();
            string strMin = min < 10 ? $"0{min}" : min.ToString();
            return $"{strHours}:{strMin}";
        }
    }
}