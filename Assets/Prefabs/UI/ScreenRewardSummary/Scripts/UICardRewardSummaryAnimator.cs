using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
    public class UICardRewardSummaryAnimator : AnimObject {
        private static readonly int TRIGGER_APPEAR = Animator.StringToHash("appear");

        public delegate void AnimationOverHandler();
        public event AnimationOverHandler OnAnimationOver;

        private const float SPEED_UP_VALUE = 5f;
        
        private void Handle_AnimationOver() {
            ResetSpeed();
            OnAnimationOver?.Invoke();
        }

        public void SpeedUp() {
            animator.speed = SPEED_UP_VALUE;
        }

        public void ResetSpeed() {
            animator.speed = 1f;
        }

        public void Show() {
            SetTrigger(TRIGGER_APPEAR);
        }
    }
}