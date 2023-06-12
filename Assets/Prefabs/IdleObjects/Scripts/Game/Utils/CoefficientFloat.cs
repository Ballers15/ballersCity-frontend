namespace SinSity.Core {
    public class CoefficientFloat {

        #region DELEGATES

        public delegate void CoefficientFloatHandler(CoefficientFloat coefficient);
        public event CoefficientFloatHandler OnCoefficientChangedEvent;

        #endregion
        
        
        public float value { get; protected set; }

        public CoefficientFloat() {
            this.value = 1;
        }

        public CoefficientFloat(float value) {
            this.value = value;
        }

        protected void NotifyAboutCoefficientChanged() {
            this.OnCoefficientChangedEvent?.Invoke(this);
        }
    }
}