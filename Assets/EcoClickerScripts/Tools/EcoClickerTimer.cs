using System;
using VavilichevGD.Tools;

namespace SinSity.Tools {
    public class EcoClickerTimer {

        #region DELEGATES

        public delegate void EcoClickerTimerHandler();

        public event EcoClickerTimerHandler OnTimerStartedEvent;
        public event EcoClickerTimerHandler OnTimerPausedEvent;
        public event EcoClickerTimerHandler OnTimerCompletedEvent;
        public event EcoClickerTimerHandler OnTimerValueChangedEvent;

        #endregion
        
        public int timerValue { get; set; }
        public bool isActive { get; private set; }
        

        public EcoClickerTimer(int timerValue) {
            this.timerValue = timerValue;
            this.isActive = false;
        }

        public EcoClickerTimer() {
            this.timerValue = 0;
            this.isActive = false;
        }


        public void Start() {
            if (this.isActive)
                return;
            
            GameTime.OnSecondTickEvent += OnGameTimeSecondTick;
            this.isActive = true;
            this.OnTimerStartedEvent?.Invoke();
        }

        public void Pause() {
            if (!this.isActive)
                return;
                
            GameTime.OnSecondTickEvent -= this.OnGameTimeSecondTick;
            this.isActive = false;
            this.OnTimerPausedEvent?.Invoke();
        }
        
        public void Stop() {
            if (!this.isActive)
                return;
            
            GameTime.OnSecondTickEvent -= this.OnGameTimeSecondTick;
            this.isActive = false;
            this.timerValue = 0;
            this.OnTimerCompletedEvent?.Invoke();
        }

        #region EVENTS

        private void OnGameTimeSecondTick() {
            this.timerValue = Math.Max(this.timerValue - 1, 0);
            this.OnTimerValueChangedEvent?.Invoke();

            if (this.timerValue == 0)
                this.Stop();
        }

        #endregion


        ~EcoClickerTimer() {
            this.Stop();
        }
        
    }
}