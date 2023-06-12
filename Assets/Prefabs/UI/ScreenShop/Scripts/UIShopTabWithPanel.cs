using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    public class UIShopTabWithPanel : MonoBehaviour {
        public static event Action<UIShopTabWithPanel> OnPanelActivated;
        
        [SerializeField] private Button buttonTab;
        [SerializeField] private GameObject containerPanel;
        [SerializeField] private RectTransform imageBackground;
        [SerializeField] private float activeAdditionalSize;

        private bool isActive;
        private float defaultSize;

        private void Awake() {
            defaultSize = imageBackground.rect.width;
        }

        private void OnEnable() {
            buttonTab.onClick.AddListener(OnButtonClicked);
        }
        
        private void OnButtonClicked() {
            if (!isActive) {
                ActivatePanel();
                OnPanelActivated?.Invoke(this);
            }
        }

        public void ActivatePanel() {
            if(isActive) return;
            
            isActive = true;
            containerPanel.gameObject.SetActive(true);
            imageBackground.sizeDelta = new Vector2(defaultSize + activeAdditionalSize, imageBackground.rect.height);
        }
        
        public void DeactivatePanel() {
            if(!isActive) return;
            
            isActive = false;
            containerPanel.gameObject.SetActive(false);
            imageBackground.sizeDelta = new Vector2(defaultSize, imageBackground.rect.height);
        }

        private void OnDisable() {
            buttonTab.onClick.RemoveListener(OnButtonClicked);
        }
    }
}