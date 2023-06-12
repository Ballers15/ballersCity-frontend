using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIPanelAnim<T> : UIElementAnim where T : UIProperties {
        [SerializeField] protected T properties;
    }
}