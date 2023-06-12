using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class ProgressBarMasked : ProgressBar {
        [SerializeField] protected Image imgMask;

#if UNITY_EDITOR
        [SerializeField, Range(0f, 1f)] 
        private float value;

        private void OnValidate() {
            if (imgMask == null) return;
            if (imgMask.fillAmount != value)
                imgMask.fillAmount = value;
        }
#endif

        public override void SetValue(float newNormalizedValue) {
            float value = Mathf.Clamp01(newNormalizedValue);
            imgMask.fillAmount = value;
        }

        public void Activate() {
            gameObject.SetActive(true);
        }

        public void Deactivate() {
            gameObject.SetActive(false);
        }
    }
}