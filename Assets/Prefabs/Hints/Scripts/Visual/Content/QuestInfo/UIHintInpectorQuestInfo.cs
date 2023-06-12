using System.Collections;
using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;

namespace SinSity.Core
{
    public sealed class UIHintInpectorQuestInfo : UIHintCertainInspector<HintInspectorQuestInfo>
    {
        [SerializeField]
        private UIHintQuestInfo uiHintQuestInfo;

        private void Awake()
        {
            this.uiHintQuestInfo.SetInvisible();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            if (this.hintInspector.IsViewed())
            {
                return;
            }

            UIWidgetBtnNavigateQuests.OnPopupOpenedEvent += this.OnQuestsPopupOpened;
        }

        private void OnQuestsPopupOpened()
        {
            UIWidgetBtnNavigateQuests.OnPopupOpenedEvent -= this.OnQuestsPopupOpened;
            this.hintInspector.NotifyAboutQuestInfoViewed();
            this.uiHintQuestInfo.SetVisible();
            StartCoroutine(InitializeRoutine());

            TutorialAnalytics.LogEventInMin("hint_mission_window_first_time");
        }
        
        private IEnumerator InitializeRoutine() {
            yield return new WaitForSeconds(1f);
            this.uiHintQuestInfo.OnClickedEvent.AddListener(() =>
            {
                this.uiHintQuestInfo.SetInvisible();
                TutorialAnalytics.LogEventInMin("hint_mission_window_first_time_close");
            });
        }
    }
}