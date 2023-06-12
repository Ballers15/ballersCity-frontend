using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker.UI.Utils {
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridCellResizerCustom : MonoBehaviour {

        public GridLayoutGroup gridLG;
        public float coefWidth = 1f;
        public float coefHeigth = 1f;


        private void OnEnable() {
            this.RecalculateSize();
        }

        private void RecalculateSize() {
            var rectGrid = this.gridLG.GetComponent<RectTransform>();

            var cellSize = this.gridLG.cellSize;
            var parentHeight = rectGrid.rect.height;

            var cellWidth = parentHeight / this.coefWidth;
            cellSize.x = cellWidth;

            var cellHeight = parentHeight / this.coefHeigth;
            cellSize.y = cellHeight;

            this.gridLG.cellSize = cellSize;
        }
        

#if UNITY_EDITOR
        private void Reset() {
            if (this.gridLG == null)
                this.gridLG = this.GetComponent<GridLayoutGroup>();
        }
        #endif

        private void Update() {
            if (!Application.isPlaying)
                this.RecalculateSize();
        }
    }
}