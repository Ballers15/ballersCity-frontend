using Orego.Util;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI {
	public class UICheatCaller : MonoBehaviour {

		[SerializeField] private Button button1;
		[SerializeField] private Button button2;

		[SerializeField] private int combination;

		private string enteredCombination;
		private int buttonClickedCount1;
		private int buttonClickedCount2;
		private Button lastClickedButton;
		
		private void OnEnable() {
			this.ResetEnteredCombination();			
			this.button1.AddListener(this.OnButton1_Click);
			this.button2.AddListener(this.OnButton2_Click);
		}

		private void OnDisable() {
			this.button1.RemoveListener(this.OnButton1_Click);
			this.button2.RemoveListener(this.OnButton2_Click);
		}

		private void ResetEnteredCombination() {
			this.enteredCombination = "";
			this.buttonClickedCount1 = 0;
			this.buttonClickedCount2 = 0;
			this.lastClickedButton = null;
		}

		private void TryToShowPopupCheats(string enteredCombinationForCheck) {
			if (this.combination.ToString() == enteredCombinationForCheck) {
				var uiInteractor = Game.GetInteractor<UIInteractor>();
				var uiController = uiInteractor.uiController;
				uiController.Show<UIPopupCheats>();
			}
		}

		#region CALLBACKS

		private void OnButton1_Click() {
			if (this.lastClickedButton == this.button2) {
				this.enteredCombination += this.buttonClickedCount2.ToString();
				this.buttonClickedCount2 = 0;
			}

			this.buttonClickedCount1++;
			this.lastClickedButton = button1;

			var enteredCombinationForCheck = this.enteredCombination + this.buttonClickedCount1.ToString();
			this.TryToShowPopupCheats(enteredCombinationForCheck);
		}

		private void OnButton2_Click() {
			if (this.lastClickedButton == this.button1) {
				this.enteredCombination += this.buttonClickedCount1.ToString();
				this.buttonClickedCount1 = 0;
			}

			this.buttonClickedCount2++;
			this.lastClickedButton = button2;

			var enteredCombinationForCheck = this.enteredCombination + this.buttonClickedCount2.ToString();
			this.TryToShowPopupCheats(enteredCombinationForCheck);
		}

		#endregion
		
	}
}