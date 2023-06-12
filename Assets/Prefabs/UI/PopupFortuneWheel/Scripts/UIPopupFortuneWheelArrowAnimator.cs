using SinSity.Meta;
using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
    public class UIPopupFortuneWheelArrowAnimator : AnimObject {

        #region CONSTANTS

        private static readonly int triggerTrigger = Animator.StringToHash("trigger");

        #endregion

        private FortuneWheel fortuneWheel;

        private void Awake() {
            var uiWidgetFortuneWheel = this.GetComponentInParent<UIWidgetFortuneWheel>();
            this.fortuneWheel = uiWidgetFortuneWheel.fortuneWheel;
        }

        private void OnEnable() {
            this.fortuneWheel.OnSeparatorAngleReachedEvent += this.OnSeparatorAngleReached;
        }

        private void OnDisable() {
            this.fortuneWheel.OnSeparatorAngleReachedEvent -= this.OnSeparatorAngleReached;
        }


        #region EVENTS

        private void OnSeparatorAngleReached(FortuneWheel fortunewheel) {
            this.animator.SetTrigger(triggerTrigger);
        }

        #endregion
    }
}