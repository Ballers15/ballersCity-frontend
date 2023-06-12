using System.Collections;
using System.Collections.Generic;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Core {
    public class MockedGameManager : MonoBehaviour {
        private IEnumerable<IUpdateListenerInteractor> _updateInteractors;
        private bool _isLaunched;
        
        private void OnEnable() {
            Game.OnGameStart += OnGameStart;
        }

        private void OnGameStart(Game obj) {
            _updateInteractors = Game.GetInteractors<IUpdateListenerInteractor>();
            StartCoroutine(LaunchUpdate());
        }
        
        private IEnumerator LaunchUpdate() {
            yield return null;
            _isLaunched = true;
        }
        
        private void Update() {
            if (!_isLaunched) {
                return;
            }

            var unscaledDeltaTime = Time.unscaledDeltaTime;
            foreach (var updateInteractor in _updateInteractors) {
                updateInteractor.OnUpdate(unscaledDeltaTime);
            }
        }
    }
}