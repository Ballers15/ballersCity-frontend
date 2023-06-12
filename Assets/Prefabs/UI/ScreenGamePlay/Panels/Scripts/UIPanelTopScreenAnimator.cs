using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
    public class UIPanelTopScreenAnimator : AnimObject {
        private static readonly int triggerBounce = Animator.StringToHash("bounce");

        public void PlayBounce() {
            SetTrigger(triggerBounce);
        }
    }
}