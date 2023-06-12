using TMPro;
using UnityEngine;

namespace SinSity.UI {
    public class UIWidgetBtnNavigateLocker : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI textLevelValue;

        public void Deactivate() {
            this.gameObject.SetActive(false);
        }

        public void Activate(int requiredLevel) {
            this.gameObject.SetActive(true);
            this.textLevelValue.text = $"{requiredLevel}";
        }
    }
}