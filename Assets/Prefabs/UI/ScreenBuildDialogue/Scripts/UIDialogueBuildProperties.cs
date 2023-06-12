using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [System.Serializable]
    public class UIDialogueBuildProperties : UIProperties {
        public Text textHeader;
        public Image imgIcon;
        public Text textDescription;
        public Button btnClose;
        public Button btnBuild;
        public UIPanelPriceBuildIdleObject panelPrice;
        public AudioClip audioClipShow;
        public AudioClip audioClipClose;
    }
}