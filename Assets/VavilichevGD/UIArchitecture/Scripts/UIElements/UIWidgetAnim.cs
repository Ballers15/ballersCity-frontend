using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIWidgetAnim<T> : UIElementAnim where T : UIProperties {
        [SerializeField] protected T properties;
    }
}