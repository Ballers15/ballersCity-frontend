using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.UI {
    public class WorldObjectEcoClicker : IObjectEcoClicker {
        public Vector3 worldPosition;

        public WorldObjectEcoClicker(Vector3 worldPosition) {
            this.worldPosition = worldPosition;
        }
        
        public Vector3 GetPositionRelativeCameraUI(Camera cameraUI, Camera cameraMain) {
            Vector3 viewPortRelativeCameraMain = cameraMain.WorldToScreenPoint(this.worldPosition);
            viewPortRelativeCameraMain.z = 0f;
            Vector3 newWorldPositionRelativeCameraUI = cameraUI.ScreenToWorldPoint(viewPortRelativeCameraMain);
            return newWorldPositionRelativeCameraUI;
        }
    }
}