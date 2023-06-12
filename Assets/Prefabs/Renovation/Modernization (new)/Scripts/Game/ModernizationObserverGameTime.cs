using UnityEngine.Events;
using VavilichevGD.Tools;
using VavilichevGD.Architecture;
using System;
using SinSity.Meta;
using SinSity.Core;


namespace SinSity.Extensions
{
    public class ModernizationObserverGameTime : IModernizationObserver
    {

        #region DELEGATES

        public event UnityAction<object, int> OnScoresReceivedEvent;

        #endregion

        private ModernizationConfig config;
        private int secondsCount = 0;

        public ModernizationObserverGameTime(ModernizationConfig config)
        {
            this.config = config;
        }

        public void Subscribe()
        {
            GameTime.OnSecondTickEvent += onGameTime;
        }

        public void Unsubscribe()
        {
            GameTime.OnSecondTickEvent -= onGameTime;
        }

        #region EVENTS

        public void onGameTime()
        {
            this.secondsCount++;

            if (this.secondsCount == config.timePeriodForScoresReward)
            {
                this.secondsCount = 0;
                OnScoresReceivedEvent?.Invoke(this, config.scoresForTimePeriod);
            }

            return;
        }

        #endregion

    }
}