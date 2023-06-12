using UnityEngine;

namespace SinSity.UI {
    public abstract class FXUIFlyingReward<T> : MonoBehaviour{
        [SerializeField] protected float maxSpeed = 1f;

        [HideInInspector] public float speed = 1f;

        protected T rewardCurrent;
        protected Transform myTransform;
        protected Transform target;
        protected Vector3 startPosition;
        protected float timer;


        #region Initializing

        protected virtual void Awake() {
            Initialize();
        }

        protected virtual void Initialize() {
            myTransform = transform;
        }

        #endregion

        #region Reset

        protected virtual void OnEnable() {
            Reset();
        }

        protected virtual void Reset() {
            timer = 0f;
        }

        #endregion


        public void SetTarget(Transform newTarget) {
            target = newTarget;
        }

        public void SetStartPosition(Vector3 position) {
            transform.position = position;
            startPosition = position;
        }

        public void SetReward(T reward) {
            this.rewardCurrent = reward;
        }

        protected virtual void Update() {
            if (timer < 1f) {
                float period = 1f / (maxSpeed * speed);
                timer = Mathf.Min(timer + Time.unscaledDeltaTime / period, 1f);
                Vector3 newPosition = Vector3.Lerp(startPosition, target.position, timer);
                myTransform.position = newPosition;
            }
            else {
                FinishWork();
            }
        }

        protected virtual void FinishWork() {
            ApplyReward();
            Deactivate();
        }

        protected abstract void ApplyReward();

        protected void Deactivate() {
            gameObject.SetActive(false);
        }
    }
}