using System.Collections.Generic;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core
{
    public abstract class ProfileLevelUnlockConfig : ScriptableObject
    {
        public virtual LevelUp GenerateLevelUp(int reachLevel)
        {
            var levelUp = new LevelUp
            {
                level = reachLevel,
                rewards = this.GenerateRewards()
            };
            return levelUp;
        }

        protected abstract IEnumerable<Reward> GenerateRewards();
    }
}