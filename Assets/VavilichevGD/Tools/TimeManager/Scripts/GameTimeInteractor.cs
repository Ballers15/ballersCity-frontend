using System;
using System.Collections;
using UnityEngine;
using VavilichevGD.Architecture;

namespace VavilichevGD.Tools
{
    public class GameTimeInteractor : Interactor
    {
        #region Event

        public delegate void GameTimeInitializeHandler();

        public static event GameTimeInitializeHandler OnGameTimeInitialized;

        #endregion

        public GameSessionTimeData gameSessionTimeDataCurrentSession { get; }

        public double timeSinceLastSessionEndedToCurrentSessionStarted { get; private set; }
        
        public double timeSinceGameStarted { get; private set; }
        
        private  GameTimeRepository gameTimeRepository { get; set; }

        public GameSessionTimeData gameSettionTimeDataLastSession => this.gameTimeRepository.gameSessionTimeDataLastSession;

        public DateTime now => this.GetNowDateTime();

        public DateTime firstPlayTime => this.gameTimeRepository.firstPlayTime;
        public override bool onCreateInstantly { get; } = false;


        private DateTime GetNowDateTime()
        {
            var gameStartTime = this.gameSessionTimeDataCurrentSession.sessionStart;
            DateTime currentTime = gameStartTime.AddSeconds(this.timeSinceGameStarted);
            return currentTime;
        }
        
        public GameTimeInteractor() {
            this.gameSessionTimeDataCurrentSession = new GameSessionTimeData();
        }


        protected override void Initialize() {
            base.Initialize();
            this.gameTimeRepository = this.GetRepository<GameTimeRepository>();
        }

        protected override IEnumerator InitializeRoutineNew() {
            var timeLoader = new TimeLoader();
            timeLoader.OnTimeDownloaded += TimeLoader_OnTimeDownloaded;
            yield return timeLoader.LoadTime();
            GameTime.Initialize(this);
        }

        private void TimeLoader_OnTimeDownloaded(TimeLoader timeLoader, DownloadedTimeArgs e)
        {
            timeLoader.OnTimeDownloaded -= TimeLoader_OnTimeDownloaded;
            InitGameTimeSessionCurrent(e.downloadedTime);
            CalculateTimeLeftSinceLastSession(gameSettionTimeDataLastSession, gameSessionTimeDataCurrentSession);
            OnGameTimeInitialized?.Invoke();
        }

        private void InitGameTimeSessionCurrent(DateTime downloadedTime)
        {
            gameSessionTimeDataCurrentSession.sessionStartSerializedFromServer.SetDateTime(downloadedTime);
            DateTime deviceTime = DateTime.Now.ToUniversalTime();
            gameSessionTimeDataCurrentSession.sessionStartSerializedFromDevice.SetDateTime(deviceTime);
            gameSessionTimeDataCurrentSession.timeValueActiveDeviceAtStart = GetDeviceWorkTimeInSeconds();
        }

        private long GetDeviceWorkTimeInSeconds()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass systemClock = new AndroidJavaClass("android.os.SystemClock");
			return Mathf.FloorToInt(systemClock.CallStatic<long>("elapsedRealtime") / 1000f);
//#elif UNITY_IOS && !UNITY_EDITOR
//			return IOSTime.GetSystemUpTime() / 1000f;
#else
            int deviceRunTimeTicks = Environment.TickCount & Int32.MaxValue;
            int totalSeconds = Mathf.FloorToInt(deviceRunTimeTicks / 1000f);
            return totalSeconds;
#endif
        }

        private void CalculateTimeLeftSinceLastSession(GameSessionTimeData timeDataLastSession,
            GameSessionTimeData timeDataCurrentSession)
        {
            if (timeDataLastSession == null)
            {
                timeSinceLastSessionEndedToCurrentSessionStarted = 0;
                return;
            }

            timeSinceLastSessionEndedToCurrentSessionStarted =
                timeDataCurrentSession.timeValueActiveDeviceAtStart - timeDataLastSession.timeValueActiveDeviceAtEnd;
            if (timeSinceLastSessionEndedToCurrentSessionStarted < 0f)
            {
                timeSinceLastSessionEndedToCurrentSessionStarted = Mathf.FloorToInt(
                    (float) (timeDataCurrentSession.sessionStart - timeDataLastSession.sessionOver).TotalSeconds);
                timeSinceLastSessionEndedToCurrentSessionStarted =
                    Mathf.Max((float) timeSinceLastSessionEndedToCurrentSessionStarted, 0f);
            }
        }


        public override string ToString()
        {
            return $"Last session: {gameSettionTimeDataLastSession}\n\n" +
                   $"Current session: {gameSessionTimeDataCurrentSession}\n\n" +
                   $"Time between sessions: {timeSinceLastSessionEndedToCurrentSessionStarted}";
        }

        public void Save()
        {
            gameSessionTimeDataCurrentSession.sessionDuration = timeSinceGameStarted;
            gameSessionTimeDataCurrentSession.timeValueActiveDeviceAtEnd = GetDeviceWorkTimeInSeconds();
            gameTimeRepository.SaveGameSession(gameSessionTimeDataCurrentSession);
        }


        public void Update(float unscaledDeltaTime)
        {
            timeSinceGameStarted += unscaledDeltaTime;
        }
    }
}