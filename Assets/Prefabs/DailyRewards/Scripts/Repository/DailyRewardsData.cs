using System;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.Tools;

namespace VavilichevGD.Meta
{
    [Serializable]
    public sealed class DailyRewardsData
    {
        [SerializeField]
        public bool isCompleted;

        [SerializeField]
        public int dailyRewardsDayIndex;

        [SerializeField]
        public DateTimeSerialized dailyRewardReceivedTimeSerialized;

        public DateTime dailyRewardReceivedTime
        {
            get { return this.dailyRewardReceivedTimeSerialized.GetDateTime(); }
        }

        public DailyRewardsData Clone()
        {
            return new DailyRewardsData
            {
                isCompleted = this.isCompleted,
                dailyRewardsDayIndex = this.dailyRewardsDayIndex,
                dailyRewardReceivedTimeSerialized = this.dailyRewardReceivedTimeSerialized.Clone()
            };
        }

        public static DailyRewardsData defaultValue
        {
            get { return GetDefault(); }
        }

        private static DailyRewardsData GetDefault()
        {
            var dateTime = new DateTime();
            var data = new DailyRewardsData
            {
                isCompleted = false,
                dailyRewardsDayIndex = 0,
                dailyRewardReceivedTimeSerialized = new DateTimeSerialized(dateTime)
            };
            return data;
        }
    }
}