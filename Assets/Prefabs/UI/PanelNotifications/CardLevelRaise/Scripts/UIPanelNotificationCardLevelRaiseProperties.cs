using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIPanelNotificationCardLevelRaiseProperties : UIProperties {

        [SerializeField] private TextMeshProUGUI textSpeedValue;
        [SerializeField] private Image imgSpriteIcon;
        [SerializeField] public AudioClip audioClipShow;


        public void SetSpeedValue(int speedMultiplicator) {
            textSpeedValue.text = $"x{speedMultiplicator}";
        }

        public void SetIcon(Sprite spriteIcon) {
            imgSpriteIcon.sprite = spriteIcon;
        }
    }
}