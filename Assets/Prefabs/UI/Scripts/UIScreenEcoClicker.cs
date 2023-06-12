using SinSity.UI;
using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIScreenEcoClicker<T> : UIElementEcoClicker where T : UIProperties {
        [SerializeField] protected T properties;
    }
}