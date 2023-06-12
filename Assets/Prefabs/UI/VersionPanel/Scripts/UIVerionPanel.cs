using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker.UI {
	public class UIVerionPanel : MonoBehaviour {
		[SerializeField]
		private Text textVersion;

		private void Start() {
			textVersion.text = string.Format("v. {0}", Application.version.ToString());
		}

		private void Reset() {
			if (!textVersion)
				textVersion = GetComponentInChildren<Text>(true);
		}
	}
}