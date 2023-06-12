using System;
using System.Collections;
using SinSity.Monetization;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;

namespace SinSity.Domain
{
    public sealed class TimeBoosterActivationInteractor : Interactor
    {
        #region Const

        private const int SECONDS_PER_HOUR = 3600;

        #endregion

        #region Event

        public event Action<ProductInfoTimeBooster> OnTimeBoosterActivatedEvent;

        #endregion

        public override bool onCreateInstantly { get; } = true;

        private RewindTimeInteractor rewindTimeInteractor;

        public override void OnInitialized()
        {
            base.OnInitialized();
            this.rewindTimeInteractor = this.GetInteractor<RewindTimeInteractor>();
        }


        public IEnumerator ActivateTimeBooster(Product timeBooster)
        {
            var timeBoosterInfo = timeBooster.GetInfo<ProductInfoTimeBooster>();
            var timeBoosterState = timeBooster.GetState<ProductStateTimeBooster>();
            var rewindSeconds = timeBoosterInfo.timeHours * SECONDS_PER_HOUR;
//            Debug.Log($"DETECTED TIME BOOSTER TIME: {rewindSeconds}!");
            timeBoosterState.SpendBooster();
            var rewindTimeIntent = new RewindTimeIntentTimeBooster(rewindSeconds);
//            Debug.Log("START TO REWIND TIME!");
            yield return this.rewindTimeInteractor.RewindTime(rewindTimeIntent);
//            Debug.Log("COMPLETE TO REWIND TIME!");
            CommonAnalytics.LogTimeBoosterUsed(timeBooster.id);
            this.OnTimeBoosterActivatedEvent?.Invoke(timeBoosterInfo);
        }
    }
}