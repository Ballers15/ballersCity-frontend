using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Meta.Rewards;
using Object = UnityEngine.Object;

namespace SinSity.UI {
    [Serializable]
    public class UIPopupNewLevelProperties : UIProperties {
        [SerializeField] private TextMeshProUGUI textLevelNumberValue;
        [Space]
        [SerializeField] private Transform transformContainerRewards;
        [Space]
        [SerializeField] private Text textActionRewardTitle;
        [SerializeField] private Image imgActionRewardIcon;
        [Space]
        [SerializeField] private Button m_btnApply;
        [SerializeField] private AudioClip m_sfxApply;

        public Button btnApply => m_btnApply;
        public AudioClip sfxApply => m_sfxApply;

        public void SetLevelNumber(int levelNumber) {
            textLevelNumberValue.text = $"{levelNumber}";
        }

        public void CleanRewards() {
            foreach (Transform child in this.transformContainerRewards)
                Object.Destroy(child.gameObject);
        }

        public void AddReward(UIWidgetLevelReward prefWidget, Reward reward) {
            UIWidgetLevelReward createdWidget = Object.Instantiate(prefWidget, this.transformContainerRewards);
            createdWidget.Setup(reward);
        }

        public void ApplyRewards() {
            foreach (Transform child in this.transformContainerRewards) {
                UIWidgetLevelReward widget = child.GetComponent<UIWidgetLevelReward>();
                widget.ApplyReward();
            }
        }

        public void SetActionRewardInfo(string titleText, Sprite spriteIcon) {
            this.textActionRewardTitle.text = titleText;
            this.imgActionRewardIcon.sprite = spriteIcon;
        }
        
    }
}