using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.UI {
    public class UISafeAreaDynamic : MonoBehaviour {
      
        private RectTransform rectTransform;
        private Rect lastSafeArea = new Rect(0, 0, 0, 0);

        private void Awake() {
            this.rectTransform = this.GetComponent<RectTransform>();
            this.Refresh();
        }

        void Update() {
            Refresh();
        }

        void Refresh() {
            var safeArea = Screen.safeArea;

            if (safeArea != lastSafeArea)
                this.ApplySafeArea(safeArea);
        }

        void ApplySafeArea(Rect r) {
            lastSafeArea = r;

            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            var anchorMin = r.position;
            var anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;

#if UNITY_EDITOR
            Logging.Log(string.Format(
                "New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                name, r.x, r.y, r.width, r.height, Screen.width, Screen.height));
#endif
        }
    }
}