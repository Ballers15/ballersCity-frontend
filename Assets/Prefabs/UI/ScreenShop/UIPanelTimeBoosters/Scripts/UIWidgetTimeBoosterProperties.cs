using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIWidgetTimeBoosterProperties : UIProperties {
        public Text textTitle;
        public GameObject panelTimeBoosterValue;
        public TextMeshProUGUI textCountCurrentValue;
        public Image imgIcon;
        public UIPanelPriceShopItem panelPrice;
        public Button btn;
        public AudioClip audioClipNotEnoughGems;
        public AudioClip audioClipActivateTimeBooster;
    }
}