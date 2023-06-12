using SinSity.Meta;
using SinSity.Tools;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWIdgetFortuneWheelDescription : UIWidget<UIWIdgetFortuneWheelDescription.Properties> {

        #region CONSTANTS

        private static readonly int boolFreeSpinAvailable = Animator.StringToHash("freeSpinAvailable");

        #endregion
        
        private FortuneWheelInteractor interactor;
        private EcoClickerTimer timerFreeSpinNotAvailable;

        protected override void Awake() {
            base.Awake();
            this.interactor = this.GetInteractor<FortuneWheelInteractor>();
            this.timerFreeSpinNotAvailable = new EcoClickerTimer();
        }

        private void OnEnable() {
            this.interactor.OnFortuneWheelStateChangedEvent += this.OnFortuneWheelStateChanged;
            this.UpdateState();
        }

        private void OnDisable() {
            this.interactor.OnFortuneWheelUnlockedEvent -= this.OnFortuneWheelStateChanged;

            if (this.timerFreeSpinNotAvailable.isActive) {
                this.timerFreeSpinNotAvailable.OnTimerValueChangedEvent -= this.OnTimerValueChanged;
                this.timerFreeSpinNotAvailable.OnTimerCompletedEvent -= this.OnTimerCompleted;
                this.timerFreeSpinNotAvailable.Stop();
            }
        }

        private void UpdateState() {
            if (!this.interactor.isInitialized)
                return;
            
            var freeSpinAvailable = this.interactor.CanUseFreeSpin();
            this.properties.animator.SetBool(boolFreeSpinAvailable, freeSpinAvailable);
            
            if (!freeSpinAvailable && !timerFreeSpinNotAvailable.isActive)
                this.ActivateTimer();
                
        }

        private void ActivateTimer() {
            if (this.timerFreeSpinNotAvailable.isActive)
                return;
        
            var totalSeconds = this.interactor.GetTimeToNextFreeSpin();
            this.timerFreeSpinNotAvailable.timerValue = totalSeconds;
            this.timerFreeSpinNotAvailable.Start();
            this.timerFreeSpinNotAvailable.OnTimerValueChangedEvent += this.OnTimerValueChanged;
            this.timerFreeSpinNotAvailable.OnTimerCompletedEvent += this.OnTimerCompleted;
            this.UpdateTimerText();
        }

        private void OnTimerCompleted() {
            this.UpdateState();
            this.timerFreeSpinNotAvailable.OnTimerValueChangedEvent -= this.OnTimerValueChanged;
            this.timerFreeSpinNotAvailable.OnTimerCompletedEvent -= this.OnTimerCompleted;
        }

        private void OnTimerValueChanged() {
            this.UpdateTimerText();
        }

        private void UpdateTimerText() {
            var totalSeconds = this.timerFreeSpinNotAvailable.timerValue;
            this.properties.textFreeSpinTimer.text = GameTime.ConvertToFormatHMS(totalSeconds);
        }
        
        

        #region EVENTS

        private void OnFortuneWheelStateChanged(FortuneWheelInteractor fortuneWheelInteractor) {
            this.UpdateState();
        }

        #endregion


        [System.Serializable]
        public class Properties : UIProperties {
            public Animator animator;
            public Text textFreeSpinTimer;
        }
        
    }
}