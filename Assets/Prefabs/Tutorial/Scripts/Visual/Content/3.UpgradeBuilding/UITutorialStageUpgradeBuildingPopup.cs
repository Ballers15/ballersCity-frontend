using System;
using Orego.Util;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.Core
{
    public sealed class UITutorialStageUpgradeBuildingPopup : MonoBehaviour
    {
        #region Event

        public AutoEvent OnClickEvent { get; } = new AutoEvent();

        #endregion

        [SerializeField]
        private Button m_buttonClick;

        private void OnEnable()
        {
            this.m_buttonClick.onClick.AddListener(this.OnClickEvent.Invoke);
        }

        private void OnDisable()
        {
            this.m_buttonClick.onClick.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            this.OnClickEvent.Dispose();
        }
    }
}