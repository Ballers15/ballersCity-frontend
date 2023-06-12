using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public sealed class UITutorialStageBuildTractor : UITutoriaCertainStage<TutorialStageControllerBuildTractor>
    {
        [SerializeField]
        private UITutorialStageBuildTractorIntroPopup m_popupIntro;

        [SerializeField]
        private UITutorialStageBuildTractorCollectingPopup m_popupCollecting;

        [SerializeField]
        private UITutorialStageBuildTractorToBuildPopup m_popupBuildTractor;

        private IdleObject tractorIdleObject;
        
        private IdleObjectsInteractor idleObjectsInteractor;

        private void Awake()
        {
            this.idleObjectsInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            this.tractorIdleObject = idleObjectsInteractor.GetIdleObject(IDLE_OBJECT_2_ID);
            this.m_popupIntro.SetVisible();
            this.m_popupCollecting.SetInvisible();
            this.m_popupBuildTractor.SetInvisible();
            this.m_popupIntro.OnClickEvent.AddListener(this.OnIntroPopupClick);
            this.m_popupBuildTractor.OnBuildClickEvent.AddListener(this.OnBuildTractorClick);
        }

        private void OnEnable()
        {
            Bank.uiBank.OnStateChangedEvent += OnBankStateChanged;
        }

       

        protected override void Start()
        {
            base.Start();

            uiWidgetBtnNavigateUpgrade = FindObjectOfType<UIWidgetBtnNavigateUpgrade>();
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, false);
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_2_ID, IDLE_OBJECT_2_VISUAL_NAME, false);
            SetActiveBtnHighlighter(false);
            SetCameraRenderLayerToSpecial();

            TutorialAnalytics.LogTutorialEventInSec("tutor_sixth_hint", 4);
        }

        private void OnDisable()
        {
            Bank.uiBank.OnStateChangedEvent -= OnBankStateChanged;
        }

        private void OnIntroPopupClick()
        {
            this.m_popupIntro.SetInvisible();
            this.m_popupCollecting.SetVisible();

            SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, true);
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_2_ID, IDLE_OBJECT_2_VISUAL_NAME, true);
            SetActiveBtnHighlighter(true);

            TutorialAnalytics.LogTutorialEventInSec("tutor_sixth_hint_close", 4);
            TutorialAnalytics.LogTutorialEventInSec("tutor_seventh_hint", 4);
        }
        
        private void OnBankStateChanged(object sender) {
            var softCurrencyCount = Bank.softCurrencyCount;
            var priceToBuildForTractor = this.tractorIdleObject.info.priceToBuild;
            if (softCurrencyCount < priceToBuildForTractor)
            {
                return;
            }

            this.m_popupCollecting.SetInvisible();
            this.m_popupBuildTractor.SetVisible();

            SetActiveBtnHighlighter(false);
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, false);
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_2_ID, IDLE_OBJECT_2_VISUAL_NAME, true);
            
        }

        private void OnBuildTractorClick()
        {
            var uiIdleObject = this.tractorIdleObject.GetComponentInChildren<IdleObjectUI>();
            uiIdleObject.OnBuildBtnClick();
            this.m_popupBuildTractor.panelInfo.SetInvisible();
            SetActiveBtnHighlighter(false);

            TutorialAnalytics.LogTutorialEventInSec("tutor_second_object_click", 4);
            var uiPopupBuild = Game.GetInteractor<UIInteractor>().GetUIElement<UIPopupBuild>();
            uiPopupBuild.OnDialogueResults += this.OnBuildSecondObjectDialogueResults;
        }

        private void OnBuildSecondObjectDialogueResults(UIPopupArgs e)
        {
            var uiPopupBuild = Game.GetInteractor<UIInteractor>().GetUIElement<UIPopupBuild>();
            uiPopupBuild.OnDialogueResults -= this.OnBuildSecondObjectDialogueResults;
            
            if (e.result == UIPopupResult.Apply)
            {
                TutorialAnalytics.LogTutorialEventInSec("tutor_second_object_buildwindow_build", 4);
            }
            else if (e.result == UIPopupResult.Close)
            {
                TutorialAnalytics.LogTutorialEventInSec("tutor_second_object_buildwindow_close", 4);
            }
        }

        public override void OnStageFinished()
        {
            base.OnStageFinished();
            this.m_popupBuildTractor.SetInvisible();
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, false);
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_2_ID, IDLE_OBJECT_2_VISUAL_NAME, false);
            SetActiveBtnHighlighter(false);
        }
    }
}