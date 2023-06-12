using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;

namespace SinSity.Core
{
    public sealed class UIHintInspectorResearchInfo : UIHintCertainInspector<HintInspectorResearchInfo>
    {
        [SerializeField]
        private UIHintResearchInfo uiHintResearchInfo;

        private void Awake()
        {
            this.uiHintResearchInfo.SetInvisible();
            this.uiHintResearchInfo.OnClickedEvent.AddListener(() =>
            {
                this.uiHintResearchInfo.SetInvisible();
                TutorialAnalytics.LogEventInMin("hint_research_window_hint_close");
            });
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            if (this.hintInspector.IsViewed())
            {
                return;
            }

            UIWidgetBtnNavigateResearch.OnScreenOpenedEvent += this.OnResearchScreenOpened;
        }

        private void OnResearchScreenOpened()
        {
            UIWidgetBtnNavigateResearch.OnScreenOpenedEvent -= this.OnResearchScreenOpened;
            this.hintInspector.NotifyAboutResearchInfoViewed();
            this.uiHintResearchInfo.SetVisible();
            TutorialAnalytics.LogEventInMin("hint_research_window_first_time");
        }
    }
}