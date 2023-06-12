using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.UI.Example;

namespace VavilichevGD.UI {
    public class UILayerTutorial : UILayer {

        [SerializeField] protected Transform container;
        [SerializeField] protected GameObject maskHole;
        [SerializeField] protected Button btnBackground;
        
        protected RectTransform lastHighlightedObject;
        protected int siblingIndexOrigin;
        protected Transform parentOrigin;


        public virtual void HighlightObject(RectTransform rectTransform, UITutorialStep tutorialStep) {
            CancelHighlightingLastObject();
            
            RememberRectTransform(rectTransform);
            PlaceMaskHole(rectTransform, tutorialStep.pointerInebled);
            if (tutorialStep.shiftToFirstPlan)
                ShiftHighlightingObjectToContainer(rectTransform);
            SubscribeToInteraction();
            
            SetActive(true);
        }

        protected virtual void RememberRectTransform(RectTransform rectTransform) {
            if (rectTransform) {
                lastHighlightedObject = rectTransform;
                siblingIndexOrigin = rectTransform.GetSiblingIndex();
                parentOrigin = rectTransform.parent;
            }
        }

        protected virtual void PlaceMaskHole(Transform objectTransform, bool isActive) {
            Debug.Log(objectTransform);
            maskHole.SetActive(isActive);
            if (isActive && maskHole)
                maskHole.transform.position = objectTransform.position;
        }

        protected void ShiftHighlightingObjectToContainer(RectTransform rectTransform) {
            if (rectTransform) {
                rectTransform.parent = container;
                rectTransform.SetAsLastSibling();
            }
        }

        protected void SubscribeToInteraction() {
            Button button = lastHighlightedObject ? lastHighlightedObject.GetComponent<Button>() : null;
            if (button)
                button.onClick.AddListener(OnClick);
            else 
                btnBackground.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick() {
            CancelHighlightingLastObject();

            UITutorialInteractor interactor = Game.GetInteractor<UITutorialInteractor>();
            interactor.MarkStepAsCompleted();
        }
        

        public virtual void CancelHighlightingLastObject() {
            UnsubscribeFromInteraction();
            
            ShiftBackHighlightingIbject();
            lastHighlightedObject = null;
            SetActive(false);
        }

        private void UnsubscribeFromInteraction() {
            Button button = lastHighlightedObject ? lastHighlightedObject.GetComponent<Button>() : null;
            if (button)
                button.onClick.RemoveListener(OnClick);
            else
                btnBackground.onClick.RemoveListener(OnClick);
        }

        protected void ShiftBackHighlightingIbject() {
            if (lastHighlightedObject) {
                lastHighlightedObject.parent = parentOrigin;
                lastHighlightedObject.SetSiblingIndex(siblingIndexOrigin);
            }
        }
    }
}