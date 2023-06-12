using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using SinSity.Repo;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using Object = UnityEngine.Object;

namespace SinSity.Domain
{
    public sealed class TutorialPipelineInteractor : Interactor
    {
        #region Const

        private const string TUTORIAL_PIPELINE_PATH = "Pipeline/TutorialStagePipeline";

        #endregion

        #region Events

        public event Action<TutorialStageController> OnControllerStateChangedEvent;

        public event Action<TutorialStageController> OnCurrentControllerChangedEvent;

        public event Action OnTutorialCompleteEvent;

        #endregion

        public bool isTutorialCompleted { get; private set; }

        public TutorialStageController currentStageController { get; private set; }

        private new TutorialRepository repository;

        private TutorialStagePipeline stagePipeline;

        public override bool onCreateInstantly { get; } = true;

        public T GetStageCurrentController<T>() where T : TutorialStageController
        {
            return (T) this.currentStageController;
        }

        #region Initialize


        protected override void Initialize() {
            base.Initialize();
            var pipelineAsset = Resources.Load<TutorialStagePipeline>(TUTORIAL_PIPELINE_PATH);
            this.stagePipeline = Object.Instantiate(pipelineAsset);
            
            this.repository = this.GetRepository<TutorialRepository>();
            var stageControllers = this.stagePipeline.GetStageControllers();
            foreach (var stageController in stageControllers)
            {
                stageController.OnInitialize();
            }

            this.InitializeWithState();
            TutorialAnalytics.Start();
        }

        private void InitializeWithState()
        {
            var tutorialStatistics = this.repository.GetStatistics();
            if (tutorialStatistics.isCompleted)
            {
                this.isTutorialCompleted = true;
                return;
            }

            var currentStageId = tutorialStatistics.currentStageId;
            if (string.IsNullOrEmpty(currentStageId))
            {
                this.currentStageController = this.stagePipeline.GetStageController(0);
                this.currentStageController.OnStateChangedEvent += this.OnControllerStateChanged;
                this.currentStageController.OnTriggeredEvent += this.OnControllerTriggered;
                this.currentStageController.OnBeginListen();
                this.SaveToRepository(this.currentStageController);
            }
            else
            {
                this.currentStageController = this.stagePipeline.GetStageController(currentStageId);
                this.currentStageController.OnStateChangedEvent += this.OnControllerStateChanged;
                this.currentStageController.OnTriggeredEvent += this.OnControllerTriggered;
                var currentStageJson = tutorialStatistics.currentStageJson;
                this.currentStageController.OnContinueListen(currentStageJson);
            }
        }

        #endregion

        private void SaveToRepository(TutorialStageController tutorialStageController = null)
        {
            string json = null;
            string id = null;
            if (tutorialStageController != null)
            {
                json = tutorialStageController.GetState();
                id = tutorialStageController.id;
            }

            var tutorialStatistics = new TutorialStatistics
            {
                isCompleted = this.isTutorialCompleted,
                currentStageId = id,
                currentStageJson = json
            };

            this.repository.Update(tutorialStatistics);
        }

        #region Events

        private void OnControllerStateChanged(TutorialStageController tutorialStageController)
        {
            this.SaveToRepository(tutorialStageController);
            this.OnControllerStateChangedEvent?.Invoke(tutorialStageController);
        }

        private void OnControllerTriggered(TutorialStageController tutorialStageController)
        {
            if (tutorialStageController != this.currentStageController)
            {
                throw new Exception("Only current controller can trigger!");
            }

            this.currentStageController.OnStateChangedEvent -= this.OnControllerStateChanged;
            this.currentStageController.OnTriggeredEvent -= this.OnControllerTriggered;
            this.currentStageController.OnEndListen();
            var id = this.currentStageController.id;
            if (!this.stagePipeline.HasNextStageController(id, out var nextStageController))
            {
                this.isTutorialCompleted = true;
                this.SaveToRepository();
                this.OnTutorialCompleteEvent?.Invoke();
                return;
            }

            this.currentStageController = nextStageController;
            this.currentStageController.OnStateChangedEvent += this.OnControllerStateChanged;
            this.currentStageController.OnTriggeredEvent += this.OnControllerTriggered;
            this.currentStageController.OnBeginListen();
            this.SaveToRepository(this.currentStageController);
            this.OnCurrentControllerChangedEvent?.Invoke(this.currentStageController);
        }

        #endregion
    }
}