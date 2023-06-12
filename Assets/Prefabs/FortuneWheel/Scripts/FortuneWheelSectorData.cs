using System;
using UnityEngine;
using VavilichevGD.Meta.Rewards;
using Random = UnityEngine.Random;

namespace SinSity.Meta {
    [Serializable]
    public class FortuneWheelSectorData {
        public float angle;
        [Range(0f, 100f)]
        public float chance;
        public RewardInfo[] rewardInfoArray;
        
        public RewardInfo GetRewardInfo() {
            var rIndex = Random.Range(0, this.rewardInfoArray.Length);
            return this.rewardInfoArray[rIndex];
        }
    }
}