using System;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Scripts {
    public abstract class UISceneElement : MonoBehaviour {
        private void Awake() {
            Game.OnGameInitialized += OnGameInitialized;
        }

        private void OnEnable() {
            OnElementEnable();
        }
        
        private void OnDisable() {
            OnElementDisable();
        }

        private void OnGameInitialized(Game game) {
            OnGameInitialized();
        }

        protected virtual void OnGameInitialized() {
            
        }
        
        protected virtual void OnElementEnable() {
            
        }
        
        protected virtual void OnElementDisable() {
            
        }
    }
}