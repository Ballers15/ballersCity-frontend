using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SinSity.UI {
    public class UIButtonClickAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        [SerializeField] protected Transform targetTransform;
        [Tooltip("In percents.")]
        [Range(0f, 3f)]
        [SerializeField] protected float clickedScaleFactor = 1f;

        public bool isActive { get; set; }
        
        protected float scaleDefault;

        protected void Awake() {
            Initialize();
        }

        protected virtual void Initialize() {
            isActive = true;
            scaleDefault = targetTransform.localScale.x;
        }

        protected void SetScale(float scale) {
            targetTransform.localScale = Vector3.one * scale;
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (!isActive)
                return;
            
            float newScale = scaleDefault * clickedScaleFactor;
            SetScale(newScale);
        }

        public void OnPointerUp(PointerEventData eventData) {
            SetScale(scaleDefault);
        }

        private void Reset() {
            Button btn = gameObject.GetComponent<Button>();
            if (btn) 
                btn.transition = Selectable.Transition.None;

            if (!targetTransform)
                targetTransform = this.transform;
        }
    }
}