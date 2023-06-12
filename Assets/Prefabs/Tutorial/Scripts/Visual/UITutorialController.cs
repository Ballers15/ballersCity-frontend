using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SinSity.UI;
using IdleClicker.UI;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public sealed class UITutorialController : UIWidget<UITutorialControllerProperties>
    {
        private Dictionary<Type, UITutorialStage> uiStagePrefMap;

        private TutorialPipelineInteractor tutorialPipelineInteractor;

        private UITutorialStage currentUITutorialStage;

        private Transform myTransform;

        #region Initialize

        protected override void Awake()
        {
            base.Awake();
            Game.OnGameReady += this.OnGameReady;
            this.myTransform = this.transform;
            this.uiStagePrefMap = this.properties.uiStagePrefs
                .ToDictionary(it => it.GetRequiredControllerType());
        }

        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            this.tutorialPipelineInteractor = this.GetInteractor<TutorialPipelineInteractor>();
            this.tutorialPipelineInteractor.OnCurrentControllerChangedEvent += this.OnCurrentControllerChanged;
            this.tutorialPipelineInteractor.OnTutorialCompleteEvent += this.OnTutorialComplete;
        }

        private void OnGameReady(Game game)
        {
            Game.OnGameReady -= this.OnGameReady;
            if (this.tutorialPipelineInteractor.isTutorialCompleted)
            {
                return;
            }
            
            this.RefreshCurrentStageController();
        }

        #endregion

        #region Events

        private void OnTutorialComplete()
        {
            if (this.currentUITutorialStage != null)
            {
                this.currentUITutorialStage.OnStageFinished();
            }
        }
        
        private void OnCurrentControllerChanged(TutorialStageController obj)
        {
            if (this.currentUITutorialStage != null)
            {
                this.currentUITutorialStage.OnStageFinished();
            }
        }

        private void OnStageFinished()
        {
            Destroy(this.currentUITutorialStage.gameObject);
            this.currentUITutorialStage = null;
            if (!this.tutorialPipelineInteractor.isTutorialCompleted)
            {
                this.RefreshCurrentStageController();
            }
        }
        
        private void RefreshCurrentStageController()
        {
            var currentStageController = this.tutorialPipelineInteractor.currentStageController;
            var controllerType = currentStageController.GetType();
            var uiStagePref = this.uiStagePrefMap[controllerType];
            var uiTutorialStage = Instantiate(uiStagePref, this.myTransform);
#if DEBUG
            Debug.Log("CREATED UI STAGE " + uiTutorialStage.name);
#endif
            this.currentUITutorialStage = uiTutorialStage;
            this.currentUITutorialStage.OnStageFinishedEvent.AddListener(this.OnStageFinished);
            this.currentUITutorialStage.SetController(currentStageController);
        }
        
        #endregion
    }
}