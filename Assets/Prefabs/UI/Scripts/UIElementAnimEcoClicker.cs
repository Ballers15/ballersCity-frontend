using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class UIElementAnimEcoClicker : UIElement {
        [SerializeField] protected Animator animator;
        
        private static readonly int triggerClose = Animator.StringToHash("close");

        protected void Handle_AnimationOutOver() {
            HideInstantly();
        }

        public override void Hide() {
            animator.SetTrigger(triggerClose);
        }

        protected void SetBoolTrue(int id) {
            animator.SetBool(id, true);
        }

        protected void SetBoolFalse(int id) {
            animator.SetBool(id, false);
        }

        protected void SetBoolTrue(string id) {
            animator.SetBool(id, true);
        }

        protected void SetBoolFalse(string id) {
            animator.SetBool(id, false);
        }

        protected void SetTrigger(int id) {
            animator.SetTrigger(id);
        }

        protected void SetTrigger(string id) {
            animator.SetTrigger(id);
        }
    }
}