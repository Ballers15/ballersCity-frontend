using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIPanelAnimEcoClicker<T> : UIElementAnimEcoClicker where T : UIProperties {
        [SerializeField] protected T properties;
    }
}