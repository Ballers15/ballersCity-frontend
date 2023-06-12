using UnityEngine.Events;
using VavilichevGD.Tools;
using VavilichevGD.Architecture;
using System;
using SinSity.Meta;
using SinSity.Core;


namespace SinSity.Extensions
{
    public class ModernizationObserverIdleBuild : IModernizationObserver
    {

        #region DELEGATES

        public event UnityAction<object, int> OnScoresReceivedEvent;

        #endregion

        private ModernizationConfig config;

        public ModernizationObserverIdleBuild(ModernizationConfig config)
        {
            this.config = config;
        }

        public void Subscribe()
        {
            IdleObject.OnIdleObjectBuilt += this.OnIdleObjectBuilt;
        }

        public void Unsubscribe()
        {
            IdleObject.OnIdleObjectBuilt -= this.OnIdleObjectBuilt;
        }


        #region EVENTS

        public void OnIdleObjectBuilt(IdleObject idleobject, IdleObjectState state)
        {
            OnScoresReceivedEvent?.Invoke(this, config.scoresForObjectBuilding);
            return;
        }

        #endregion

    }
}