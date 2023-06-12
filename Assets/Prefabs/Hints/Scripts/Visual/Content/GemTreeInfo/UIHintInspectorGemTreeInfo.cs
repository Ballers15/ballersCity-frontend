using System.Collections;
using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;

namespace SinSity.Core
{
    public sealed class UIHintInspectorGemTreeInfo : 
        UIHintCertainInspector<HintInspectorGemTreeInfo>
    {
        [SerializeField]
        private UIHintGemTreeInfo uiHintGemTreeInfo;

        private void Awake()
        {
            this.uiHintGemTreeInfo.SetInvisible();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            if (this.hintInspector.IsViewed())
            {
                return;
            }

            UIWidgetBtnNavigateGemTree.OnPopupOpenedEvent += this.OnGemTreePopupOpened;
        }

        private void OnGemTreePopupOpened()
        {
            UIWidgetBtnNavigateGemTree.OnPopupOpenedEvent -= this.OnGemTreePopupOpened;
            this.hintInspector.NotifyAboutGemTreeInfoViewed();
            this.uiHintGemTreeInfo.SetVisible();
            this.StartCoroutine(this.InitializeRoutine());
            TutorialAnalytics.LogEventInMin("hint_gem_tree_window_first_time");
        }

        private IEnumerator InitializeRoutine()
        {
            yield return new WaitForSeconds(1f);
            this.uiHintGemTreeInfo.OnClickedEvent.AddListener(() =>
            {
                this.uiHintGemTreeInfo.SetInvisible();
                TutorialAnalytics.LogEventInMin("hint_gem_tree_window_first_time_close");
            });
        }
    }
}