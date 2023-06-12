using System;
using System.Collections;
using SinSity.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using VavilichevGD;

namespace SinSity.UI {
	public class IdleObjectNextLevelBtn : AnimObject, IPointerDownHandler, IPointerUpHandler {
		private IdleObject idleObject;
		private bool pointerInside;
		private static readonly int TRIGGER_NOT_ENOUGH_MONEY = Animator.StringToHash("not_enough_money");
		private static readonly int TRIGGER_CLICK = Animator.StringToHash("click");

		private void Awake() {
			Initialize();
		}

		private void Initialize() {
			idleObject = gameObject.GetComponentInParent<IdleObject>();
			idleObject.OnInitialized += IdleObject_OnInitialized;
		}

		private void IdleObject_OnInitialized() {
			idleObject.OnInitialized -= IdleObject_OnInitialized;
		}

		public void OnPointerDown(PointerEventData eventData) {
			pointerInside = true;
			StartCoroutine("LifeRoutine");
		}
		
		public void OnPointerUp(PointerEventData eventData) {
			StopCoroutine("LifeRoutine");
			pointerInside = false;
		}
		
		private IEnumerator LifeRoutine() {
			float periodDelay = 0.2f;
			float periodDelayMin = 0.03f;
			float periodDelayStep = 0.02f;
			while(pointerInside) {
				OnBtnClick();
				yield return new WaitForSeconds(periodDelay);
				periodDelay = Mathf.Max(periodDelay - periodDelayStep, periodDelayMin);
			}
		}

		private void OnBtnClick() {
			idleObject.NextLevel();
		}
		
		private void PlayClickAnim() {
			SetTrigger(TRIGGER_CLICK);
		}

		private void OnEnable() {
			pointerInside = false;
			idleObject.OnLevelRisenEvent += OnLevelRisen;
		}

		private void OnLevelRisen(int newlevel, bool success) {
			if (success)
            {
				PlayClickAnim();
            }
            else
            {
				SetTrigger(TRIGGER_NOT_ENOUGH_MONEY);
            }
		}

		private void OnDisable() {
			idleObject.OnLevelRisenEvent -= OnLevelRisen;
		}
	}
}