using SinSity.Domain;

namespace SinSity.Core
{
    public abstract class UIHintCertainInspector<T> : UIHintInpector where T : HintInspector
    {
        protected T hintInspector { get; private set; }

        public override void OnInitialized()
        {
            base.OnInitialized();
            this.hintInspector = this.hintSystemInteractor.GetInspector<T>();
            this.hintInspector.OnStateChangedEvent += this.OnInspectorStateChanged;
            this.hintInspector.OnTriggeredEvent += this.OnInspectorTriggered;
        }
        
        private void OnDestroy()
        {
            this.hintInspector.OnStateChangedEvent -= this.OnInspectorStateChanged;
            this.hintInspector.OnTriggeredEvent -= this.OnInspectorTriggered;
        }

        protected virtual void OnInspectorStateChanged(HintInspector inspector)
        {
        }

        protected virtual void OnInspectorTriggered(HintInspector inspector)
        {
        }
    }
}