using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;

namespace SinSity.Core
{
    public sealed class UIHintInpectorEcoBoostInfo : UIHintCertainInspector<HintInspectorEcoBoostInfo>
    {
        [SerializeField]
        private UIHintEcoBoostInfo uiHintEcoBoostInfo;

        private void Awake()
        {
            this.uiHintEcoBoostInfo.SetInvisible();
            this.uiHintEcoBoostInfo.OnClickedEvent.AddListener(() =>
            {
                this.uiHintEcoBoostInfo.SetInvisible();

                TutorialAnalytics.LogEventInMin("hint_ecobust_window_hint_close");
            });
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            if (this.hintInspector.IsViewed())
            {
                return;
            }

            UIWidgetBtnNavigateEcoBoost.OnPopupOpenedEvent += this.OnQuestsPopupOpened;
        }

        private void OnQuestsPopupOpened()
        {
            UIWidgetBtnNavigateEcoBoost.OnPopupOpenedEvent -= this.OnQuestsPopupOpened;
            this.hintInspector.NotifyAboutEcoBoostInfoViewed();
            this.uiHintEcoBoostInfo.SetVisible();

            TutorialAnalytics.LogEventInMin("hint_ecobust_window_first_time");
        }
    }
}