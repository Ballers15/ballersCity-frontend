using System;
using UnityEngine.Events;

namespace SinSity.UI
{
    [Serializable]
    public abstract class UIWidgetPanelResearchProperties : UIProperties
    {
        public virtual void AddListener(UnityAction callback)
        {
        }

        public virtual void RemoveListener(UnityAction callback)
        {
        }

        public virtual void RemoveAllListeners()
        {
        }
    }
}