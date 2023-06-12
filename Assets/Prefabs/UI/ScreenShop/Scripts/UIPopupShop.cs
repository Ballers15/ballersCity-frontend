using System;
using System.Collections;
using System.Numerics;
using Orego.Util;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.UI;
using Vector3 = UnityEngine.Vector3;

namespace SinSity.UI {
	public class UIPopupShop : UIPopup<UIPopupShopProperties, UIPopupArgs> {
		[SerializeField] private Button btnClose;
		[SerializeField] private UIShopTabWithPanel[] tabPanels;

		public delegate void ShopStateChangeHandler(UIPopupShop popupShop, bool isActive);
		public static event ShopStateChangeHandler OnUIPopupShopStateChanged;

		private bool isPositionContentDefaultInitialized;
		private Vector3 positionContentDefault;
		private UIShopTabWithPanel activePanel;

		public override void Initialize() {
			base.Initialize();
			HideInstantly();
		}

		protected override void NotifyAboutStateChanged(bool activated) {
			base.NotifyAboutStateChanged(activated);
			OnUIPopupShopStateChanged?.Invoke(this, activated);
		}

		public void ActivateTimeBoosterFX() {
			properties.fxTimeBooster.SetActive(true);
		}

		private void OnEnable() {
			if (!Game.isInitialized) return;
			
			properties.fxTimeBooster.SetActive(false);

			if (activePanel != null) 
				ChangeActivePanel(tabPanels[0]);
			else {
				activePanel = tabPanels[0];
			}
			
			activePanel.ActivatePanel();
			UIShopTabWithPanel.OnPanelActivated += ChangeActivePanel;
			btnClose.onClick.AddListener(OnCloseBtnClick);
			//TryToScrollRelative();
		}

		private void OnDisable() {
			UIShopTabWithPanel.OnPanelActivated -= ChangeActivePanel;
			btnClose.onClick.RemoveListener(OnCloseBtnClick);
		}

		private void ChangeActivePanel(UIShopTabWithPanel panel) {
			activePanel.DeactivatePanel();
			activePanel = panel;
		}

		public void ScrollToCases() {
			//StartCoroutine(ScrollViewToRoutine(properties.rectPanelCases));
		}

		private IEnumerator ScrollViewToRoutine(RectTransform rectTarget) {
			ScrollRect scrollRect = properties.rectContent.GetComponentInParent<ScrollRect>();
			Vector3 positionTarget = scrollRect.transform.InverseTransformPoint(properties.rectContent.position)
				- scrollRect.transform.InverseTransformPoint(rectTarget.position);

			float timer = 0f;
			float duration = 1f;
			while (timer < 1f) {
				timer = Mathf.Min(timer + Time.deltaTime / duration, 1f);
				Vector3 newPosition = Vector3.Lerp(positionContentDefault, positionTarget, timer);
				properties.rectContent.anchoredPosition = newPosition;
				yield return null;
			}
		}

		private void ScrollToInstanly(RectTransform rectTarget) {
			ScrollRect scrollRect = properties.rectContent.GetComponentInParent<ScrollRect>();
			Vector3 positionTarget = scrollRect.transform.InverseTransformPoint(properties.rectContent.position)
			                         - scrollRect.transform.InverseTransformPoint(rectTarget.position);
			properties.rectContent.anchoredPosition = positionTarget;
		}
		
		private void OnCloseBtnClick() {
			NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Close));
			Hide();
		}
	}
}