using IdleClicker;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Core {
    public class Zones : MonoBehaviour {
        
        private CameraController camController;
        private Zone[] zones;

        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            zones = gameObject.GetComponentsInChildren<Zone>();
            camController = FindObjectOfType<CameraController>();
            
            Game.OnGameInitialized += OnGameInitialized;
        }

        private void OnGameInitialized(Game game) {
            Game.OnGameInitialized -= OnGameInitialized;
            FirstSetup();
        }


        private void FirstSetup() {
            int currentIndexZone = camController.indexPointCurrent;
            for (int i = 0; i < zones.Length; i++) {
                bool isActive = i == currentIndexZone;
                zones[i].SetActiveUI(isActive);
                zones[i].SetActiveVisual(isActive);
            }
        }
        

        private void OnEnable() {
            CameraController.OnCameraMovingOver += OnCameraMovingOver;
            CameraController.OnCameraMovingStart += OnCameraMovingStart;
        }

        private void OnCameraMovingOver(int fromPointIndex, int toPointIndex) {
            if (IsValidIndex(fromPointIndex)) {
                zones[fromPointIndex].SetActiveUI(false);
                zones[fromPointIndex].SetActiveVisual(false);
            }
        }

        private void OnCameraMovingStart(int fromPointIndex, int toPointIndex) {
            if (IsValidIndex(toPointIndex)) {
                zones[toPointIndex].SetActiveUI(true);
                zones[toPointIndex].SetActiveVisual(true);                
            }
        }

        private bool IsValidIndex(int index) {
            return index >= 0 && index < zones.Length;
        }

        private void OnDisable() {
            CameraController.OnCameraMovingOver -= OnCameraMovingOver;
            CameraController.OnCameraMovingStart -= OnCameraMovingStart;
        }
    }
}