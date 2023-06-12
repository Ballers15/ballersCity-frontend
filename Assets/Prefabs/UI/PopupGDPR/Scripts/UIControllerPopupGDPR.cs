using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI {
	public class UIControllerPopupGDPR : MonoBehaviour {
		
		private void Awake() {
			/*if (MaxSdk.IsInitialized()) {
				this.CheckTermsOfServicecAgreement();
				return;
			}

			MaxSdkCallbacks.OnSdkInitializedEvent += this.OnApplovingMaxSDKInitialized;*/
		}

		private void CheckTermsOfServicecAgreement() {
			/*if (!Game.isInitialized) {
				Game.OnGameStart  += this.OnGameStarted;
				return;
			}
			
			if (MaxSdk.HasUserConsent())
				return;

			var uiInteractor = Game.GetInteractor<UIInteractor>();
			var uiController = uiInteractor.uiController;
			var popup = uiController.Show<UIPopupGDPR>();
			popup.OnTermsOfServiceApplyedEvent += OnTermsOfServiceApplyed;*/
		}

		
		#region CALLBACKS
		
		private void OnGameStarted(Game game) {
			Game.OnGameStart  -= this.OnGameStarted;
			this.CheckTermsOfServicecAgreement();
		}

		private void OnTermsOfServiceApplyed(UIPopupGDPR popup) {
			popup.OnTermsOfServiceApplyedEvent -= OnTermsOfServiceApplyed;
			//MaxSdk.SetHasUserConsent(true);
		}

		/*private void OnApplovingMaxSDKInitialized(MaxSdkBase.SdkConfiguration config) {
			MaxSdkCallbacks.OnSdkInitializedEvent -= this.OnApplovingMaxSDKInitialized;
			this.CheckTermsOfServicecAgreement();
		}*/

		#endregion

	}
}