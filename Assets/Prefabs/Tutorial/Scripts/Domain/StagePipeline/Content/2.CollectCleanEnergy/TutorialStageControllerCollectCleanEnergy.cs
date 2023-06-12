using SinSity.Core;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "TutorialStageControllerCollectCleanEnergy",
        menuName = "Domain/Tutorial/TutorialStageControllerCollectCleanEnergy"
    )]
    public sealed class TutorialStageControllerCollectCleanEnergy : TutorialStageController
    {
        [SerializeField]
        private int m_needCollectTimes = 2;

        public int currentCollectTimes { get; private set; }

        public override void OnBeginListen()
        {
            IdleObject.OnIdleObjectCurrencyCollected += this.OnIdleObjectCurrencyCollected;
            this.currentCollectTimes = 0;
        }

        public override void OnContinueListen(string currentStageJson)
        {
            IdleObject.OnIdleObjectCurrencyCollected += this.OnIdleObjectCurrencyCollected;
            this.currentCollectTimes = 0;
        }

        public override void OnEndListen()
        {
            IdleObject.OnIdleObjectCurrencyCollected -= this.OnIdleObjectCurrencyCollected;
        }

        #region Event

        private void OnIdleObjectCurrencyCollected(object sender, BigNumber collectedcurrency)
        {
            if (this.currentCollectTimes + 1 >= this.m_needCollectTimes)
            {
                this.currentCollectTimes++;
                this.NotifyAboutTriggered();
            }
            else
            {
                this.currentCollectTimes++;
                this.NotifyAboutStateChanged();
            }

            TutorialAnalytics.LogCollectTimesFromFirstBuildingInSecondStage(this.currentCollectTimes);
        }

        #endregion
    }
}