using System.Collections;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using SinSity.Data;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.Meta.Rewards {
    public sealed class DailyRewardRepository : Repository {
        #region Const

        private const string PREF_KEY_LAST_REWARD_DATA = "DAILY_REWARD_DATA";

        #endregion

        private DailyRewardsData data;
        
        protected override IEnumerator InitializeRepositoryRoutine() {
            using var request = new GameDataRequest(PREF_KEY_LAST_REWARD_DATA);
            
            yield return request.Send(RequestType.GET);
            
            data = request.GetGameData(DailyRewardsData.defaultValue);
        }

        public void SetLastDailyRewardData(DailyRewardsData newDailyRewardDate) {
            data = newDailyRewardDate;
            Save();
            //Storage.SetCustom(PREF_KEY_LAST_REWARD_DATA, newDailyRewardDate);
        }

        public override void Save() {
            if (isSavingInProcess) return;
            Coroutines.StartRoutine(SaveAsync());
        }

        private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            using var request = new GameDataRequest(PREF_KEY_LAST_REWARD_DATA, data);
            
            yield return request.Send();
            
            isSavingInProcess = false;
        }

        public DailyRewardsData GetLastDailyRewardData() {
            return data;
        }

        public override void Reset() {
            Storage.ClearKey(PREF_KEY_LAST_REWARD_DATA);
        }
    }
}