using SinSity.UI;
using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIDialogueEcoClicker<T, P> : UIElementEcoClicker where T : UIProperties where P : UIDialogueArgs{

        [Space, SerializeField] protected T properties;

        public delegate void DialogueResultsHandler(P e);
        public event DialogueResultsHandler OnDialogueResults;

        protected virtual void NotifyAboutResults(P e) {
            OnDialogueResults?.Invoke(e);
        }
    }
}