using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [System.Serializable]
    public class UIWidgetCaseProperties : UIProperties {
        public Image imgIcon;
        public GameObject goContainerCount;
        public TextMeshProUGUI textCount;
        public Text textCaseTitle;
        public UIPanelPriceShopItem panelPrice;
        public Button btn;
        [Space] 
        [SerializeField] private Image imgBack;
        [SerializeField] private Sprite spriteBackPurple;
        [SerializeField] private Sprite spriteBackYellow;
        public AudioClip audioClipNotEnoughGems;
        public AudioClip audioClipOpenCase;


        public void ActivatePurpleBack() {
            imgBack.sprite = spriteBackPurple;
        }

        public void ActivateYellowBack() {
            imgBack.sprite = spriteBackYellow;
        }
    }
}