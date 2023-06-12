using SinSity.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.Core
{
    public abstract class UITutorialAreaFocusedStage<T> : UITutoriaCertainStage<T> where T : TutorialStageController
    {
        [SerializeField]
        private Button m_buttonFocus;

        protected Button buttonFocus
        {
            get { return this.m_buttonFocus; }
        }

        protected abstract void OnFocusButtonClick();

        private void OnEnable()
        {
            this.m_buttonFocus.onClick.AddListener(this.OnFocusButtonClick);
        }

        private void OnDisable()
        {
            this.m_buttonFocus.onClick.RemoveAllListeners();
        }
    }
}