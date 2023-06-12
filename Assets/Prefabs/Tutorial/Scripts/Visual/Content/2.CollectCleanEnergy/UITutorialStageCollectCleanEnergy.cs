using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Core
{
    public sealed class UITutorialStageCollectCleanEnergy :
        UITutorialAreaFocusedStage<TutorialStageControllerCollectCleanEnergy>
    {
        [SerializeField]
        private Transform m_interactionArea;

        private IdleObject firstIdleObject;

        private void Awake()
        {
            this.m_interactionArea.SetInvisible();
        }

        protected override void Start()
        {
            base.Start();
            
            var idleObjectInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            this.firstIdleObject = idleObjectInteractor.GetIdleObject("io_1");
            if (!this.firstIdleObject.isWorking)
            {
                this.OnIdleObjectWorkOver();
            }
            else
            {
                this.firstIdleObject.OnWorkOver += this.OnIdleObjectWorkOver;
            }

            var pointerEnterHandler = this.buttonFocus.GetComponent<PointerEnterHandler>();
            pointerEnterHandler.OnPointerEnterEvent.AddListener(data => this.CollectSoftCurrency());
            
            SetActiveIdleObjectHighlighter(IDLE_OBJECT_1_ID, IDLE_OBJECT_1_VISUAL_NAME, true);
            SetActiveIdleObjectCanvasHighlighter(IDLE_OBJECT_1_ID, true);
            SetCameraRenderLayerToSpecial();

            TutorialAnalytics.LogTutorialEventInSec("tutor_second_hint", 2);
        }
        
        private void OnIdleObjectWorkOver()
        {
            this.firstIdleObject.OnWorkOver -= this.OnIdleObjectWorkOver;
            this.m_interactionArea.SetVisible();
        }

        protected override void OnFocusButtonClick()
        {
            this.CollectSoftCurrency();
        }

        private void CollectSoftCurrency()
        {
            this.firstIdleObject.CollectCurrency();
        }

        public override void OnStageFinished()
        {
            base.OnStageFinished();

            SetCameraRenderLayerToDefault();
            TutorialAnalytics.LogTutorialEventInSec("tutor_second_hint_close", 2);
        }
    }
}