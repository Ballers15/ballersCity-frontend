using TMPro;
using UnityEngine;

namespace SinSity.UI {
    public class ProgressBarMaskedWithTMPro : ProgressBarMasked {
        [SerializeField] private TextMeshProUGUI textValueField;

        public void SetTextValue(string textValue) {
            textValueField.text = textValue;
        }
    }
}