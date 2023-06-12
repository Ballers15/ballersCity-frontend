using System;
using Orego.Util;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;

namespace SinSity.Core
{
    [Serializable]
    public sealed class AirShipSpawnParams
    {
        #region Const

        private const float ZERO = 0.0f;

        private static readonly Vector3 CENTER_VECTOR = new Vector3(0.5f, 0.5f);

        #endregion

        [SerializeField] private int standardSpawnPeriod = 20;
        [SerializeField] private int standardSpawnPeriodDeviation = 10;
        [SerializeField] private int extraSpawnPeriod = 10;
        [SerializeField] private int extraSpawnPerionDeviation = 3;
        [Range(0, 0.5f)] 
        [SerializeField] private float centerDeviationX = 0.25f;
        [Range(-0.5f, 0.5f)]
        [SerializeField] private float maxDeviationY = 0.25f;
        [SerializeField] public bool randomX = true;

        public float GetStandardRandomSpawnPeriod()
        {
            return GetRandomPeriod(this.standardSpawnPeriod, this.standardSpawnPeriodDeviation);
        }

        public float GetExtraRandomSpawnPeriod()
        {
            return GetRandomPeriod(this.extraSpawnPeriod, this.extraSpawnPerionDeviation);
        }

        public Vector2 GetRandomRelativePosition()
        {
            var sign = (float) OregoIntUtils.RandomSign();
            var deviationXInPercent = (int) this.centerDeviationX * 100;
            var randomDeviation = OregoIntUtils.Random(0, deviationXInPercent) / 100.0f;
            var resultDeviation = sign * randomDeviation;
            var randomDeviationY = ZERO + UnityEngine.Random.Range(0f, maxDeviationY);
            return CENTER_VECTOR + new Vector3(resultDeviation, randomDeviationY);
        }

        private static float GetRandomPeriod(int period, int deviation)
        {
            var sign = OregoIntUtils.RandomSign();
            var resultDeviation = sign * OregoIntUtils.Random(0, deviation);
            return period + resultDeviation;
        }
    }
}