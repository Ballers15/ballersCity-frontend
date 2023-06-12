using IdleClicker.Gameplay;
using VavilichevGD.Architecture;

namespace SinSity.Core {
    public class IOPeriodCoefficientCoffeeBoost : CoefficientFloat {

        private CoffeeBoostInteractor interactor;
        private CoffeeBoostConfig config;
        
        public IOPeriodCoefficientCoffeeBoost() {
            this.interactor = Game.GetInteractor<CoffeeBoostInteractor>();
            this.config = interactor.coffeeBoost.actualConfig;
            this.interactor.coffeeBoost.OnStartedEvent += this.OnCoffeeBoostStarted;
            this.interactor.coffeeBoost.OnEndedEvent += this.OnCoffeeBoostEnded;
            
            if (this.interactor.coffeeBoost.isActive)
                this.Activate();
        }

        public void Activate() {
            this.value = this.config.boostTimeScale;
            this.NotifyAboutCoefficientChanged();
        }

        public void Reset() {
            this.value = 1f;
            this.NotifyAboutCoefficientChanged();
        }

        
        ~IOPeriodCoefficientCoffeeBoost() {
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