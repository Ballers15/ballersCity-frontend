using System.Collections;
using System.Collections.Generic;
//using Facebook.Unity;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Core
{
    public sealed class IdleObjectRewindTimeInteractor : Interactor, IRewindTimeAsyncListenerInteractor
    {
        private IdleObjectsInteractor idleObjectsInteractor;

        private IEnumerable<IIdleObjectExternalMultipliersHolderInteractor> externalMultipliersHolders;

        public IdleObjectCollectedOfflineCurrencyAggregator collectedOfflineCurrencyAggregator { get; private set; }

        public override bool onCreateInstantly { get; } = true;


        protected override void Initialize() {
            base.Initialize();
            this.collectedOfflineCurrencyAggregator = new IdleObjectCollectedOfflineCurrencyAggregator();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            this.idleObjectsInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            this.externalMultipliersHolders = Game.GetInteractors<IIdleObjectExternalMultipliersHolderInteractor>();
        }

        #region OnReady

        public override void OnReady()
        {
            base.OnReady();
            this.LoadExternalMultipliersForEachIdleObject();
        }


        #region LoadExternalMultipliersForEachIdleObject

        public void LoadExternalMultipliersForEachIdleObject()
        {
            var idleObjects = this.idleObjectsInteractor.GetIdleObjects();
            foreach (var idleObject in idleObjects)
            {
                this.LoadExternalMultipliersForIdleObject(idleObject);
            }
        }

        public void LoadExternalMultipliersForIdleObject(IdleObject idleObject)
        {
            var constCoefficientMap = new Dictionary<string, Coefficient>();
            var timeCoefficientList = new Dictionary<string, TimeCoefficient>();
            foreach (var multipliersHolder in this.externalMultipliersHolders)
            {
                var externConstCoefficientMap = multipliersHolder.GetStaticMultiplierMapBy(idleObject);
                //constCoefficientMap.AddAllKVPFrom(externConstCoefficientMap);
                foreach (var externConstCoef in externConstCoefficientMap) {
                    constCoefficientMap.Add(externConstCoef.Key, externConstCoef.Value);
                }
                var externTimeCoefficientMap = multipliersHolder.GetTimeMultiplierMapBy(idleObject);
                //timeCoefficientList.AddAllKVPFrom(externTimeCoefficientMap);
                foreach (var externTimeCoef in externTimeCoefficientMap) {
                    constCoefficientMap.Add(externTimeCoef.Key, externTimeCoef.Value);
                }
            }

            var idleObjectState = idleObject.state;
            idleObjectState.incomeConstMultiplicator.SetCoefficients(constCoefficientMap);
            idleObjectState.incomeTimeMultiplicator.SetCoefficients(timeCoefficientList);
        }

        #endregion

        #endregion

        #region OnRewindTimeAsync

        public IEnumerator OnRewindTimeAsync(RewindTimeIntent intent)
        {
            this.collectedOfflineCurrencyAggregator.Reset();
            yield return RewindTimeForEachIdleObject(intent.passedSeconds);
        }

        #region InstallOfflineIncome

        public IEnumerator RewindTimeForEachIdleObject(long passedOfflineSeconds)
        {
            var idleObjects = this.idleObjectsInteractor.GetIdleObjects();

            foreach (var idleObject in idleObjects)
            {
                yield return this.RewindTimeForIdleObject(idleObject, passedOfflineSeconds);
            }
        }

        public IEnumerator RewindTimeForIdleObject(IdleObject idleObject, long offlineSeconds)
        {
            var idleObjectState = idleObject.state;

            if (!idleObjectState.isBuilded)
            {
                yield break;
            }

            var lastCycleSeconds = idleObjectState.progressInTime;
            var totalSecondsPassed = offlineSeconds + lastCycleSeconds;
            var cyclesCount = Mathf.FloorToInt(totalSecondsPassed / idleObjectState.incomePeriod);
            var secondsLeft = idleObjectState.incomePeriod - totalSecondsPassed % idleObjectState.incomePeriod;
            var secondsPassed = idleObjectState.incomePeriod - secondsLeft;
            var progressNormalized = Mathf.Clamp01(secondsPassed / idleObjectState.incomePeriod);
            if (cyclesCount < 1)
            {
                idleObjectState.progressNomalized = progressNormalized;
                idleObject.timerValue = idleObjectState.progressNomalized * idleObjectState.incomePeriod;
                idleObject.NotifyAboutWorkStarted();
                yield break;
            }

            if (idleObject.autoplayEnabled)
            {
                var algorithmResult = IdleObjectOfflineTimeCalcIncomeAlgorithm
                    .CalcIncomeByPassedOfflineTime(idleObject, offlineSeconds);
                idleObjectState.collectedCurrency += algorithmResult;
                idleObjectState.progressNomalized = progressNormalized;
                idleObject.timerValue = idleObjectState.progressNomalized * idleObjectState.incomePeriod;
                idleObject.NotifyAboutWorkStarted();
                this.collectedOfflineCurrencyAggregator.Aggregate(idleObject, algorithmResult);
            }
            else
            {
                idleObjectState.progressNomalized = 1f;
                var collectedCurrencyBefore = idleObjectState.collectedCurrency;
                var collectedCurrencyOffline = idleObject.incomeCurrent - collectedCurrencyBefore;
                idleObjectState.collectedCurrency += collectedCurrencyOffline;
                this.collectedOfflineCurrencyAggregator.Aggregate(idleObject, collectedCurrencyOffline);
            }

            idleObject.NotifyAboutStateChanged();
            yield return new WaitForEndOfFrame();
        }

        #endregion

        #endregion
    }
}