using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.UI {
	public class PanelCollectedCurrencyLeaf : MonoBehaviour {

		public delegate void CollectHandler(Vector3 position, BigNumber rewardValue);
		public static event CollectHandler OnLeafCollected;

		private Transform myTransform;

		private void Awake() {
			Initialize();
		}

		private void Initialize() {
			myTransform = transform;
		}

		public void SetActive(bool isActive) {
			gameObject.SetActive(isActive);
		}

		public void Collect(BigNumber rewardValue) {
			OnLeafCollected?.Invoke(myTransform.position, rewardValue);
		}
	}
}