using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    public class UIPanelTextWithFitter : MonoBehaviour {
        [SerializeField] private Text textValue;
        [SerializeField] private bool updateOnStart = false;

        private void Start() {
            if (updateOnStart)
                UpdateVisual();
        }

        private void UpdateVisual() {
            RectTransform rectTransform = textValue.transform.parent as RectTransform;
            rectTransform.RecalculateWithHorizontalFitterInside(ContentSizeFitter.FitMode.PreferredSize);
        }

        public void SetText(string text) {
            textValue.text = text;
            UpdateVisual();
        }

        private void Reset() {
            if (!textValue)
                textValue = gameObject.GetComponentInChildren<Text>();
        }
    }
}