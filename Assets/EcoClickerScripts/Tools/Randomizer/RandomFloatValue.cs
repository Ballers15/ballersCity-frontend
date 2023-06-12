using System;
using Random = UnityEngine.Random;

namespace Ecorobotics {
    [Serializable]
    public struct RandomFloatValue {
        public float valueMin;
        public float valueMax;

        public float GetValue() {
            return Random.Range(valueMin, valueMax);
        }
    }
}