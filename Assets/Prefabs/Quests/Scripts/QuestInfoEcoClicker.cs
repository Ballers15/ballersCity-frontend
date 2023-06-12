using UnityEngine;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    public abstract class QuestInfoEcoClicker : QuestInfo
    {
        [SerializeField]
        private RewardInfo m_rewardInfo;

        public RewardInfo rewardInfo
        {
            get { return this.m_rewardInfo; }
        }

        public virtual bool CanCreateQuest()
        {
            return true;
        }
    }
}