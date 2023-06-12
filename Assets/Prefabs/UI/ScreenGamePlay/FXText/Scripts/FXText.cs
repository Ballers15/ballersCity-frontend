using UnityEngine;
using UnityEngine.UI;
using VavilichevGD;

namespace SinSity.UI {
    public class FXText : AnimObject {
        [SerializeField] private Text textField;

        public void Setup(string text) {
            textField.text = text;
        }

        private void Handle_AnimationOver() {
            Destroy();
        }

        private void Destroy() {
            Destroy(this.gameObject);
        }
    }
}
