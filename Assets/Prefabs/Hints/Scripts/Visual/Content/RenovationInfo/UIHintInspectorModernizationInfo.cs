using Orego.Util;
using SinSity.Domain;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Core
{
    public sealed class UIHintInspectorModernizationInfo : UIHintCertainInspector<HintInspectorRenovationInfo>
    {
        public static bool isShown { get; private set; }
        
        
        [SerializeField]
        private UIHintModernizationInfo uiHintModernizationInfo;

        private ModernizationAnalytics analytics;

        private void Awake()
        {
            this.uiHintModernizationInfo.SetInvisible();
            this.uiHintModernizationInfo.OnClickedEvent.AddListener(() =>
            {
                this.uiHintModernizationInfo.SetInvisible();
                isShown = false;
            });
        }

        public override void OnInitialized() {
            base.OnInitialized();
            if (this.hintInspector.IsViewed())
            {
                return;
            }

            var modernizationInteractor = Game.GetInteractor<ModernizationInteractor>();
            modernizationInteractor.analytics.hintEnabled = true;

            UIWidgetBtnNavigateQuests.OnPopupOpenedEvent += this.OnQuestPopupOpened;
        }

        private void OnQuestPopupOpened()
        {
            if (this.hintInspector.IsReadyForView() && !this.hintInspector.IsViewed())
            {
                UIWidgetBtnNavigateQuests.OnPopupOpenedEvent -= this.OnQuestPopupOpened;
                this.hintInspector.NotifyAboutRenovationInfoViewed();
                this.uiHintModernizationInfo.SetVisible();
                isShown = true;
            }
        }
    }
}