using System;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UICar : MonoBehaviour {
        public event Action OnCarClicked;

        private bool isShowing;
        
        private void OnMouseDown() {
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var uiController = uiInteractor.uiController;
            var notAllowedToClick = !uiController.OnlyGameplayScreenOpened() || BluePrint.bluePrintModeEnabled;
            if (notAllowedToClick) return;
            OnCarClicked?.Invoke();
        }
        
        public void Show(Vector2 position) {
            transform.position = position;
            isShowing = true;
            gameObject.SetActive(true);
        }
        
        public void Hide() {
            isShowing = false;
            gameObject.SetActive(false);
        }

        public bool IsShowing() {
            return isShowing;
        }
    }
}