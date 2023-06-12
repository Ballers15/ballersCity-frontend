using System;
using VavilichevGD.Tools;

namespace SinSity.Meta
{
    [Serializable]
    public class UIPopupRateUsData
    {
        public int showedTimes;
        public DateTimeSerialized timeNextShowing;

        public UIPopupRateUsData()
        {
            this.showedTimes = 0;
            this.timeNextShowing = new DateTimeSerialized(DateTime.Now);
        }
    }
}