using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public sealed class UITutorialStageBuildObject :
        UITutorialAreaFocusedStage<TutorialStageControllerBuildObject>
    {
        [SerializeField] private Transform m_panelIntro;
        [SerializeField] private Transform m_firstStage;
        [SerializeField] private Transform m_secondStage;
        [SerializeField] private Button m_secondStageButton;

        private IdleObject firstIdleObject;
        private UIPopupBuild popupBuild;

        protected override void Start()
        {
            base.Start();
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            this.firstIdleObject = idleObjectInteractor.GetIdleObject(IDLE_OBJECT_1_ID);
            this.SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, true);
            this.SetCameraRenderLayerToSpecial();
            TutorialAnalytics.LogTutorialEventInSec("tutor_start_first_hint", 1);
        }

        protected override void OnFocusButtonClick()
        {
            var idleObjectUI = firstIdleObject.GetComponentInChildren<IdleObjectUI>();
            idleObjectUI.OnBuildBtnClick();
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            popupBuild = uiInteractor.GetUIElement<UIPopupBuild>();
            popupBuild.OnDialogueResults += this.OnBuildIdleObjectDialogueResults;
            this.m_panelIntro.SetInvisible();
            TutorialAnalytics.LogTutorialEventInSec("tutor_first_object_click", 1);
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, false);
            m_firstStage.SetInvisible();
            m_secondStage.SetVisible();
            m_secondStageButton.onClick.AddListener(BuildIdleObject);
        }

        private void BuildIdleObject() {
            m_secondStageButton.onClick.RemoveListener(BuildIdleObject);
            popupBuild.OnBuildBtnClick();
        }

        private void OnBuildIdleObjectDialogueResults(UIPopupArgs e)
        {
            var uiPopupBuild = Game.GetInteractor<UIInteractor>().GetUIElement<UIPopupBuild>();
            uiPopupBuild.OnDialogueResults -= this.OnBuildIdleObjectDialogueResults;
            
            var result = e.result;
            if (result == UIPopupResult.Apply)
            {
                TutorialAnalytics.LogTutorialEventInSec("tutor_first_object_buildwindow_build", 1);
            }
            else if (result == UIPopupResult.Close)
            {
                TutorialAnalytics.LogTutorialEventInSec("tutor_first_object_buildwindow_close", 1);
                SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, true);
            }

            SetCameraRenderLayerToDefault();
        }
    }
}