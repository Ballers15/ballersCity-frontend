using System;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using VavilichevGD.UI;
using Random = UnityEngine.Random;

namespace SinSity.UI {
    public abstract class FXGenerator<T> : MonoBehaviour where T : MonoBehaviour {
        protected static Camera mainCamera;
        protected static Camera uiCamera;
        
        [SerializeField] protected T pref;
        [SerializeField] protected int poolCount = 3;

        
        protected Pool<T> fxPool;

        protected virtual void Awake() {
            CreateSingleton();
        }

        protected abstract bool CreateSingleton();
        protected abstract void InitFXPool();

        protected virtual void Start() {
            LateInitialize();
            InitFXPool();
        }
        
        protected virtual void LateInitialize() {
            if (uiCamera && mainCamera)
                return;
            
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            UIController uiController = uiInteractor.uiController;
            uiCamera = uiController.GetComponentInChildren<Camera>();
            mainCamera = Camera.main;
        }

        protected static Vector3 GetValidPosition(IObjectEcoClicker objectEcoClicker) {
            return UIController.GetPositionRelativeUICamera(objectEcoClicker);
        }
        
        protected Vector3 GetRandomPositionAround(Vector3 centerPosition, float offset) {
            float rX = Random.Range(-offset, offset);
            float rY = Random.Range(-offset, offset);
            float Z = 0f;
            Vector3 rOffset = new Vector3(rX, rY, Z);
            Vector3 finalPorision = centerPosition + rOffset;
            return finalPorision;
        }

        protected int ClampWithFreeElements(int value) {
            int clampedValue = Mathf.Clamp(this.fxPool.GetFreeElementsCount(), 0, value);
            if (clampedValue == 0)
                throw new Exception($"There is no free elements in the pool: {this.name}");
            return clampedValue;
        }
    }
}