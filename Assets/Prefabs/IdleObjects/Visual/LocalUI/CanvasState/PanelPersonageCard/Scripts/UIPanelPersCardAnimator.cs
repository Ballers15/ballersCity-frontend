using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
    public class UIPanelPersCardAnimator : AnimObject {
        private static readonly int BOOL_READY = Animator.StringToHash("ready");
//        private static readonly int TRIGGER_LEVEL_UP = Animator.StringToHash("level_up");
//        private static readonly int TRIGGER_NOT_ENOUGH_GEMS = Animator.StringToHash("not_enough_gems");

        public void PlayReadyForPurchase() {
            SetBoolTrue(BOOL_READY);
        }

        public void StopReadyForPurchase() {
            SetBoolFalse(BOOL_READY);
        }

//        public void PlayLevelUp() {
//            SetTrigger(TRIGGER_LEVEL_UP);
//        }

//        public void PlayNotEnoughGems() {
//            SetTrigger(TRIGGER_NOT_ENOUGH_GEMS);
//        }
    }
}