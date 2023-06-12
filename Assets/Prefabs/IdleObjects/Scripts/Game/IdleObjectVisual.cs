using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public class IdleObjectVisual : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] additiveContent;

        [Space]
        [SerializeField]
        private GameObject buildedVisual;

        private IdleObject idleObject;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            idleObject = gameObject.GetComponentInParent<IdleObject>();
            idleObject.OnInitialized += OnIdleObjectInitialized;
            this.idleObject.OnResetEvent += this.OnIdleObjectReset;
            this.idleObject.OnBuiltEvent += OnBuiltEvent;
            BluePrint.OnBluePrintStateChanged += OnBlueprintStateChanged;
        }

        private void OnBlueprintStateChanged(bool isactive) {
            if (!idleObject.isBuilt) return;
            
            buildedVisual.SetActive(!isactive);
        }

        private void ActivateVisual() {
            this.gameObject.SetActive(true);
        }

        private void DeactivateVisual() {
            this.gameObject.SetActive(false);
        }

        private void OnBuiltEvent(IdleObject idleobject, IdleObjectState state) {
            buildedVisual.SetActive(true);
        }

        private void OnIdleObjectInitialized()
        {
            UpdateBuildings(idleObject.isBuilt);
        }
        
        private void UpdateBuildings(bool isBuilded)
        {
            buildedVisual.SetActive(idleObject.isBuilt);
        }

        private void OnIdleObjectReset()
        {
            this.buildedVisual.SetActive(false);
        }

        public void SetActive(bool isActive)
        {
            if (isActive)
                this.ActivateVisual();
            else
                this.DeactivateVisual();
            
            foreach (GameObject go in additiveContent)
                go.SetActive(isActive);
        }


        private void OnDestroy()
        {
            this.idleObject.OnBuiltEvent -= OnBuiltEvent;
            this.idleObject.OnInitialized -= this.OnIdleObjectInitialized;
            this.idleObject.OnResetEvent -= this.OnIdleObjectReset;
            BluePrint.OnBluePrintStateChanged += OnBlueprintStateChanged;
        }
    }
}