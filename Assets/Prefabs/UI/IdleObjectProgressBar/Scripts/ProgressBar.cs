using UnityEngine;

namespace VavilichevGD.UI {
    public abstract class ProgressBar : MonoBehaviour {

        public abstract void SetValue(float normalizedValue);

        public void Activate() {
            this.gameObject.SetActive(true);
        }

        public void Deactivate() {
            this.gameObject.SetActive(false);
        }
    }
}