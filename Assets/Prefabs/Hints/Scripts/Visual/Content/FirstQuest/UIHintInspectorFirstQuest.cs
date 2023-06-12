using System.Collections;
using Orego.Util;
using SinSity.Core.Content.FirstQuest;
using SinSity.Domain;
using SinSity.Services;
using UnityEngine;

namespace SinSity.Core
{
    public sealed class UIHintInspectorFirstQuest : UIHintCertainInspector<HintInspectorFirstQuest>
    {
        [SerializeField]
        private UIHintFirstQuest uiHintFirstQuest;

        private void Awake()
        {
            this.uiHintFirstQuest.SetInvisible();
        }

        protected override void OnInspectorStateChanged(HintInspector inspector)
        {
            if (!this.hintInspector.IsViewed())
            {
                this.hintInspector.NotifyAboutFirstQuestViewed();
                this.uiHintFirstQuest.SetVisible();
                StartCoroutine(InitializeRoutine());

                TutorialAnalytics.LogEventInMin("hint_mission_window_finished_mission_close");
            }
        }

        private IEnumerator InitializeRoutine() {
            yield return new WaitForSeconds(1f);
            this.uiHintFirstQuest.OnClickedEvent.AddListener(() =>
            {
                this.uiHintFirstQuest.SetInvisible();

                TutorialAnalytics.LogEventInMin("hint_mission_window_finished_mission_close");
            });
        }
    }
}