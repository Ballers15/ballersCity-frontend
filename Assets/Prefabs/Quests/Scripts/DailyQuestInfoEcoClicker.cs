using UnityEngine;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Meta.Rewards;


namespace SinSity.Core
{
    public abstract class DailyQuestInfo : QuestInfoEcoClicker
    {
        public enum Difficulty
        {
            easy,
            normal,
            hard
        };

        [SerializeField]
        public Difficulty difficulty;
    }
}
