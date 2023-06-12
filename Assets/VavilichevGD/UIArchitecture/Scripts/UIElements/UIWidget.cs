using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIWidget<T> : 
    UIElement where T : UIProperties {
        [SerializeField] protected T properties;
    }
}