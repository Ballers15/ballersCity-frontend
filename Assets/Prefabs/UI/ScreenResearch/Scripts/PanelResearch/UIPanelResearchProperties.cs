using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIPanelResearchProperties : UIProperties {
        [SerializeField] public string researchId;
        [SerializeField] private Text textTitle;
        [SerializeField] private Text textDescription;
        [SerializeField] private Transform containerControlWidgets;
        [SerializeField] private Image imgRewardIcon;

        [Space] 
        [SerializeField] private UIWidgetPanelResearchButton prefWidgetBtnStart;
        [SerializeField] private UIWidgetPanelResearchButton prefWidgetBtnGet;
        [SerializeField] private UIWidgetPanelResearchTimer prefWidgetTimer;

        [Space] 
        [SerializeField] private AudioClip m_sfxGetReward;

        public AudioClip sfxGetReward => m_sfxGetReward;


        public void SetTitle(string titleText) {
            textTitle.text = titleText;
        }

        public void SetDescription(string descriptionText) {
            textDescription.text = descriptionText;
        }
        
        public UIWidgetPanelResearchButton CreateWidgetButtonStart() {
            CleanContainer();
            UIWidgetPanelResearchButton createdWidget =
                UnityEngine.Object.Instantiate(prefWidgetBtnStart, containerControlWidgets);
            return createdWidget;
        }

        public UIWidgetPanelResearchButton CreateWidgetButtonGetReward() {
            CleanContainer();
            UIWidgetPanelResearchButton createdWidget =
                UnityEngine.Object.Instantiate(prefWidgetBtnGet, containerControlWidgets);
            return createdWidget;
        }

        public UIWidgetPanelResearchTimer CreateWidgetTimer() {
            CleanContainer();
            UIWidgetPanelResearchTimer createdWidget =
                UnityEngine.Object.Instantiate(prefWidgetTimer, containerControlWidgets);
            return createdWidget;
        }

        private void CleanContainer() {
            foreach (Transform child in containerControlWidgets) {
                IUIWidgetPanelResearch widgetPanelResearch = child.GetComponent<IUIWidgetPanelResearch>();
                widgetPanelResearch.RemoveAllListeners();
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        public Vector3 GetRewardIconPosition() {
            return imgRewardIcon.transform.position;
        }

        public UIWidgetPanelResearchButton GetWidgetButton() {
            return this.containerControlWidgets.GetComponentInChildren<UIWidgetPanelResearchButton>();
        }
    }
}