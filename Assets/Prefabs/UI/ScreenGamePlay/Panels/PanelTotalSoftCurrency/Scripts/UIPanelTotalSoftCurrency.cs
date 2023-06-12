using SinSity.Tools;
using UnityEngine.SocialPlatforms;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI {
	public class UIPanelTotalSoftCurrency : UIPanelCurrency {
		
		protected override void UpdateVisual() {
			var dictionary = BigNumberLocalizator.GetSimpleDictionary();
			properties.textTotalCollected.text  = this.uiBank.softCurrency.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
		}

		protected override void OnEnable() {
			base.OnEnable();
			Localization.OnLanguageChanged += OnLanguageChanged;
		}

		protected override void OnDisable() {
			base.OnDisable();
			Localization.OnLanguageChanged -= OnLanguageChanged;
		}

		protected override void SubscribeOnEvents() {
			this.uiBank.OnSoftCurrencyReceivedEvent += this.OnSoftCurrencyReceived;
			this.uiBank.OnSoftCurrencySpentEvent += this.OnSoftCurrencySpent;
		}

		protected override void UnsubscribeFromEvents() {
			this.uiBank.OnSoftCurrencyReceivedEvent -= this.OnSoftCurrencyReceived;
			this.uiBank.OnSoftCurrencySpentEvent -= this.OnSoftCurrencySpent;
		}


		#region Events

		private void OnLanguageChanged() {
			UpdateVisual();
		}
        
		private void OnSoftCurrencyReceived(object sender, BigNumber bigNumber) {
			this.UpdateVisual();
			this.properties.PlayBounce();
		}
        
		private void OnSoftCurrencySpent(object sender, BigNumber bigNumber) {
			this.UpdateVisual();
		}
        
		#endregion
		
	}
}