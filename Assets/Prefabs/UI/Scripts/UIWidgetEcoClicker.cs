using SinSity.UI;
using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIWidgetEcoClicker<T> : UIElementEcoClicker where T : UIProperties{
        [SerializeField] protected T properties;
    }
}