using System;
using VavilichevGD.Tools;

namespace SinSity.Data {
    [Serializable]
    public class DailyRewardsFromCharactersData {
        public DateTimeSerialized lastRewardCollectionTimeSerialized;

        public DailyRewardsFromCharactersData(DateTime lastRewardCollectionTimeSerialized) {
            this.lastRewardCollectionTimeSerialized = new DateTimeSerialized(lastRewardCollectionTimeSerialized);
        }
    }
}