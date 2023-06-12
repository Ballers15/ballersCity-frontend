using UnityEngine;

namespace VavilichevGD.Meta.Rewards
{
    public abstract class ScriptableRewardInfoBuilder : ScriptableObject, IRewardInfoBuilder
    {
        public abstract RewardInfo Build();
    }
}