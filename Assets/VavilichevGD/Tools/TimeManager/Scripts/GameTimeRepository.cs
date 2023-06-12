using System;
using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using SinSity.Data;
using UnityEngine;
using VavilichevGD.Architecture;

namespace VavilichevGD.Tools {
    public class GameTimeRepository : Repository {
        #region Const

        private const string PREF_KEY_GAME_TIME_DATA = "PREF_KEY_GAME_TIME_DATA";
        private const string FIRST_PLAY_TIME_PREF_KEY = "PREF_KEY_FIRST_PLAY_TIME";

        #endregion

        public GameSessionTimeData gameSessionTimeDataLastSession { get; private set; }
        public DateTime firstPlayTime { get; private set; }

        #region Initialize

        protected override IEnumerator InitializeRepositoryRoutine() {
            using var dataRequest = new GameDataRequest(PREF_KEY_GAME_TIME_DATA);
            using var firstPlayTimeRequest = new GameDataRequest(FIRST_PLAY_TIME_PREF_KEY);
            
            yield return dataRequest.Send(RequestType.GET);
            yield return firstPlayTimeRequest.Send(RequestType.GET);

            gameSessionTimeDataLastSession = dataRequest.GetGameData(new GameSessionTimeData());
            var nowDateSerialized = new DateTimeSerialized(DateTime.Now);
            var firstPlayDateSerialized = firstPlayTimeRequest.GetGameData(nowDateSerialized);
            firstPlayTime = firstPlayDateSerialized.GetDateTime();
            if (nowDateSerialized.GetDateTime() == firstPlayTime) {
                Coroutines.StartRoutine(SaveFirstPlayDate());
            }
        }

        private IEnumerator SaveFirstPlayDate() {
            using var request = new GameDataRequest(FIRST_PLAY_TIME_PREF_KEY, new DateTimeSerialized(DateTime.Now));
            yield return request.Send();
        }

        #endregion

        public void SaveGameSession(GameSessionTimeData newData) {
            Coroutines.StartRoutine(SaveGameSessionRoutine(newData));
            Storage.SetCustom(PREF_KEY_GAME_TIME_DATA, newData);
        }

        private IEnumerator SaveGameSessionRoutine(GameSessionTimeData newData) {
            using var request = new GameDataRequest(PREF_KEY_GAME_TIME_DATA, newData);
            yield return request.Send();
        }
    }
}