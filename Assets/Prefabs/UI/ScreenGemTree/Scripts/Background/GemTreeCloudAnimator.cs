using UnityEngine;

namespace SinSity.UI {
    public class GemTreeCloudAnimator : MonoBehaviour {

        [SerializeField] private float speed = 1f;
        [SerializeField] private float targetLocalX = 715f;
        [SerializeField] private float startLocalX = -715f;

        private Transform myTransform;

        private void Start() {
            this.myTransform = this.transform;
        }

        public void ForcedUpdate() {
            Vector3 currentPosition = this.myTransform.localPosition;
            currentPosition.x += Time.deltaTime * this.speed;
            if (currentPosition.x >= targetLocalX)
                currentPosition.x = startLocalX;
            
            this.myTransform.localPosition = currentPosition;
        }
    }
}
