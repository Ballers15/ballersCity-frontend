using System.Collections.Generic;

namespace SinSity.Core {
    public class IOPeriodAllCoefficients {

        private List<CoefficientFloat> coefficients;
        
        public IOPeriodAllCoefficients() {
            this.coefficients = new List<CoefficientFloat>();
            
            this.coefficients.Add(new IOPeriodCoefficientCoffeeBoost());
        }

        public float GetTotalValue() {
            float totalValue = 1f;
            foreach (var coefficient in this.coefficients)
                totalValue = totalValue * coefficient.value;
            return totalValue;
        }
        
    }
}