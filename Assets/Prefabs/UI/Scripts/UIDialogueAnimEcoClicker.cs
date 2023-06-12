using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIDialogueAnimEcoClicker<T, P> : UIElementAnimEcoClicker where T : UIProperties where P : UIDialogueArgs{

        [Space, SerializeField] protected T properties;

        public delegate void DialogueResultsHandler(P e);
        public event DialogueResultsHandler OnDialogueResults;

        protected virtual void NotifyAboutResults(P e) {
            OnDialogueResults?.Invoke(e);
        }
        
        protected virtual void Reset() {
#if UNITY_EDITOR
            m_layer = Layer.Dialogue;
#endif
        }
    }
}