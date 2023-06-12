using UnityEngine.Events;

namespace SinSity.UI {
    public interface IUIWidgetPanelResearch {
        void AddListener(UnityAction callback);
        void RemoveListener(UnityAction callback);
        void RemoveAllListeners();
    }
}