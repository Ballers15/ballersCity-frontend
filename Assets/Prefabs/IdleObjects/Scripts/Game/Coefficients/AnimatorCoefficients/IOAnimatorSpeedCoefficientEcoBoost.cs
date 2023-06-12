using SinSity.Domain;
using VavilichevGD.Architecture;


namespace SinSity.Core {
    public class IOAnimatorSpeedCoefficientEcoBoost : CoefficientFloat {
        
        #region CONSTANTS

        private const float SPEED_MULTIPLIER = 2f;

        #endregion
        
        private EcoBoostInteractor interactor;
        
        public IOAnimatorSpeedCoefficientEcoBoost() {
            this.interactor = Game.GetInteractor<EcoBoostInteractor>();
            this.interactor.OnEcoBoostEnabledEvent += OnEcoBoostEnabled;
            this.interactor.OnEcoBoostDisabledEvent += OnEcoBoostDisabled;
            this.interactor.OnEcoBoostLaunchedEvent += OnEcoBoostEnabled;

            if (this.interactor.isEcoBoostWorking)
                this.Activate();
        }

        public void Activate() {
            this.value = SPEED_MULTIPLIER;
            this.NotifyAboutCoefficientChanged();
        }

        public void Reset() {
            this.value = 1f;
            this.NotifyAboutCoefficientChanged();
        }

        ~IOAnimatorSpeedCoefficientEcoBoost() {
            this.interactor.OnEcoBoostEnabledEvent -= OnEcoBoostEnabled;
            this.interactor.OnEcoBoostDisabledEvent -= OnEcoBoostDisabled;
            this.interactor.OnEcoBoostLaunchedEvent -= OnEcoBoostEnabled;
        }

        #region EVENTS

        private void OnEcoBoostDisabled() {
            this.Reset();    
        }

        private void OnEcoBoostEnabled() {
            this.Activate();
        }
        
        #endregion
    }
}