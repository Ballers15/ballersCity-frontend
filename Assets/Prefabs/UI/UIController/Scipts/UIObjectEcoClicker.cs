using UnityEngine;

namespace SinSity.UI {
    public class UIObjectEcoClicker : IObjectEcoClicker{
        public Vector3 worldPosition;

        public UIObjectEcoClicker(Vector3 worldPosition) {
            this.worldPosition = worldPosition;
        }
        
        public Vector3 GetPositionRelativeCameraUI(Camera cameraUI, Camera cameraMain) {
            return worldPosition;
        }
    }
}