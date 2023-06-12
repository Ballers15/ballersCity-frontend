using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Tools;

namespace SinSity.UI {
	public class PanelCollectedCurrency : MonoBehaviour {

		[SerializeField] private Text textValue;

		public void SetValue(BigNumber collectedCurrency) {
			var dictionary = BigNumberLocalizator.GetSimpleDictionary();
			textValue.text = collectedCurrency > 0 ?  collectedCurrency.ToString(BigNumber.FORMAT_XXX_XC,dictionary) : "";
		}
	}
}