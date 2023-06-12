using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public sealed class UITutorialStageUpgradeBuilding :
        UITutoriaCertainStage<TutorialStageControllerUpgradeBuilding>
    {
        [SerializeField] private UITutorialStageUpgradeBuildingPopup m_popupShowBlueprint;
        [SerializeField] private UITutorialStageUpgradeBuildingPopup m_popupUpgradeBuilding;
        [SerializeField] private UITutorialStageUpgradeBuildingPopup m_popupUpgrade;
        [SerializeField] private UITutorialStageUpgradeBuildingPopup m_popupHideBlueprint;
        [SerializeField] private int upgradesCount = 2;

        private int currentUpgradesCount;
        private IdleObject firstIdleObject;

        private void Awake()
        {
            this.m_popupShowBlueprint.OnClickEvent.AddListener(this.OnShowBluePrintClick);
            this.m_popupUpgradeBuilding.OnClickEvent.AddListener(this.OnOpenUpgradeClicked);
            this.m_popupUpgrade.OnClickEvent.AddListener(OnUpgradeBuildingClick);
            this.m_popupHideBlueprint.OnClickEvent.AddListener(this.OnHideBluePrintClick);
            this.m_popupShowBlueprint.SetVisible();
            this.m_popupUpgradeBuilding.SetInvisible();
            this.m_popupHideBlueprint.SetInvisible();
            this.currentUpgradesCount = 0;
        }


        protected override void Start()
        {
            base.Start();
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, false);
            SetActiveBtnHighlighter(true);
            SetCameraRenderLayerToSpecial();
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            firstIdleObject = idleObjectInteractor.GetIdleObject(IDLE_OBJECT_1_ID);

            TutorialAnalytics.LogTutorialEventInSec("tutor_third_hint", 3);
        }

        #region ClickEvents

        private void OnShowBluePrintClick()
        {
            uiWidgetBtnNavigateUpgrade.OnClick();
            this.m_popupShowBlueprint.SetInvisible();
            this.m_popupUpgradeBuilding.SetVisible();
            SetActiveBtnHighlighter(false);
            SetActiveIdleObjectCanvasHighlighter(IDLE_OBJECT_1_ID, true);
            SetActiveIdleObjectVisualHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, false);
            TutorialAnalytics.LogTutorialEventInSec("tutor_upgrade_window_open", 3);
            TutorialAnalytics.LogTutorialEventInSec("tutor_fourth_hint", 3);
        }

        private void OnOpenUpgradeClicked() {
            m_popupUpgradeBuilding.SetInvisible();
            m_popupUpgrade.SetVisible();
            var idleObjectUI = firstIdleObject.GetComponentInChildren<IdleObjectUI>();
            idleObjectUI.OnUpgradeClicked();
            Bank.uiBank.AddSoftCurrency(this, new BigNumber(200));
            TutorialAnalytics.LogTutorialEventInSec("tutor_upgrade_window_click", 3);
            TutorialAnalytics.LogTutorialEventInSec("tutor_fifth_hint", 3);
        }

        private void OnUpgradeBuildingClick()
        {
            currentUpgradesCount++;

            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            var firstIdleObject = idleObjectInteractor.GetIdleObject("io_1");
            firstIdleObject.NextLevel();

            if (currentUpgradesCount < upgradesCount)
                return;

            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var uiPopupUpgrade = uiInteractor.GetUIElement<UIPopupUpgrade>();
            uiPopupUpgrade.Hide();
            this.m_popupUpgrade.SetInvisible();
            this.m_popupHideBlueprint.SetVisible();
            SetActiveBtnHighlighter(true);
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, false);

            TutorialAnalytics.LogTutorialEventInSec("tutor_sixth_object_upgrade", 3);
            TutorialAnalytics.LogTutorialEventInSec("tutor_sixth_hint", 3);
        }

        private void OnHideBluePrintClick()
        {
            uiWidgetBtnNavigateUpgrade.OnClick();
            this.m_popupHideBlueprint.SetInvisible();
            this.OnStageFinishedEvent?.Invoke();
            SetActiveBtnHighlighter(false);

            TutorialAnalytics.LogTutorialEventInSec("tutor_upgrade_window_exit", 3);
        }

        #endregion

        public override void OnStageFinished()
        {
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, false);
            SetCameraRenderLayerToDefault();
        }
    }
}