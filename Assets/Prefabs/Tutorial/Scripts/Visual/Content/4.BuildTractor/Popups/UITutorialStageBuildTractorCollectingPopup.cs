using Orego.Util;
using SinSity.Domain;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;

namespace SinSity.Core
{
    public sealed class UITutorialStageBuildTractorCollectingPopup : MonoBehaviour
    {
        [SerializeField]
        private Button m_buttonCollectCleanEnergy;

        [SerializeField]
        private Button m_buttonUpgradeBuiding;

        [SerializeField]
        private Button m_buttonShowBlueprint;

        private UIWidgetBtnNavigateUpgrade uiWidgetBtnNavigateUpgrade;

        private IdleObject firstIdleObject;
        private UITutorialStageBuildTractor uiTutorialStage;

        private void Start() {
            uiTutorialStage = gameObject.GetComponentInParent<UITutorialStageBuildTractor>();
            this.m_buttonCollectCleanEnergy.GetComponent<PointerEnterHandler>().OnPointerEnterEvent
                .AddListener(data => this.OnCollectCleanEnergyClick());
            this.uiWidgetBtnNavigateUpgrade = FindObjectOfType<UIWidgetBtnNavigateUpgrade>();
            this.firstIdleObject = Game
                .GetInteractor<IdleObjectsInteractor>()
                .GetIdleObject("io_1");
            this.RefreshButtons();
        }

        private void OnEnable()
        {
            this.m_buttonCollectCleanEnergy.onClick.AddListener(this.OnCollectCleanEnergyClick);
            this.m_buttonUpgradeBuiding.onClick.AddListener(this.OnUpgradeBuilding);
            this.m_buttonShowBlueprint.onClick.AddListener(this.OnShowBlueprintClick);
        }

        private void OnDisable()
        {
            this.m_buttonShowBlueprint.onClick.RemoveAllListeners();
            this.m_buttonCollectCleanEnergy.onClick.RemoveAllListeners();
            this.m_buttonUpgradeBuiding.onClick.RemoveAllListeners();
        }

        private void RefreshButtons()
        {
            if (this.uiWidgetBtnNavigateUpgrade.IsPopupAlreadyOpened())
            {
                this.m_buttonCollectCleanEnergy.SetInvisible();
                this.m_buttonUpgradeBuiding.SetVisible();
            }
            else
            {
                this.m_buttonCollectCleanEnergy.SetVisible();
                this.m_buttonUpgradeBuiding.SetInvisible();
            }
        }

        #region ClickEvents

        private void OnCollectCleanEnergyClick()
        {
            this.firstIdleObject.CollectCurrency();
        }

        private void OnUpgradeBuilding()
        {
            this.firstIdleObject.NextLevel();
        }

        private void OnShowBlueprintClick() {
            
            this.uiWidgetBtnNavigateUpgrade.OnClick();
            this.RefreshButtons();

            bool isActive = BluePrint.bluePrintModeEnabled;
            
            uiTutorialStage.SetActiveIdleObjectCanvasHighlighter(
                UITutoriaCertainStage<TutorialStageControllerBuildTractor>.IDLE_OBJECT_1_ID, true);
            uiTutorialStage.SetActiveIdleObjectCanvasHighlighter(
                UITutoriaCertainStage<TutorialStageControllerBuildTractor>.IDLE_OBJECT_2_ID, true);
            
            uiTutorialStage.SetActiveIdleObjectVisualHighlighter(
                UITutoriaCertainStage<TutorialStageControllerBuildTractor>.IDLE_OBJECT_1_ID,
                UITutoriaCertainStage<TutorialStageControllerBuildTractor>.IDLE_OBJECT_1_VISUAL_NAME, !isActive);
            uiTutorialStage.SetActiveIdleObjectVisualHighlighter(
                UITutoriaCertainStage<TutorialStageControllerBuildTractor>.IDLE_OBJECT_2_ID,
                UITutoriaCertainStage<TutorialStageControllerBuildTractor>.IDLE_OBJECT_2_VISUAL_NAME, !isActive);
        }

        #endregion
    }
}