using UnityEngine;

namespace SinSity {
    public class InputManager : MonoBehaviour {

        public delegate void InputHandler();

        public static event InputHandler OnBackKeyClicked;

        public static event InputHandler OnInputTap;
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home)) {
                OnBackKeyClicked?.Invoke();
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnInputTap?.Invoke();
            }
        }
    }
}