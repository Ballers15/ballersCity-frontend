using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIWidgetLevelRewardProperties : UIProperties {

        [SerializeField] private Image imgIcon;
        [SerializeField] private TextMeshProUGUI textCountValue;

        public void SetCountValueText(string countValueText) {
            this.textCountValue.text = countValueText;
        }

        public void SetIcon(Sprite spriteIcon) {
            this.imgIcon.sprite = spriteIcon;
        }
    }
}