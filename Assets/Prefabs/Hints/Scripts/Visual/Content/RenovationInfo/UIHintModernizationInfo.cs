using Orego.Util;
using SinSity.UI;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public sealed class UIHintModernizationInfo : AutoMonoBehaviour
    {
        #region Event

        public AutoEvent OnClickedEvent { get; }

        #endregion

        [SerializeField]
        private Button m_buttonOk;

        private UIWidgetModernization uiWidgetModernization;
        
        public UIHintModernizationInfo()
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
            SetActiveBtnHighlighter(uiWidgetModernization, false);
            
            if (uiWidgetModernization != null)
                this.m_buttonOk.onClick.RemoveListener(this.uiWidgetModernization.OnStartModernizationClick);
        }
        
        private void Start() {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            UIScreenQuests uiScreenQuests = uiInteractor.GetUIElement<UIScreenQuests>();
            uiWidgetModernization = uiScreenQuests.transform.GetComponentInChildren<UIWidgetModernization>(true);
            SetActiveBtnHighlighter(uiWidgetModernization, true);
            
            this.m_buttonOk.onClick.AddListener(this.uiWidgetModernization.OnStartModernizationClick);
        }

        private void SetActiveBtnHighlighter(UIWidgetModernization uiWidgetRenovationQuest, bool isActive) {
            Button btnRenovation = uiWidgetRenovationQuest.GetModernizationBtn();
            UITutorialStageUpgradeBuildingHighlighter btnHighlighter =
                btnRenovation.GetComponent<UITutorialStageUpgradeBuildingHighlighter>();
            if (!btnHighlighter)
                btnHighlighter = uiWidgetRenovationQuest.gameObject
                    .AddComponent<UITutorialStageUpgradeBuildingHighlighter>();
            if (isActive)
                btnHighlighter.Highlight(btnRenovation.transform, 750);
            else
                btnHighlighter.Reset(btnRenovation.transform);
        }
    }
}