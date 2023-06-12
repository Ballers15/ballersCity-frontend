using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [System.Serializable]
    public class UIWidgetGemsProperties : UIProperties {
        public Image imgIcon;
        public Text textCountValue;
        public Text textTitle;
        public UIPanelPriceGems panelPrice;
        public Button btn;
        public AudioClip audioClipClick;

        [Space] 
        [SerializeField] private Image imgBack;
        [SerializeField] private Sprite spriteBackPurple;
        [SerializeField] private Sprite spriteBackYellow;
        
        public void ActivateBackPurple() {
            imgBack.sprite = spriteBackPurple;
        }

        public void ActivateBackYellow() {
            imgBack.sprite = spriteBackYellow;
        }
    }
}