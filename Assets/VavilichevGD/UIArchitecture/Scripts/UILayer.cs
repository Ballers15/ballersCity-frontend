using UnityEngine;

namespace VavilichevGD.UI {
    [RequireComponent(typeof(Canvas))]
    public class UILayer : MonoBehaviour {

        protected Canvas canvas;

        protected void Awake() {
            canvas = gameObject.GetComponent<Canvas>();
        }

        public void SetActive(bool isActive) {
            if (isActive) {
                gameObject.SetActive(true);
                canvas.enabled = true;
            }
            else {
                canvas.enabled = false;
                gameObject.SetActive(false);
            }
        }
    }
}