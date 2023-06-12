using System;
using SinSity.Core;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;

namespace SinSity.UI {
    public class IdleObjectBtnUpgrade : MonoBehaviour {
        [SerializeField] private Button m_btn;

        public event Action OnButtonClicked;
        
        private IdleObject idleObject;
        private bool isReady;

        private void Awake() {
            idleObject = GetComponentInParent<IdleObject>();
        }

        private void OnEnable() {
            m_btn.onClick.AddListener(OnClicked);
            
            if (Game.isInitialized) {
                UpdateState();
            }
            else {
                void OnGameInitialized(Game game) {
                    Game.OnGameInitialized -= OnGameInitialized;
                    UpdateState();
                }

                Game.OnGameInitialized += OnGameInitialized;
            }
        }

        private void UpdateState() {
            if (idleObject == null || !idleObject.isInitialized)
                return;
            
            if (idleObject.isBuilt && BluePrint.bluePrintModeEnabled)
                SetAsReadyForUpgrade();
            else
                SetAsNOTReadyForUpgrade();
        }

        private void SetAsNOTReadyForUpgrade() {
            isReady = false;
        }

        private void SetAsReadyForUpgrade() {
            isReady = true;
        }

        private void OnDisable() {
            m_btn.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked() {
            if (!isReady) return;
            
            OnButtonClicked?.Invoke();
        }
    }
}