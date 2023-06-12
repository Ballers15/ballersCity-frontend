using System;
using UnityEngine;

namespace SinSity.Extensions {
    [Serializable]
    public class ChanceGroupTimeBoosters {
        [SerializeField] private ChanceTimeBooster[] values;

        public ChanceTimeBooster GetRandomChanceTimeBooster() {
            CheckValidation();

            int rChance = new System.Random().Next(0, 101);
            int length = values.Length;
            int chanceSum = 0;
            for (int i = 0; i < length; i++) {
                chanceSum += values[i].chance;
                if (rChance < chanceSum)
                    return values[i];
            }

            return null;
        }

        private void CheckValidation() {
            int maxSum = 100;
            int sum = 0;
            foreach (ChanceTimeBooster value in values) {
                sum += value.chance;
            }

            if (sum > maxSum)
                throw new Exception($"Sum of chances cannot be more than {maxSum}");
        }
    }
}