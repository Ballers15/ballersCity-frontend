using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIScreenAnimEcoClicker<T> : UIElementAnimEcoClicker where T : UIProperties {
        [SerializeField] protected T properties;
    }
}