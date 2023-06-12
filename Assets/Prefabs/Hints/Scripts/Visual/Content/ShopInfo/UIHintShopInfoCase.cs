using Orego.Util;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.Core
{
    public sealed class UIHintShopInfoCase : AutoMonoBehaviour
    {
        #region Event

        public AutoEvent OnClickedEvent { get; }

        #endregion

        [SerializeField]
        private Button m_buttonOk;
        
        public UIHintShopInfoCase()
        {
            this.OnClickedEvent = this.AutoInstantiate<AutoEvent>();
        }

        private void OnEnable()
        {
            this.m_buttonOk.AddListener(this.OnClickedEvent.Invoke);
        }

        private void OnDisable()
        {
            this.m_buttonOk.RemoveListeners();
        }
    }
}