using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIPopupModernizationProperties : UIProperties {
        public Button btnApply;
        public Button btnClose;
        public Button btnHint;
        public AudioClip audioClipApply;
        public AudioClip audioClipClose;
    }
}