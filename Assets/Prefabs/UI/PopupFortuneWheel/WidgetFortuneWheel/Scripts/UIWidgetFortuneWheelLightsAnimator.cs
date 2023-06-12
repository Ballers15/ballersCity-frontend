using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
    public class UIWidgetFortuneWheelLightsAnimator : AnimObject {

        #region CONSTANTS

        private static readonly int integerState = Animator.StringToHash("state");
        private const int STATE_IDLE = 0;
        private const int STATE_ROTATE = 1;
        private const int STATE_BLINKING = 2;
        
        #endregion

        
        public void PlayIdle() {
            this.animator.SetInteger(integerState, STATE_IDLE);
        }

        public void PlayRotate() {
            this.animator.SetInteger(integerState, STATE_ROTATE);
        }

        public void PlayBlinking() {
            this.animator.SetInteger(integerState, STATE_BLINKING);
        }
    }
}