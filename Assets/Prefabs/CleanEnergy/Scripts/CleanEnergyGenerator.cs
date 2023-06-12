using UnityEngine;

namespace IdleClicker {
	public class CleanEnergyGenerator : MonoBehaviour {
		public static CleanEnergyGenerator instance { get; private set; }
		public static bool isInitialized { get { return instance != null; } }

		[SerializeField]
		private Transform targetPoint;
		[SerializeField]
		private CleanEnergyVisual cleanEnegryVisualPref;

		private void Start() {
			//InitTargetPointPosition();
		}

		private void InitTargetPointPosition() {
			//Transform targetPointUI = UIIdleController.instance.cleanEnergyTargetTransform;
			//Vector3 worldPositionRargetPointUI = targetPointUI.position;// Camera.main.ScreenToWorldPoint(targetPointUI.position);
			//Vector3 newPosition = new Vector3(worldPositionRargetPointUI.x, worldPositionRargetPointUI.y, targetPoint.position.z);
			//targetPoint.position = newPosition;
		}

		private Vector3 GetRandomPosition(Vector3 centerPosition) {
			float rX = Random.Range(-1f, 1f);
			float rY = Random.Range(-1f, 1f);
			float z = centerPosition.z;
			float rDistance = Random.Range(0f, 0.1f);
			return centerPosition + new Vector3(rX, rY, z).normalized * rDistance;
		}

		private void OnEnable() {
			//IdleObject.OnIdleObjectTapAndReceivedCurrency += IdleObject_OnIdleObjectTapAndReceivedCurrency;
		}

//		private void IdleObject_OnIdleObjectTapAndReceivedCurrency(IdleObject idleObject) {
//			PanelCollectedCurrencyLeaf panelCollectedCurrencyLeaf = idleObject.GetComponentInChildren<PanelCollectedCurrencyLeaf>(true);
//			CreateEnergy(panelCollectedCurrencyLeaf.transform.position);
//		}

		private void OnDisable() {
			//IdleObject.OnIdleObjectTapAndReceivedCurrency -= IdleObject_OnIdleObjectTapAndReceivedCurrency;
		}

		public void CreateEnergy(Vector3 centerPosition) {
			InitTargetPointPosition();
			CleanEnergyVisual createdEnergy = Instantiate(cleanEnegryVisualPref);
			createdEnergy.transform.position = GetRandomPosition(centerPosition);
			createdEnergy.SetTargetPoint(targetPoint);
		}
	}
}