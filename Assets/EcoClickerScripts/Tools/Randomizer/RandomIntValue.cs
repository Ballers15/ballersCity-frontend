using System;
using Random = UnityEngine.Random;

namespace Ecorobotics {
    [Serializable]
    public struct RandomIntValue {
        public int valueMin;
        public int valueMax;

        public int GetValue() {
            return Random.Range(valueMin, valueMax + 1);
        }
    }
}