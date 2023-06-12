using UnityEngine;

namespace SinSity.UI {
    public class GemTreeBackgroundAnimator : MonoBehaviour {

        private GemTreeCloudAnimator[] clouds;

        private void Start() {
            this.clouds = this.gameObject.GetComponentsInChildren<GemTreeCloudAnimator>();
        }

        private void Update() {
            foreach (GemTreeCloudAnimator cloud in this.clouds)
                cloud.ForcedUpdate();
        }
    }
}