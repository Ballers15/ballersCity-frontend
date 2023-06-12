using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.UI {
    public class UISafeZoneStatic : MonoBehaviour {

        private RectTransform rectTransform;

        private void Awake() {
            this.rectTransform = this.GetComponent<RectTransform>();
            this.SetupSafeArea();
        }

        void SetupSafeArea() {
            var safeArea = Screen.safeArea;

            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;

#if UNITY_EDITOR
            Logging.Log(string.Format(
                "New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                name, safeArea.x, safeArea.y, safeArea.width, safeArea.height, Screen.width, Screen.height));
#endif
        }
    }
}