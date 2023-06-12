using SinSity.UI;
using UnityEngine;

namespace VavilichevGD.UI
{
    public abstract class UIPanelEcoClicker<T> : UIElementEcoClicker where T : UIProperties
    {
        [SerializeField]
        protected T properties;
    }
}