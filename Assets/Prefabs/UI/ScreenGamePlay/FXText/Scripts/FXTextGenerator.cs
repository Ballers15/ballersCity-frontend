using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class FXTextGenerator : MonoBehaviour {
        private static FXTextGenerator instance;

        [SerializeField] private FXText pref;

        private Camera mainCamera;
        private Camera uiCamera;

        private void Awake() {
            CreateSingleton();
        }

        private void Start() {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            UIController uiController = uiInteractor.uiController;
            uiCamera = uiController.GetComponentInChildren<Camera>();
            mainCamera = Camera.main;
        }

        private bool CreateSingleton() {
            if (instance == null) {
                instance = this;
                return true;
            }
            else {
                Destroy(gameObject);
                return false;
            }
        }

        public static void MakeFX(IObjectEcoClicker iObjectEcoClicker, string text) {
            instance.CreateFXLocal(iObjectEcoClicker, text);
        }

        private void CreateFXLocal(IObjectEcoClicker iObjectEcoClicker, string text) {
            Vector3 validPosition = this.GetValidPosition(iObjectEcoClicker);
            FXText createdFX = Instantiate(instance.pref, instance.transform);
            createdFX.Setup(text);
            createdFX.transform.position = validPosition;
        }
        
        protected Vector3 GetValidPosition(IObjectEcoClicker objectEcoClicker) {
            return UIController.GetPositionRelativeUICamera(objectEcoClicker);
        }
    }
}