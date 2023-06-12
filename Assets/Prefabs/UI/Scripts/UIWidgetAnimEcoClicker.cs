using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIWidgetAnimEcoClicker<T> : UIElementAnimEcoClicker where T : UIProperties {
        [SerializeField] protected T properties;
    }
}