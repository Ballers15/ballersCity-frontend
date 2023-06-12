using System;
using System.Collections.Generic;
using SinSity.Domain.Utils;
using SinSity.Services;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "AirShipParcelSupplier",
        menuName = "Game/AirDrop/New AirShipParcelSupplier"
    )]
    public sealed class AirShipParcelSupplier : ScriptableObject
    {
        #region Event

        public event Action<int> OnLuckyIndexChangedEvent;

        public event Action OnLuckyPipelineFinishedEvent;

        #endregion

        public int luckyIndex { get; set; }

        public bool isLuckyModeEnabled { get; set; }

        [SerializeField] private StandardParams standardParams;
        [SerializeField] private LuckyPipeline luckyPipeline;
        [SerializeField] private StandardPipeline standardPipeline;
        [Space] 
        [SerializeField] private bool jackpotNotRequired;

        private int standardParcelPointer;

        private int standardParcelRange;

        private bool isSkipParcelRequired;

        public void Initialize()
        {
            this.standardParcelRange = this.standardParams.GetRandomRange();
            this.standardPipeline.Initialize();
        }

        public AirDropParcel SupplyNextParcel() {
            return this.SupplyNextStandardParcel();
        }

        public void SkipParcel()
        {
            this.isSkipParcelRequired = true;
        }

        public void NotifyAboutParcelUsed()
        {
            if (!this.isLuckyModeEnabled)
            {
                return;
            }

            this.isSkipParcelRequired = false;
            this.luckyIndex++;
            this.OnLuckyIndexChangedEvent?.Invoke(this.luckyIndex);
            var queueSize = this.luckyPipeline.GetSize();
            if (this.luckyIndex < queueSize)
            {
                return;
            }

            this.isLuckyModeEnabled = false;
            this.OnLuckyPipelineFinishedEvent?.Invoke();
        }

        private AirDropParcel SupplyNextLuckyParcel()
        {
            if (!this.isSkipParcelRequired)
            {
                return this.luckyPipeline.GetParcel(this.luckyIndex);
            }

            this.isSkipParcelRequired = false;
            this.luckyIndex++;
            this.OnLuckyIndexChangedEvent?.Invoke(this.luckyIndex);
            var queueSize = this.luckyPipeline.GetSize();
            if (this.luckyIndex < queueSize)
            {
                return this.luckyPipeline.GetParcel(this.luckyIndex);
            }

            this.isLuckyModeEnabled = false;
            this.OnLuckyPipelineFinishedEvent?.Invoke();
            return SupplyNextStandardParcel();

        }
        
        private AirDropParcel SupplyNextStandardParcel()
        {
            var parcel = new AirDropParcel();
            parcel.needWatchAds = false;
            parcel.rewardInfoBuilder = standardPipeline.GetRandomNoAdsBuilder();
            standardParcelPointer++;
            return parcel;
        }
        
        private bool IsJackpotRequired() {
            if (this.jackpotNotRequired)
                return false;
            
            var idleObjectsInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            if (!idleObjectsInteractor.HasMostCheapNotBuiltIdleObject(out var mostCheapIdleObject))
            {
                return false;
            }

            var price = mostCheapIdleObject.info.priceToBuild;
            var requiredCurrencyForBuild = price - Bank.softCurrencyCount - idleObjectsInteractor
                .GetAllCollectedIdleObjectCurrency();
            var jackpotBuilder = this.standardPipeline.cleanEnergyJackpotBuilder;
            var seconds = jackpotBuilder.secondCoefficent;
            return idleObjectsInteractor.GetFullIncomeByIdleObjectsForTime(seconds) >= requiredCurrencyForBuild;
        }

        [Serializable]
        public sealed class LuckyPipeline
        {
            [SerializeField]
            private AirDropParcel[] parcelArray;

            public AirDropParcel GetParcel(int index)
            {
                return this.parcelArray[index];
            }

            public int GetSize()
            {
                return this.parcelArray.Length;
            }
        }

        [Serializable]
        public sealed class StandardPipeline
        {
            [SerializeField]
            private RewardBuilderProb[] noAdsRewardInfoBuilderProbArray;

            [SerializeField]
            private RewardBuilderProb[] adsRewardInfoBuilderProbArray;

            public List<ScriptableRewardInfoBuilder> noAdsBuilderPool { get; }

            public List<ScriptableRewardInfoBuilder> adsBuilderPool { get; }
            
            public AirDropCleanEnergyRewardInfoBuilder cleanEnergyJackpotBuilder { get; private set; }

            public StandardPipeline()
            {
                this.noAdsBuilderPool = new List<ScriptableRewardInfoBuilder>();
                this.adsBuilderPool = new List<ScriptableRewardInfoBuilder>();
            }

            public void Initialize()
            {
                foreach (var builderProb in this.noAdsRewardInfoBuilderProbArray)
                {
                    var builder = Instantiate(builderProb.rewardInfoBuilder);
                    builderProb.probability.Times(() => this.noAdsBuilderPool.Add(builder));
                }

                var maxSecondCoefficient = 0;
                foreach (var builderProb in this.adsRewardInfoBuilderProbArray)
                {
                    var builder = Instantiate(builderProb.rewardInfoBuilder);
                    builderProb.probability.Times(() => this.adsBuilderPool.Add(builder));
                    if (builder is AirDropCleanEnergyRewardInfoBuilder cleanEnergyBuilder &&
                        cleanEnergyBuilder.secondCoefficent >= maxSecondCoefficient)
                    {
                        maxSecondCoefficient = cleanEnergyBuilder.secondCoefficent;
                        this.cleanEnergyJackpotBuilder = cleanEnergyBuilder;
                    }
                }
            }

            public ScriptableRewardInfoBuilder GetRandomNoAdsBuilder()
            {
                return this.noAdsBuilderPool.GetRandom();
            }

            public ScriptableRewardInfoBuilder GetRandomAdsBuilder()
            {
                return this.adsBuilderPool.GetRandom();
            }

            [Serializable]
            public sealed class RewardBuilderProb
            {
                [SerializeField]
                [Range(0, 100)]
                public int probability;

                [SerializeField]
                public ScriptableRewardInfoBuilder rewardInfoBuilder;
            }
        }

        [Serializable]
        public sealed class StandardParams
        {
            [SerializeField]
            private int adsRewardNumber = 6;

            [SerializeField]
            private int adsRewardDeviation = 2;

            public int GetRandomRange()
            {
                var sign = OregoIntUtils.RandomSign();
                var randomDeviation = OregoIntUtils.Random(0, this.adsRewardDeviation);
                var resultDeviation = sign * randomDeviation;
                return this.adsRewardNumber + resultDeviation;
            }
        }
    }
}