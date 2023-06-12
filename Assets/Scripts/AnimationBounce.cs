using UnityEngine;

namespace VavilichevGD.Animations {
    public class AnimationBounce : MonoBehaviour {

        [SerializeField] protected Transform transformRoot;
        [SerializeField] protected AnimationCurve bounceCurve;
        [SerializeField] protected bool pingPong;
        [SerializeField] protected float speed = 1f;
        

        private float timer;
        private bool forward;

        protected virtual void OnEnable() {
            this.timer = 0f;
            this.forward = true;
        }


        
        protected virtual void Update() {
            this.CalculateTimer();
            var scaleFactor = this.bounceCurve.Evaluate(this.timer);
            var scale = Vector3.one * scaleFactor;
            this.transformRoot.localScale = scale;
        }
        
        

        protected void CalculateTimer() {
            if (forward) {
                this.timer += Time.deltaTime * speed;
                if (this.timer > 1f) {
                    if (this.pingPong) {
                        this.timer = 2f - this.timer;
                        this.forward = false;
                    }
                    else {
                        this.timer = this.timer - 1f;
                    }
                }
            }
            else {
                this.timer -= Time.deltaTime * speed;
                if (this.timer < 0f) {
                    if (this.pingPong) {
                        this.timer = -this.timer;
                        this.forward = true;
                    }
                    else {
                        this.timer = 1 + this.timer;
                    }
                }
            }
        }
        
        
        #if UNITY_EDITOR
        protected virtual void Reset() {
            this.transformRoot = this.transform;
        }
        #endif
    }
}