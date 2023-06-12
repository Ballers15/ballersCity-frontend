using UnityEngine.Events;
using VavilichevGD.Tools;
using VavilichevGD.Architecture;
using System;
using SinSity.Core;
using SinSity.Meta;


namespace SinSity.Extensions
{
    public class ModernizationObserverIdleUpgrd : IModernizationObserver
    {

        #region DELEGATES

        public event UnityAction<object, int> OnScoresReceivedEvent;

        #endregion

        private ModernizationConfig config;

        public ModernizationObserverIdleUpgrd(ModernizationConfig config)
        {
            this.config = config;
        }

        public void Subscribe()
        {
            IdleObject.OnIdleObjectLevelStageRisen += this.OnIdleObjectUpgrade;
        }

        public void Unsubscribe()
        {
            IdleObject.OnIdleObjectLevelStageRisen -= this.OnIdleObjectUpgrade;
        }


        #region EVENTS

        private void OnIdleObjectUpgrade(IdleObject idleObject, LevelImprovementReward reward)
        {
            OnScoresReceivedEvent?.Invoke(this, config.scoresForObjectUpgradeStage);
            return;
        }

        #endregion

    }
}