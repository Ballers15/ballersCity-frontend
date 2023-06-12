using SinSity.UI;
using UnityEngine;

namespace SinSity.Core {
    public class Zone : MonoBehaviour {

        [SerializeField] private GameObject goVisual;
        [SerializeField] private Transform m_notBuildedSlotContainer;
        [SerializeField] private Transform m_cleanContainer;

        public Transform notBuildedSlotContainer => m_notBuildedSlotContainer;
        public Transform cleanContainer => m_cleanContainer;
        
        public IdleObject[] idleObjects { get; private set; }
        private IdleObjectUI[] idleObjectsUI;
        private IdleObjectVisual[] idleObjectsVisual;
        
        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            idleObjects = gameObject.GetComponentsInChildren<IdleObject>();
            idleObjectsUI = gameObject.GetComponentsInChildren<IdleObjectUI>();
            idleObjectsVisual = gameObject.GetComponentsInChildren<IdleObjectVisual>();
        }

        public void SetActiveUI(bool isActive) {
            foreach (IdleObjectUI objectUi in idleObjectsUI)
                objectUi.SetActive(isActive);
        }

        public void SetActiveVisual(bool isActive) {
            foreach (IdleObjectVisual objectVisual in idleObjectsVisual)
                objectVisual.SetActive(isActive);
            goVisual.SetActive(isActive);
        }
    }
}