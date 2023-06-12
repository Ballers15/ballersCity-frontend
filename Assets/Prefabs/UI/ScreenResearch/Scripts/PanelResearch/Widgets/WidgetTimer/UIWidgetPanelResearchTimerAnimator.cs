using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    public class UIWidgetPanelResearchTimerAnimator : MonoBehaviour {

        [SerializeField] private float speed = 1f;

        private Material materialCurrent;
        private float offsetCurrent;

        private const string TEXTURE_NAME = "_MainTex";
        
        private void Start() {
            Image img = this.gameObject.GetComponent<Image>();
            materialCurrent = img.material;
            offsetCurrent = 0f;
        }

        private void Update() {
            offsetCurrent = Clamp(offsetCurrent + Time.deltaTime * speed);
            SetMaterialOffset(offsetCurrent);
        }

        private void SetMaterialOffset(float offsetX) {
            float clampedOffsetX = Clamp(offsetX);
            Vector2 offset = new Vector2(clampedOffsetX, 0f);
            materialCurrent.SetTextureOffset(TEXTURE_NAME, offset);
        }

        private float Clamp(float offset) {
            float clampedOffset = offset;
            if (offset > 1f)
                clampedOffset = 1 - offset;
            else if (offset < -1f)
                clampedOffset = offset + 1;
            return clampedOffset;
        }
    }
}