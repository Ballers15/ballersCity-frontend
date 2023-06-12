using UnityEngine;

namespace VavilichevGD.UI
{
    public abstract class UIPanel<T> : UIElement where T : UIProperties
    {
        [SerializeField]
        protected T properties;
    }
}