using System.Collections.Generic;

namespace SinSity.Core {
    public class IOAnimatorSpeedAllCoefficients {

        #region DELEGATES

        public delegate void IOAnimatorSpeedAllCoefficientsHandler(IOAnimatorSpeedAllCoefficients allCoefficients);
        public event IOAnimatorSpeedAllCoefficientsHandler OnStateChangedEvent;

        #endregion
        
        
        private List<CoefficientFloat> coefficients;
        
        public IOAnimatorSpeedAllCoefficients() {
            this.coefficients = new List<CoefficientFloat>();
            
            this.coefficients.Add(new IOAnimatorSpeedCoefficientCoffeeBoost());
            this.coefficients.Add(new IOAnimatorSpeedCoefficientEcoBoost());

            foreach (var coefficient in this.coefficients)
                coefficient.OnCoefficientChangedEvent += this.OnCoefficientChanged;
        }

        public float GetTotalValue() {
            float totalValue = 1f;
            foreach (var coefficient in this.coefficients)
                totalValue = totalValue * coefficient.value;
            return totalValue;
        }

        ~IOAnimatorSpeedAllCoefficients() {
            foreach (var coefficient in this.coefficients)
                coefficient.OnCoefficientChangedEvent -= this.OnCoefficientChanged;
        }

        #region EVENTS

        private void OnCoefficientChanged(CoefficientFloat coefficient) {
            this.OnStateChangedEvent?.Invoke(this);
        }

        #endregion
    }
}