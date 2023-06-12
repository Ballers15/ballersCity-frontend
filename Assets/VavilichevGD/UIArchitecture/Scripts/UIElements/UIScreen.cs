using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIScreen<T> : UIElement where T : UIProperties {
        [SerializeField] protected T properties;
    }
}