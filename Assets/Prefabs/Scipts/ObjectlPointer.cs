using UnityEngine;
using UnityEngine.EventSystems;

namespace VavilichevGD {
	public abstract class ObjectlPointer : MonoBehaviour {

		public delegate void ObjectHittedHandler(GameObject hittedObject);
		public static event ObjectHittedHandler OnObjectHitted;

		protected virtual void Update() {
#if (UNITY_ANDROID || UINTY_IOS) && !UNITY_EDITOR
			CheckTap();
#else
			CheckClick();
#endif
		}

		private void CheckClick() {
			if (Input.GetMouseButton(0) && !IsPointerOverGameObjectMouse()) {
				Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				CheckRay(mouseRay);
			}
		}

		protected virtual void CheckRay(Ray ray) {
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				Debug.Log(string.Format("Object hitted: {0}", hit.collider.gameObject));
				NotifyAboutRayHitted(hit.collider.gameObject);
			}
		}

		protected void NotifyAboutRayHitted(GameObject hittedObject) {
			if (OnObjectHitted != null)
				OnObjectHitted(hittedObject);
		}

		protected virtual void CheckTap() {
			if (Input.touchCount > 0 && !IsPointerOverGameObjectTouch()) {
				Ray touchRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				CheckRay(touchRay);
			}
		}

		protected bool IsPointerOverGameObjectTouch() {
			foreach(Touch touch in Input.touches) {
				if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
					return true;
			}
			return false;
		}

		protected bool IsPointerOverGameObjectMouse() {
			return EventSystem.current.IsPointerOverGameObject();
		}
	}
}