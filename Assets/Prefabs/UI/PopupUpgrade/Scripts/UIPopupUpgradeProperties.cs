using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIPopupUpgradeProperties : UIProperties {
        public Text textHeader;
        public Image imgIcon;
        public Image imgProductIcon;
        public Text textDescription;
        public Button btnClose;
        public Button btnPrev;
        public Button btnNext;
        public WidgetLevelUp widgetLevelUp;
        public AudioClip audioClipShow;
        public AudioClip audioClipClose;
        public Transform contentContainer;
    }
}