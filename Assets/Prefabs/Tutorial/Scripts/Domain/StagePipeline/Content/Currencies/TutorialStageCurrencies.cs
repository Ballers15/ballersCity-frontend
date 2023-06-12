using SinSity.Core;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "TutorialStageCurrencies",
        menuName = "Domain/Tutorial/TutorialStageCurrencies"
    )]
    public sealed class TutorialStageCurrencies : TutorialStageController
    {
        [SerializeField] private int m_needCollectTimes = 1;

        public int currentCollectTimes { get; private set; }

        public override void OnBeginListen() {
            IdleObject.OnIdleObjectCurrencyCollected += OnIdleObjectCurrencyCollected;
            currentCollectTimes = 0;
        }

        public override void OnContinueListen(string currentStageJson) {
            IdleObject.OnIdleObjectCurrencyCollected += OnIdleObjectCurrencyCollected;
            currentCollectTimes = 0;
        }

        public override void OnEndListen() {
            IdleObject.OnIdleObjectCurrencyCollected -= OnIdleObjectCurrencyCollected;
        }

        #region Event

        private void OnIdleObjectCurrencyCollected(object sender, BigNumber collectedcurrency) {
            currentCollectTimes++;
            if (currentCollectTimes >= m_needCollectTimes) {
                var uiInteractor = Game.GetInteractor<UIInteractor>();
                var popup = uiInteractor.GetUIElement<UIPopupCardsCollection>();
                popup.OnUIElementClosedCompletelyEvent += OnPopupClosed;
            }
            else {
                NotifyAboutStateChanged();
            }

            TutorialAnalytics.LogCollectTimesFromFirstBuildingInSecondStage(currentCollectTimes);
        }

        private void OnPopupClosed(UIElement uielement) {
            uielement.OnUIElementClosedCompletelyEvent -= OnPopupClosed;
            NotifyAboutTriggered();
        }

        #endregion
    }
}