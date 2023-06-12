using System.Collections;
using UnityEngine;
using VavilichevGD.Architecture;

namespace VavilichevGD.UI.Example {
    public class UITutorialInteractor : Interactor {

        private UIController uiController;
        private UITutorialPipeline uiTutorialPipeline;
        private UITutorialRepository uiTutorialRepository;

        private const string TUTORIAL_PIPELINE_NAME = "UITutorialPipeline";

        public override bool onCreateInstantly { get; } = false;

        protected override void Initialize() {
            base.Initialize();
            Game.OnGameInitialized += OnGameInitialized;
        }

        private void OnGameInitialized(Game game) {
            Game.OnGameInitialized -= OnGameInitialized;

            uiTutorialRepository = Game.GetRepository<UITutorialRepository>();
            UITutorialState state = uiTutorialRepository.state;
            if (state.isComplete)
                return;
            
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            uiController = uiInteractor.uiController;
            uiTutorialPipeline = Resources.Load<UITutorialPipeline>(TUTORIAL_PIPELINE_NAME);

            UIController.OnUIStateChanged += OnUiStateChanged;
            CheckTutorial();
        }

        private void OnUiStateChanged() {
            CheckTutorial();
        }

        private void CheckTutorial() {
            int indexPrevious = uiTutorialRepository.state.lastIndexTutorialStep;
            int indexNext = indexPrevious + 1;
            
            if (!uiTutorialPipeline.IsValidIndex(indexNext)) {
                CompleteTutorial();
                return;
            }

            TryToStartNextTutorial(indexNext);
        }

        private void CompleteTutorial() {
            uiTutorialRepository.MarkAsComplete();
            UIController.OnUIStateChanged -= OnUiStateChanged;
            SendEvent_TutorialComplete();
            uiController.DeactivateTutorialLayer();
        }

        private void TryToStartNextTutorial(int index) {
            UITutorialStep nextStep = uiTutorialPipeline.GetStep(index);
            string path = nextStep.uiElementPath;
            RectTransform tutorialObject = uiController.GetRectTransform(path);
            if (tutorialObject)
                uiController.HighlightObject(tutorialObject, nextStep);
        }

        
        
        public void MarkStepAsCompleted() {
            int indexStepPrevious = uiTutorialRepository.state.lastIndexTutorialStep;
            int indexStepCurrent = indexStepPrevious + 1;
            
            if (!uiTutorialPipeline.IsValidIndex(indexStepCurrent))
                return;
            
            uiTutorialRepository.CompleteStep(indexStepCurrent);
            SendEvent_TutorialStepComplete(indexStepCurrent);
            CheckTutorial();
        }

        private void SendEvent_TutorialStepComplete(int indexStepCompleted) {
            // TODO: Just code smth.
        }

        private void SendEvent_TutorialComplete() {
            // TODO: Just code smth.
        }
    }
}