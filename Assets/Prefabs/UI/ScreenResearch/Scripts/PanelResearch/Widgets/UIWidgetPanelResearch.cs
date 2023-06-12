using UnityEngine.Events;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public class UIWidgetPanelResearch<T> : UIWidget<T>, IUIWidgetPanelResearch
        where T : UIWidgetPanelResearchProperties
    {
        public void AddListener(UnityAction callback)
        {
            this.properties.AddListener(callback);
        }

        public void RemoveListener(UnityAction callback)
        {
            this.properties.RemoveListener(callback);
        }

        public void RemoveAllListeners()
        {
            this.properties.RemoveAllListeners();
        }
    }
}