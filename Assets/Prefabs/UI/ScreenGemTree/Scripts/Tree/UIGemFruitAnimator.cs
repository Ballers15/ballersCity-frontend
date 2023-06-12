using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
    public class UIGemFruitAnimator : AnimObject {

        #region Constant

        private static readonly int boolOpened = Animator.StringToHash("opened");
        private static readonly int boolReady = Animator.StringToHash("ready");
        private static readonly int triggerGrow = Animator.StringToHash("grow");
        private static readonly int triggerCollect = Animator.StringToHash("collect");

        #endregion

        [SerializeField] private GameObject goVisual;


        public void PlayDefaultState() {
            this.Deactivate();
        }


        public void PlayAlreadySmallState() {
            this.Activate();
            this.SetBoolTrue(boolOpened);
            this.SetBoolFalse(boolReady);
        }

        public void PlayAlreadyBigState() {
            this.Activate();
            this.SetBoolTrue(boolOpened);
            this.SetBoolTrue(boolReady);
        }

        public void PlayGrowBig() {
            this.Activate();
            this.SetTrigger(triggerGrow);
        }


        public void PlayCollect() {
            this.SetTrigger(triggerCollect);
            this.ResetTrigger(triggerGrow);
        }

        public void PlayGrowFromSmallToBig() {
            this.SetTrigger(triggerGrow);
        }

        private void Activate() {
            this.goVisual.SetActive(true);
            this.animator.enabled = true;
            this.ResetAnimator();
        }

        private void ResetAnimator() {
            this.ResetTrigger(triggerCollect);
            this.ResetTrigger(triggerGrow);
            this.SetBoolFalse(boolOpened);
            this.SetBoolFalse(boolReady);
        }

        private void Deactivate() {
            this.goVisual.SetActive(false);
            this.animator.enabled = false;
            this.ResetAnimator();
        }
    }
}