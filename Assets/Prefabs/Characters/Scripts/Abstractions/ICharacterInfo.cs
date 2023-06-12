using UnityEngine;

namespace SinSity.Core {
    public interface ICharacterInfo {
        string id { get; }
        string description { get; }
        IdleObjectInfo idleObjectInfo { get; }
        CardInfo characterCardInfo { get; }
        float characterIncomeMultiplier { get; }
        RewardWithChanceWeight[] rewardsWithWeights { get; }
    }
}