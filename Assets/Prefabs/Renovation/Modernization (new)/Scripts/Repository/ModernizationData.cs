using System;
using UnityEngine;

namespace SinSity.Repo {
    [Serializable]
    public class ModernizationData {
        public float multiplier;
        public int scores;
        public bool adapted;
        public bool isAvailable;
        public int renovationIndex;

        public int multiplierInPercent => Mathf.RoundToInt(this.multiplier * 100);

        public ModernizationData() {
            this.multiplier = 1f;
            this.scores = 0;
            this.renovationIndex = 0;
            this.adapted = false;
            this.isAvailable = false;
        }

    }
}