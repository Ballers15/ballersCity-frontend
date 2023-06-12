using System;
using VavilichevGD.Tools;

namespace SinSity.Meta {
    [Serializable]
    public class FortuneWheelData {
        public bool isUnlocked;
        public int adSpinUsedCount;
        public int spinUsedCount;
        public int gemPriceCurrent;
        public DateTimeSerialized firstPlayTime;
        public DateTimeSerialized lastFreeSpinTimeSerialized;

        public FortuneWheelData(int gemPriceDefault, DateTime timeFirstPlay) {
            this.isUnlocked = false;
            this.adSpinUsedCount = 0;
            this.spinUsedCount = 0;
            this.gemPriceCurrent = gemPriceDefault;
            this.firstPlayTime = new DateTimeSerialized(timeFirstPlay);
            this.lastFreeSpinTimeSerialized = new DateTimeSerialized();
        }
    }
}