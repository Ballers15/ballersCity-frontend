using System;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta
{
    [Serializable]
    public sealed class LevelImprovementBlock {
        public int blockLevel;
        
        public int firstLevel;
        
        public int lastLevel;
        
        public RewardInfo rewardInfo;

        public float GetProgressNormalized(int level)
        {
            float levelsTotal = this.lastLevel - this.firstLevel;
            float levelHandled = level - this.firstLevel;
            return levelHandled / levelsTotal;
        }
    }
}