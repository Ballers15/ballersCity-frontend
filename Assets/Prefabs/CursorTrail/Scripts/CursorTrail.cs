using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace IdleClicker {
	public class CursorTrail : MonoBehaviour {

		private Transform myTransform;
		private TrailRenderer trailRenderer;

		private Camera cameraUI;

		private bool isInitialized => cameraUI != null;
		
		private void Awake() {
			Initialize();
		}

		private void Initialize() {
			myTransform = transform;
			trailRenderer = GetComponentInChildren<TrailRenderer>(true);
			Game.OnGameInitialized += this.OnGameInitialized;
		}

		private void OnGameInitialized(Game game) {
			Game.OnGameInitialized -= this.OnGameInitialized;
			this.cameraUI = UIController.cameraUI;
		}

		private void LateUpdate() {
#if UNITY_EDITOR
			if (Input.GetMouseButton(0))
				this.FollowPosition(Input.mousePosition);

			if (Input.GetMouseButtonDown(0))
				this.ResetRenderer();
#else
			if (Input.touchCount > 0) {
				FollowPosition(Input.GetTouch(0).position);
				if (Input.GetTouch(0).phase == TouchPhase.Began)
					ResetRenderer();
			}
#endif
		}

		private void FollowPosition(Vector3 position) {
			if (!isInitialized)
				return;
			
			Vector3 worldPosition = this.cameraUI.ScreenToWorldPoint(position);
			Vector3 newPosition = new Vector3(worldPosition.x, worldPosition.y, myTransform.position.z);
			myTransform.position = newPosition;
		}

		private void ResetRenderer() {
			trailRenderer.Clear();
		}
	}
}