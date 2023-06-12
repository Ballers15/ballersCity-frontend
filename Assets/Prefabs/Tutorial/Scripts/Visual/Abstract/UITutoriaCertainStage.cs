using System;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public abstract class UITutoriaCertainStage<T> : UITutorialStage where T : TutorialStageController
    {
        protected T stageController { get; private set; }

        public UIWidgetBtnNavigateUpgrade uiWidgetBtnNavigateUpgrade { get; protected set; }

        public const string IDLE_OBJECT_1_ID = "io_1";
        public const string IDLE_OBJECT_2_ID = "io_2";
        public const string IDLE_OBJECT_1_VISUAL_NAME = "Sweepers";
        public const string IDLE_OBJECT_2_VISUAL_NAME = "Tractor";

        protected virtual void Start() {
            uiWidgetBtnNavigateUpgrade = FindObjectOfType<UIWidgetBtnNavigateUpgrade>();
        }

        public override Type GetRequiredControllerType()
        {
            return typeof(T);
        }

        public override void SetController(TutorialStageController tutorialStageController)
        {
            this.stageController = (T) tutorialStageController;
        }
        
        public UITutorialIdleObjectHighlighter GetIdleObjectHighlighter(string idleObjectId, out IdleObject idleObject) {
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            idleObject = idleObjectInteractor.GetIdleObject(idleObjectId);

            UITutorialIdleObjectHighlighter highlighter =
                idleObject.GetComponent<UITutorialIdleObjectHighlighter>();
            
            if (!highlighter)
                highlighter = idleObject.gameObject.AddComponent<UITutorialIdleObjectHighlighter>();
            
            return highlighter;
        }
        
        public void SetActiveIdleObjectCanvasHighlighter(string idleObjectId, bool isActive) {
            IdleObject idleObject;
            UITutorialIdleObjectHighlighter highlighter = GetIdleObjectHighlighter(idleObjectId, out idleObject);

            if (isActive)
                highlighter.HighlightCanvas(idleObject.transform);
            else
                highlighter.ResetCanvas(idleObject.transform);
        }
        
        public void SetActiveIdleObjectVisualHighlighter(string idleObjectId, string visualName, bool isActive) {
            IdleObject idleObject;
            UITutorialIdleObjectHighlighter highlighter = GetIdleObjectHighlighter(idleObjectId, out idleObject);

            if (isActive)
                highlighter.HighlightVisual(idleObject.transform, visualName);
            else
                highlighter.ResetVisual(idleObject.transform, visualName);
        }
        
        public void SetActiveIdleObjectHighlighter(string idleObjectId, string visualName, bool isActive) {
            IdleObject idleObject;
            UITutorialIdleObjectHighlighter highlighter = GetIdleObjectHighlighter(idleObjectId, out idleObject);

            if (isActive)
                highlighter.Highlight(idleObject.transform, visualName);
            else
                highlighter.Reset(idleObject.transform, visualName);
        }
        
        public void SetActiveBtnHighlighter(bool isActive) {
            UITutorialStageUpgradeBuildingHighlighter btnHighlighter = uiWidgetBtnNavigateUpgrade.gameObject
                .GetComponent<UITutorialStageUpgradeBuildingHighlighter>();
            if (!btnHighlighter)
                btnHighlighter = uiWidgetBtnNavigateUpgrade.gameObject
                    .AddComponent<UITutorialStageUpgradeBuildingHighlighter>();
            if (isActive)
                btnHighlighter.Highlight(uiWidgetBtnNavigateUpgrade.transform, 500);
            else
                btnHighlighter.Reset(uiWidgetBtnNavigateUpgrade.transform);
        }
        
        protected void SetCameraRenderLayerToSpecial() {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            Camera camera = uiInteractor.uiController.gameObject.GetComponentInChildren<Camera>(true);
            camera.cullingMask = (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Default"));
        }
        
        protected void SetCameraRenderLayerToDefault() {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            Camera camera = uiInteractor.uiController.gameObject.GetComponentInChildren<Camera>(true);
            camera.cullingMask = (1 << LayerMask.NameToLayer("UI"));
        }
    }
}