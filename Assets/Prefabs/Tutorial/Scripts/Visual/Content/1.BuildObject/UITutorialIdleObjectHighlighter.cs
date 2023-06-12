using UnityEngine;
using UnityEngine.Rendering;

namespace SinSity.Core {
    public class UITutorialIdleObjectHighlighter : MonoBehaviour {

        private const string CANVAS_PATH = "[IDLE OBJECT CANVAS]";
        private const string CANVAS_ACTIVE_PANEL_PATH = "[IDLE OBJECT CANVAS]/PanelState/PanelActiveStateInfo";
        private const string CANVAS_BTN_BUILD_PATH = "[IDLE OBJECT CANVAS]/BtnBuild";
        private const string LAYER_UI = "UI";
        private const string LAYER_DEFAULT = "Default";

        private const int ORDER_HIGHLIGHTED = 500;
        private const int ORDER_DEFAULT_PANELS = 100;
        private const int ORDER_DEFAULT_MAIN = 400;

        private const bool OVERRIDE_SORTING = true;

        public void Highlight(Transform transformRoot, string visualName) {
            HighlightVisual(transformRoot, visualName);
            HighlightCanvas(transformRoot);
        }

        public void HighlightVisual(Transform transformRoot, string visualName) {
            string path = $"Visual/{visualName}";
            Transform transformVisual = transformRoot.Find(path);
            SortingGroup sortingGroup = transformVisual.gameObject.GetComponent<SortingGroup>();
            if (!sortingGroup)
                sortingGroup = transformVisual.gameObject.AddComponent<SortingGroup>();

            sortingGroup.sortingLayerName = "UI";
            sortingGroup.sortingOrder = 500;
        }

        public void HighlightCanvas(Transform transformRoot) {
            SetLayer(transformRoot, CANVAS_PATH, OVERRIDE_SORTING, LAYER_UI, ORDER_HIGHLIGHTED);
            SetLayer(transformRoot, CANVAS_ACTIVE_PANEL_PATH, OVERRIDE_SORTING, LAYER_UI, ORDER_HIGHLIGHTED);
            SetLayer(transformRoot, CANVAS_BTN_BUILD_PATH, OVERRIDE_SORTING, LAYER_UI, ORDER_HIGHLIGHTED);
        }

        public void Reset(Transform transformRoot, string visualName) {
            ResetVisual(transformRoot, visualName);
            ResetCanvas(transformRoot);
        }

        public void ResetVisual(Transform transformRoot, string visualName) {
            string path = $"Visual/{visualName}";
            Transform transformVisual = transformRoot.Find(path);
            Debug.Log($"Reset visual: {transformVisual}, visual name: {visualName}");
            SortingGroup sortingGroup = transformVisual.gameObject.GetComponent<SortingGroup>();
            if (sortingGroup)
                Destroy(sortingGroup);
        }

        public void ResetCanvas(Transform transformRoot) {
            SetLayer(transformRoot, CANVAS_PATH, !OVERRIDE_SORTING, LAYER_UI, ORDER_DEFAULT_MAIN);
            SetLayer(transformRoot, CANVAS_ACTIVE_PANEL_PATH, OVERRIDE_SORTING, LAYER_DEFAULT, ORDER_DEFAULT_PANELS);
            SetLayer(transformRoot, CANVAS_BTN_BUILD_PATH, OVERRIDE_SORTING, LAYER_DEFAULT, ORDER_DEFAULT_PANELS);
        }

        private void SetLayer(Transform transformRoot,  string path, bool overrideSorting, string layerName,  int sortingOrder) {
            Transform transformObject = transformRoot.Find(path);
            Canvas canvas = transformObject.GetComponent<Canvas>();
            canvas.overrideSorting = overrideSorting;
                canvas.sortingLayerName = layerName;
                canvas.sortingOrder = sortingOrder;
        }
    }
}