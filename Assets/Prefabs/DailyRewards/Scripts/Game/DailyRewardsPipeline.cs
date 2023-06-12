using System.Collections.Generic;
using System.Linq;
using SinSity.Meta.Rewards;
using UnityEngine;

namespace VavilichevGD.Meta.Rewards
{
    [CreateAssetMenu(
        fileName = "DailyRewardsPipeline",
        menuName = "DailyRewards/New DailyRewardsPipeline"
    )]
    public sealed class DailyRewardsPipeline : ScriptableObject
    {
        [SerializeField]
        private RewardInfoEcoClicker[] dailyRewards;

        private readonly Dictionary<string, int> dailyRewardIdVsIndexMap;

        public int dayCount
        {
            get { return this.dailyRewards.Length; }
        }

        public DailyRewardsPipeline()
        {
            this.dailyRewardIdVsIndexMap = new Dictionary<string, int>();
        }

        private void Awake()
        {
            for (var index = 0; index < this.dailyRewards.Length; index++)
            {
                var dailyReward = this.dailyRewards[index];
                var dailyRewardId = dailyReward.id;
                this.dailyRewardIdVsIndexMap[dailyRewardId] = index;
            }
        }

        public IEnumerable<RewardInfoEcoClicker> GetRewardInfos()
        {
            return this.dailyRewards.ToList();
        }

        public RewardInfoEcoClicker GetReward(string id)
        {
            var questIndex = this.dailyRewardIdVsIndexMap[id];
            return this.GetReward(questIndex);
        }

        public RewardInfoEcoClicker GetReward(int index)
        {
            return this.dailyRewards[index];
        }

        public bool HasNextReward(string id, out RewardInfoEcoClicker nextRewardInfo)
        {
            var questIndex = this.dailyRewardIdVsIndexMap[id];
            return this.HasNextReward(questIndex, out nextRewardInfo);
        }

        public bool HasNextReward(int currentIndex, out RewardInfoEcoClicker nextRewardInfo)
        {
            var nextIndex = currentIndex + 1;
            if (nextIndex >= this.dailyRewards.Length)
            {
                nextRewardInfo = null;
                return false;
            }

            nextRewardInfo = this.GetReward(nextIndex);
            return true;
        }
    }
}