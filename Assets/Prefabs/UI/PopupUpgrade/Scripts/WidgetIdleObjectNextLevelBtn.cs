using System.Collections;
using SinSity.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using VavilichevGD;

namespace SinSity.UI {
    public class WidgetIdleObjectNextLevelBtn : AnimObject, IPointerDownHandler, IPointerUpHandler {
        private IdleObject _idleObject;
        private bool _pointerInside;
        private static readonly int TRIGGER_NOT_ENOUGH_MONEY = Animator.StringToHash("not_enough_money");
        private static readonly int TRIGGER_CLICK = Animator.StringToHash("click");


        public void Setup(IdleObject idle) {
            _idleObject = idle;
        }

        public void OnPointerDown(PointerEventData eventData) {
            _pointerInside = true;
            StartCoroutine("LifeRoutine");
        }

        public void OnPointerUp(PointerEventData eventData) {
            StopCoroutine("LifeRoutine");
            _pointerInside = false;
        }

        private IEnumerator LifeRoutine() {
            var periodDelay = 0.2f;
            var periodDelayMin = 0.03f;
            var periodDelayStep = 0.02f;
            while (_pointerInside) {
                OnBtnClick();
                yield return new WaitForSeconds(periodDelay);
                periodDelay = Mathf.Max(periodDelay - periodDelayStep, periodDelayMin);
            }
        }

        private void OnBtnClick() {
            _idleObject.NextLevel();
        }

        private void PlayClickAnim() {
            SetTrigger(TRIGGER_CLICK);
        }

        private void OnEnable() {
            if (_idleObject == null) return;
            
            _pointerInside = false;
            _idleObject.OnLevelRisenEvent += OnLevelRisen;
        }

        private void OnLevelRisen(int newlevel, bool success) {
            if (success) {
                PlayClickAnim();
            }
            else {
                SetTrigger(TRIGGER_NOT_ENOUGH_MONEY);
            }
        }

        private void OnDisable() {
            if (_idleObject == null) return;
            
            _idleObject.OnLevelRisenEvent -= OnLevelRisen;
        }
    }
}