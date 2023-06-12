using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;

namespace IdleClicker.Gameplay {
    public class CoffeeBoost {

        #region DELEGATES

        public delegate void CoffeeBoostHandler(CoffeeBoost coffeeBoost);
        public event CoffeeBoostHandler OnStartedEvent;
        public event CoffeeBoostHandler OnEndedEvent;

        #endregion
        
        public bool isActive { get; private set; }
        public int level { get; private set; }
        
        public CoffeeBoostConfig[] configs { get; }
        public CoffeeBoostConfig actualConfig => configs[level];
        private Coroutine lifeRoutine;

        public CoffeeBoost() {
            this.configs = Resources.LoadAll<CoffeeBoostConfig>("Config");
            level = 0;
        }

        public void Activate() {
            if (this.isActive)
                this.Deactivate();

            var config = configs[level];
            this.lifeRoutine = Coroutines.StartRoutine(this.LifeRoutine(config));
            this.isActive = true;
            
            level = Mathf.Min(level + 1, configs.Length - 1);
            this.OnStartedEvent?.Invoke(this);
        }

        private IEnumerator LifeRoutine(CoffeeBoostConfig config) {
            yield return new WaitForSecondsRealtime(config.duration);
            this.Deactivate();
        }

        public void Deactivate() {
            if (this.lifeRoutine != null) {
                Coroutines.StopRoutine(this.lifeRoutine);
                this.lifeRoutine = null;
            }

            this.isActive = false;
            
            this.OnEndedEvent?.Invoke(this);
        }
    }
}