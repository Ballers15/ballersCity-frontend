using System;
using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using SinSity.Data;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public class DailyRewardsFromCharactersRepository : Repository {
        private const string PREF_KEY = "DAILY_REWARDS_FROM_CHARACTERS_DATA";

        private DailyRewardsFromCharactersData data;
        

        public void SaveData(DailyRewardsFromCharactersData dataToSave) {
            data = dataToSave;
            Save();
        }

        public override void Save() {
            if (isSavingInProcess) return;
            Coroutines.StartRoutine(SaveAsync());
        }
        
        private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            using var request = new GameDataRequest(PREF_KEY, data);
            
            yield return request.Send();
            
            isSavingInProcess = false;
        }
        
        public IEnumerator LoadData(DateTime firstTimeInitializationDate) {
            using var request = new GameDataRequest(PREF_KEY);

            yield return request.Send(RequestType.GET);

            data = request.GetGameData(new DailyRewardsFromCharactersData(firstTimeInitializationDate));
        }

        public void ClearData() {
            Storage.ClearKey(PREF_KEY);
        }

        public DailyRewardsFromCharactersData GetData() {
            return data;
        }
    }
}