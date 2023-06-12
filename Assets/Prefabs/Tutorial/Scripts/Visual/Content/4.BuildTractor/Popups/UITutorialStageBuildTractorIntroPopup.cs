using Orego.Util;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.Core
{
    public sealed class UITutorialStageBuildTractorIntroPopup : MonoBehaviour
    {
        #region Event

        public AutoEvent OnClickEvent { get; } = new AutoEvent();

        #endregion

        [SerializeField]
        private Button m_buttonAreaClick;

        private void OnEnable()
        {
            this.m_buttonAreaClick.onClick.AddListener(this.OnClickEvent.Invoke);
        }

        private void OnDisable()
        {
            this.m_buttonAreaClick.onClick.RemoveAllListeners();
        }
    }
}