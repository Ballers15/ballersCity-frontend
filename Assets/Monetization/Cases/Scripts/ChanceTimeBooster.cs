using System;
using SinSity.Meta.Rewards;
using UnityEngine;

namespace SinSity.Extensions {
    [Serializable]
    public class ChanceTimeBooster {
        public RewardInfoTimeBooster rewardInfoTimeBooster;
        [Range(0, 100)]
        public int chance = 50;

        [SerializeField] private int countMin;
        [SerializeField] private int countMax;
        
        public int randomCount => UnityEngine.Random.Range(countMin, countMax + 1);
    }
}