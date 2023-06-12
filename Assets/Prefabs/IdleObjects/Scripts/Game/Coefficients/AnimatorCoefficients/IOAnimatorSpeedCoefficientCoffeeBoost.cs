using IdleClicker.Gameplay;
using VavilichevGD.Architecture;

namespace SinSity.Core {
    public class IOAnimatorSpeedCoefficientCoffeeBoost : CoefficientFloat {

        #region CONSTANTS

        private const float SPEED_MULTIPLIER = 0.25f;

        #endregion
        
        private CoffeeBoostInteractor interactor;
        private CoffeeBoostConfig config;
        
        public IOAnimatorSpeedCoefficientCoffeeBoost() {
            this.interactor = Game.GetInteractor<CoffeeBoostInteractor>();
            this.config = interactor.coffeeBoost.actualConfig;
            this.interactor.coffeeBoost.OnStartedEvent += this.OnCoffeeBoostStarted;
            this.interactor.coffeeBoost.OnEndedEvent += this.OnCoffeeBoostEnded;
            
            if (this.interactor.coffeeBoost.isActive)
                this.Activate();
        }

        public void Activate() {
            this.value = this.config.boostTimeScale * SPEED_MULTIPLIER;
            this.NotifyAboutCoefficientChanged();
        }

        public void Reset() {
            this.value = 1f;
            this.NotifyAboutCoefficientChanged();
        }

        ~IOAnimatorSpeedCoefficientCoffeeBoost() {
            this.interactor.coffeeBoost.OnStartedEvent -= this.OnCoffeeBoostStarted;
            this.interactor.coffeeBoost.OnEndedEvent -= this.OnCoffeeBoostEnded;
        }

        #region EVENTS

        private void OnCoffeeBoostEnded(CoffeeBoost coffeeboost) {
            this.Reset();
        }

        private void OnCoffeeBoostStarted(CoffeeBoost coffeeboost) {
            this.Activate();
        }

        #endregion
        
    }
}