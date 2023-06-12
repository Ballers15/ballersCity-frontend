using Orego.Util;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.Core
{
    public sealed class UITutorialStageBuildTractorToBuildPopup : MonoBehaviour
    {
        #region Event

        public AutoEvent OnBuildClickEvent { get; } = new AutoEvent();

        #endregion

        [SerializeField]
        private Transform m_panelInfo;

        [SerializeField]
        private Button m_buttonBuildTractor;

        public Transform panelInfo
        {
            get { return this.m_panelInfo; }
        }

        private void OnEnable()
        {
            this.m_buttonBuildTractor.onClick.AddListener(this.OnBuildClickEvent.Invoke);
        }

        private void OnDisable()
        {
            this.m_buttonBuildTractor.onClick.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            this.OnBuildClickEvent.Dispose();
        }
    }
}