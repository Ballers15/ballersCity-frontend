using System;
using UnityEngine;


namespace SinSity.Extensions {
    [Serializable]
    public class ChanceGroupWithCount {
        [SerializeField] private ChanceWithCount[] values;

        public int GetRandomCount() {
            CheckValidation();
            
            int rChance = new System.Random().Next(0, 101);
            int length = values.Length;
            int chanceSum = 0;
            for (int i = 0; i < length; i++) {
                chanceSum += values[i].chance;
                if (rChance <= chanceSum)
                    return values[i].randomCount;
            }

            return 0;
        }

        private void CheckValidation() {
            int maxSum = 100;
            int sum = 0;
            foreach (ChanceWithCount value in values) {
                sum += value.chance;
            }
            if (sum > maxSum)
                throw new Exception($"Sum of chances cannot be more than {maxSum}");
        }
    }
}