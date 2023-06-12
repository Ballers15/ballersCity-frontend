using SinSity.Meta;
using SinSity.Tools;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIFWWidgetExclamationMark : UIWidgetExclamationMark {

        private UIPopupFortuneWheel popupFortuneWheel;
        private FortuneWheelInteractor interactor;
        private EcoClickerTimer timer;


        #region LIFECYCLE

        protected override void Awake() {
            base.Awake();
            
            this.timer = new EcoClickerTimer();
        }

        protected override void OnStart() {
            base.OnStart();
            
            this.interactor = this.GetInteractor<FortuneWheelInteractor>();
            var uiInteractor = this.GetInteractor<UIInteractor>();
            this.popupFortuneWheel = uiInteractor.uiController.GetUIElement<UIPopupFortuneWheel>();
            
            if (!this.interactor.isInitialized)
                this.interactor.OnInitializedEvent += this.OnFWInteractorInitialized;
            else
                this.UpdateState();
        }

        private void OnDestroy() {
            this.interactor.OnInitializedEvent -= this.OnFWInteractorInitialized;
            this.interactor.OnFortuneWheelUnlockedEvent -= this.OnFortuneWheelUnlocked;
            this.popupFortuneWheel.OnUIElementClosedCompletelyEvent -= this.OnPopupFortuneWheelClosedCompletely;
        }

        #endregion
        
       

        private void UpdateState() {
            if (!this.interactor.isUnlocked) {
                this.interactor.OnFortuneWheelUnlockedEvent -= this.OnFortuneWheelUnlocked;
                this.interactor.OnFortuneWheelUnlockedEvent += this.OnFortuneWheelUnlocked;
                this.Deactivate();
                return;
            }
            
            if (this.interactor.CanUseFreeSpin()) {
                this.StopTimer();
                this.Activate();
                return;
            }
            
            this.Deactivate();
            this.ActivateTimer();
        }

        private void ActivateTimer() {
            var secondsLeft = this.interactor.GetTimeToNextFreeSpin();
            this.timer.timerValue = secondsLeft;
            this.timer.Start();
            this.timer.OnTimerCompletedEvent += this.OnTimerCompleted;
        }

        private void StopTimer() {
            if (this.timer.isActive)
                this.timer.Stop();
            this.timer.OnTimerCompletedEvent -= this.OnTimerCompleted;
        }

       

        #region EVENTS
        
        private void OnFWInteractorInitialized(IInteractor iiteractor) {
            this.interactor.OnInitializedEvent -= this.OnFWInteractorInitialized;
            
            this.popupFortuneWheel.OnUIElementClosedCompletelyEvent += this.OnPopupFortuneWheelClosedCompletely;
            this.UpdateState();
        }

        private void OnPopupFortuneWheelClosedCompletely(UIElement uielement) {
            this.UpdateState();
        }
        
        private void OnTimerCompleted() {
            this.timer.OnTimerCompletedEvent -= this.OnTimerCompleted;
            this.UpdateState();
        }
        
        private void OnFortuneWheelUnlocked(FortuneWheelInteractor fortuneWheelInteractor) {
            this.interactor.OnFortuneWheelUnlockedEvent -= this.OnFortuneWheelUnlocked;
            this.UpdateState();
        }

        #endregion

    }
}