using UnityEngine;

namespace SinSity.Core {
    public class UITutorialStageUpgradeBuildingHighlighter : MonoBehaviour {

        public void Highlight(Transform transformBtn, int order) {
            Canvas canvas = transformBtn.gameObject.GetComponent<Canvas>();
            if (!canvas)
                canvas = transformBtn.gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "UI";
            canvas.sortingOrder = order;
        }

        public void Reset(Transform transformBtn) {
            Canvas canvas = transformBtn.gameObject.GetComponent<Canvas>();
            if (canvas)
                Destroy(canvas);
            Destroy(this);
        }
    }
}