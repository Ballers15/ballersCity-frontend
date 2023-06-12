using UnityEngine;

namespace SinSity.UI {
    public interface IObjectEcoClicker {
        Vector3 GetPositionRelativeCameraUI(Camera cameraUI, Camera cameraMain);
    }
}