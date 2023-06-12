using System;
using Orego.Util;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.UI;

namespace SinSity.UI {
	public class UIPopupGDPR : UIPopupAnim<UIProperties, UIPopupArgs> {

		#region EVENTS

		public event Action<UIPopupGDPR> OnTermsOfServiceApplyedEvent; 

		#endregion

		[SerializeField] private string termsOfServiceURL = "https://aigames.ae/policy";
		[SerializeField] private Button buttonApply;
		[SerializeField] private Button buttonRead;

		private void OnEnable() {
			this.buttonApply.AddListener(this.OnApplyButtonClick);
			this.buttonRead.AddListener(this.OnReadButtonClick);
		}

		private void OnDisable() {
			this.buttonApply.RemoveListener(this.OnApplyButtonClick);
			this.buttonRead.RemoveListener(this.OnReadButtonClick);
		}

		#region CALLBACKS

		private void OnApplyButtonClick() {
			this.OnTermsOfServiceApplyedEvent?.Invoke(this);
			this.Hide();
		}

		private void OnReadButtonClick() {
			Application.OpenURL(this.termsOfServiceURL);
		}

		#endregion
	}
}