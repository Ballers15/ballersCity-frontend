using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIWidgetComplexResearchRewardProperties : UIProperties {
        [SerializeField] private Image imgIcon;
        [SerializeField] private TextMeshProUGUI textDescription;

        public void SetIcon(Sprite spriteIcon) {
            imgIcon.sprite = spriteIcon;
        }

        public void SetDescription(string descriptionText) {
            textDescription.text = descriptionText;
        }

        public Vector3 GetIconPosition() {
            return imgIcon.transform.position;
        }
    }
}