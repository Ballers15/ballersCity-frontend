using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SinSity.Meta {
    [CreateAssetMenu(fileName = "FortuneWheelFirstThirdSpinConfig", menuName = "Meta/FortuneWheel/FortuneWheelFirstThirdSpinConfig", order = 0)]
    public class FortuneWheelFirstThirdSpinConfig : ScriptableObject {

        #region CONSTANTS

        protected const float MAX_ANGLE = 360f;

        #endregion


        [SerializeField] private int m_unlockSpinDays = 1;
        [SerializeField] private int m_freeSpinPeriod = 28800;
        [SerializeField] private int m_ADSpinCount = 1;
        [SerializeField] private int m_gemPriceSpinDefault = 3;
        [SerializeField] private int m_gemPriceStep = 2;
        [Space]
        [SerializeField] protected int m_sectorsCount;
        [SerializeField] protected float m_separatorAngle;
        [SerializeField] protected List<FortuneWheelSectorData> sectorsDataList;

        public int unlockSpinDays => this.m_unlockSpinDays;
        public int freeSpinPeriod => this.m_freeSpinPeriod;
        public int adSpinCount => this.m_ADSpinCount;
        public int gemPriceSpinDefault => this.m_gemPriceSpinDefault;
        public int gemPriceStep => this.m_gemPriceStep;
        public int sectorsCount => this.m_sectorsCount;
        public float separatorAngle => this.m_separatorAngle;

        

        public FortuneWheelSectorData GetSectorData(int index) {
            return sectorsDataList[index];
        }

        public float GetRandomAngle() {
            var chance = 0f;
            var rChance = Random.Range(0f, 100f);
            var count = this.sectorsDataList.Count;

            for (int i = 0; i < count; i++) {
                FortuneWheelSectorData data = this.sectorsDataList[i];
                chance += data.chance;
                if (chance >= rChance)
                    return data.angle;
            }
            
            throw new Exception("Something went wrong");
        }

        public FortuneWheelSectorData GetSectorDataByAngle(float angle) {
            foreach (var fortuneWheelSectorData in this.sectorsDataList) {
                if (Math.Abs(fortuneWheelSectorData.angle - angle) < Mathf.Epsilon)
                    return fortuneWheelSectorData;
            }
            throw new ArgumentException($"There is no data with angle: {angle}");
        }

        public FortuneWheelSectorData[] GetSectorsData() {
            return this.sectorsDataList.ToArray();
        }
        
        #if UNITY_EDITOR
        protected void OnValidate() {
            if (this.sectorsDataList == null)
                this.sectorsDataList = new List<FortuneWheelSectorData>();

            int count = this.sectorsDataList.Count;
            if (count == m_sectorsCount)
                return;

            this.RecalculateSectors();
        }

        protected void RecalculateSectors() {
            var sectorAndleSize = MAX_ANGLE / this.m_sectorsCount;

            if (this.sectorsDataList.Count > this.m_sectorsCount) {
                var difference = this.sectorsDataList.Count - this.m_sectorsCount;
                this.sectorsDataList.RemoveRange(this.m_sectorsCount, difference);    
            }

            float averageChance = 100f / this.m_sectorsCount;
            for (int i = 0; i < this.m_sectorsCount; i++) {
                var angle = i * sectorAndleSize;
                
                if (i >= this.sectorsDataList.Count) {
                    var sectorData = new FortuneWheelSectorData();
                    sectorData.angle = angle;
                    sectorData.chance = averageChance;
                    this.sectorsDataList.Add(sectorData);
                }
                else {
                    var sectorData = sectorsDataList[i];
                    sectorData.angle = angle;
                    sectorData.chance = averageChance;
                }
            }
        }
        
        public bool IsChancesValid() {
            float sumChances = 0f;
            foreach (var sectorData in this.sectorsDataList)
                sumChances += sectorData.chance;

            return Math.Abs(sumChances - 100f) < Mathf.Epsilon;
        }
#endif

    }
}