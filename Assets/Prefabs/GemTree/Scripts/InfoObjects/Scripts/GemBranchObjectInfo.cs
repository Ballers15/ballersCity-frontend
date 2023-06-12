using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "GemBranchObjectInfo",
        menuName = "Game/GemTree/New GemBranchObjectInfo"
    )]
    public sealed class GemBranchObjectInfo : ScriptableObject
    {
        [SerializeField]
        public string id;

        [Header("Hours")]
        [SerializeField]
        private int minWaitMinutes = 14;

        [SerializeField]
        private int maxWaitMinutes = 16;

        [Header("Gems")]
        [SerializeField]
        private int gemCountDefault = 1;

        [Range(0, 100)]
        [SerializeField]
        private int jackpotProb = 10;

        [SerializeField]
        private int minGemJackpotCount = 2;

        [SerializeField]
        private int maxGemJackpotCount = 3;

        public int GetRandomMinutes()
        {
            var random = new Random();
            return random.Next(this.minWaitMinutes, this.maxWaitMinutes + 1);
        }

        public GemBranchReward GenerateGemRewardCount()
        {
            var random = new Random();
            var randomInt = random.Next(0, this.jackpotProb + 1);
            if (randomInt >= this.jackpotProb) {
                int gemsReward = random.Next(this.minGemJackpotCount, this.maxGemJackpotCount + 1);
                return new GemBranchReward(gemsReward, true);
            }

            return new GemBranchReward(this.gemCountDefault, false);
        }
    }
}