using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIScreenAnim<T> : UIElementAnim where T : UIProperties {
        [SerializeField] protected T properties;
    }
}