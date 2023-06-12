using UnityEngine;

namespace SinSity.Core {
    public class AirShipDroppingBox : MonoBehaviour {
        [SerializeField] private Transform transformBoxShape;
        [SerializeField] private AirShipDroppingBoxAnimator animator;

        public delegate void AnimatorHandler(AirShipDroppingBox droppingBox);
        public event AnimatorHandler OnExplodeStart;
        
        
        public Vector3 boxShapePosition => transformBoxShape.position;

        private void OnEnable() {
            animator.OnExploded += this.OnBoxExploded;
            animator.OnAnimationFinishedEvent += this.OnAnimationFinished;
        }

        private void OnAnimationFinished() {
            Destroy();
        }

        private void OnBoxExploded() {
            OnExplodeStart?.Invoke(this);
        }


        private void OnDisable() {
            animator.OnExploded -= this.OnBoxExploded;
            animator.OnAnimationFinishedEvent -= this.OnAnimationFinished;
        }

        public void Destroy() {
            Destroy(gameObject);
        }
    }
}